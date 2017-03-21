using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TGDD.Library.Caching;

namespace HidoSport.Helpers
{
    public static class Cacher
    {
        private const string CacheInfoDictionaryKey = "CacheInfoDictionary";
        public const string ProductCacheKeyPrefix = "hido.Helper.HomeHelper.HomeHelper";
        public const string CacheActionAll = "dmx@ALL";
        public const string CacheActionByPass = "@BYPASS";
        public static string ClearCacheAction => HttpContext.Current != null ? HttpContext.Current.Request.QueryString["clearcache"] : "";
        public static string UrlDictionary => CreateCacheKeyWithPrefix("Cacher_GetUrlDictionary");

        public enum ClearCacheActions
        {
            /// <summary>
            /// Không xác định
            /// </summary>
            Undefined,

            /// <summary>
            /// Xóa cache
            /// </summary>
            [Description("1")]
            Normal = 1,

            /// <summary>
            /// Xóa tất cả cache
            /// </summary>
            [Description("dmx@ALL")]
            All = 999,

            /// <summary>
            /// Bỏ qua việc lấy cache (không xóa cache)
            /// </summary>
            [Description("@BYPASS")]
            ByPass = 1000
        }

        public static Dictionary<string, CacheInfo> GetCacheInfoDictionary()
        {
            return Get<Dictionary<string, CacheInfo>>(CacheInfoDictionaryKey, neverClear: true) ??
                   new Dictionary<string, CacheInfo>();
        }
        /// <summary>
        /// Tạo cache key với prefix và tham số
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateCacheKeyWithPrefix(string prefix, params object[] parameters)
        {
            if (prefix.StartsWith(ProductCacheKeyPrefix))
                prefix += "__SubSite";

            prefix = prefix.Replace("Async", "");

            if (parameters == null || parameters.Length == 0)
                return prefix;

            if (!prefix.EndsWith("__"))
                prefix += "__";

            var paramString = string.Empty;
            foreach (var param in parameters)
            {
                if (param == null)
                {
                    paramString += "null,";
                }
                else
                {
                    var paramType = param.GetType();
                    paramString += (paramType.IsValueType || paramType == typeof(string) ? param : param.GetMD5Hash()) + ",";
                }
            }

            return prefix + paramString.Trim(',');
        }

        /// <summary>
        /// Tạo cache key với tham số. Prefix sẽ được tự động tạo ra với dạng namespace.class.method của hàm đang được gọi
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateCacheKey(params object[] parameters)
        {
            var stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            var callingMethod = method.Name;
            if (method.ReflectedType != null)
            {
                if (callingMethod == "MoveNext")
                {
                    callingMethod = method.ReflectedType.FullName;
                    MatchCollection matchCollection = Regex.Matches(callingMethod, @"\+<[^>]*>");
                    if (matchCollection.Count > 0)
                    {
                        var methodName = matchCollection[0].ToString();
                        var i = callingMethod.IndexOf("+", StringComparison.Ordinal);

                        callingMethod = callingMethod.Substring(0, i) + "." + methodName.Substring(2, methodName.Length - 3);
                    }
                }
                else
                {
                    callingMethod = method.ReflectedType.FullName + "." + callingMethod;
                }
            }

            return CreateCacheKeyWithPrefix(callingMethod, parameters);
        }
        public static bool Add(string key, object value)
        {
            return Add(key, value, DateTime.Now.AddMinutes(60));
        }
        /// <summary>
        /// Add một object vào cache với thời điểm xác định
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Object cần cache</param>
        /// <param name="absoluteExpiration">Thời điểm hết hạn cache</param>
        /// <returns></returns>
        /// 
        public static bool Add(string key, object value, DateTime absoluteExpiration)
        {
            try
            {
                if (value == null)
                    return false;
                var result = CacheHelper.Add(key, value, absoluteExpiration);
                if (!result)
                    return false;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add một object vào cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>


        /// <summary>
        /// Remove a caching value by special key
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            var key2 = "dmv3_" + key;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Remove(key);
                HttpContext.Current.Cache.Remove(key2);
            }
            HttpRuntime.Cache.Remove(key);
            HttpRuntime.Cache.Remove(key2);
            CacheHelper.Remove(key);
            CacheHelper.Remove(key2);
        }

        /// <summary>
        /// Lấy dữ liệu từ cache. 
        /// Nếu request hiện tại có yêu cầu xóa cache, thì gọi xóa cache và trả về giá trị mặc định của kiểu dữ liệu đó (object: null, number: 0...)
        /// </summary>
        /// <typeparam name="T">Kiểu của dữ liệu được cache</typeparam>
        /// <param name="key">Key của cache</param>
        /// <param name="isRootKey">TRUE: Nghĩa là key thuộc loại RootKey, loại này chỉ bị xóa khi yêu cầu clear cache ALL</param>
        /// <param name="neverClear">TRUE: Nghĩa là không bao giờ clear cache này</param>
        /// <returns></returns>
        public static T Get<T>(string key, bool isRootKey = false, bool neverClear = false)
        {
            if (HttpContext.Current == null)
                return default(T);

            var clearCacheAction = Extension.GetValueFromDescription<ClearCacheActions>(ClearCacheAction);

            if (!neverClear && clearCacheAction != ClearCacheActions.Undefined)
            {
                if (clearCacheAction == ClearCacheActions.ByPass)
                    return default(T);

                if ((clearCacheAction < ClearCacheActions.All && !isRootKey) ||
                    clearCacheAction == ClearCacheActions.All)
                {
                    Remove(key);
                    return default(T);
                }
            }

            var obj = CacheHelper.Get(key);
            if (obj is T)
                return (T)Convert.ChangeType(obj, typeof(T));
            return default(T);
        }


        /// <summary>
        /// Nhóm của cache key
        /// </summary>
        public enum CacheGroup
        {
            Default,
            ProductData
        }

        /// <summary>
        /// Thông tin của một cache key
        /// </summary>
        public class CacheInfo
        {
            public DateTime CreatedDateTime { get; set; }
            public DateTime ExpriedDateTime { get; set; }
            [JsonConverter(typeof(StringEnumConverter))]
            public CacheGroup Group { get; set; }
            public dynamic Filter { get; set; }
            public Uri Url { get; set; }
            public Uri UrlReferrer { get; set; }
        }
    }
}