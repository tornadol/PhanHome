using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HidoSport.Helpers
{
    public static class Extension
    {
        
        
        
        public static string RemoveUnicodeLower(string text)
        {
            text = RemoveUnicode(text);
            return text.ToLower();
        }
        public static string RemoveUnicode(string text)
        {
            text = Regex.Replace(text, "[^\\w\\._ ]", "");

            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                            "đ",
                                            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                            "í","ì","ỉ","ĩ","ị",
                                            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                            "d",
                                            "e","e","e","e","e","e","e","e","e","e","e",
                                            "i","i","i","i","i",
                                            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                            "u","u","u","u","u","u","u","u","u","u","u",
                                            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            text = regex.Replace(text, @" ");
            text = text.Replace(" ", "-");
            return text;
        }
        /// <summary>
        /// Nhân bản một List bất kỳ
        /// </summary>
        /// <typeparam name="T">Class của object được chứa trong list</typeparam>
        /// <param name="listToClone">List cần nhân bản</param>
        /// <returns>List nhân bản</returns>
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        // Convert một danh sách thành mảng 2 chiều
        public static T[][] To2DArray<T>(this IEnumerable<T> source, int columnCount)
        {
            var enumerable = source as T[] ?? source.ToArray();

            var rowCount = enumerable.Length / columnCount;
            if (enumerable.Length % columnCount > 0)
                rowCount++;

            T[][] result = new T[rowCount][];

            for (var r = 0; r < rowCount; r++)
            {
                result[r] = new T[columnCount];
                for (var c = 0; c < columnCount && (r * columnCount + c < enumerable.Length); c++)
                {
                    result[r][c] = enumerable[r * columnCount + c];
                }
            }

            return result;
        }
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            return default(T);
        }

        #region String

        /// <summary>
        /// Kiểm tra xem chuỗi có phải ở dạng ID,ID,ID... hay không?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidatedCommaList(this string s)
        {
            return Regex.IsMatch(s, @"^([0-9]+,)*[0-9]+$");
        }

        /// <summary>
        /// Lấy dạng URL của một chuỗi bất kỳ
        /// </summary>
        /// <param name="phrase">Chuỗi cần chuyển thành URl</param>
        /// <returns>Chuối dạng URL</returns>
        public static string ToURL(this string phrase)
        {
            string str = phrase.ToUnsignedVietnamese().RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-/?:]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            str = str.Substring(0, str.Length <= 150 ? str.Length : 150).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            return str;
        }

        /// <summary>
        /// Chuyển mãi về chuẩn ASCII để đảm bảo loại tất cả các dấu đặt biệt
        /// </summary>
        /// <param name="txt">Chuỗi cần chuyển</param>
        /// <returns>Chuỗi đã mã hóa</returns>
        private static string RemoveAccent(this string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes).Replace("-", " ").Replace("/", " ");
        }

        /// <summary>
        /// Bỏ dấu tiếng Việt của một chuỗi bất kỳ
        /// </summary>
        /// <param name="vietnamese">Chuỗi tiếng Việt cần bỏ dấu</param>
        /// <returns>Chuỗi đã khử dấu</returns>
        private static string ToUnsignedVietnamese(this string vietnamese)
        {
            if (vietnamese == null) return string.Empty;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = vietnamese.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        /// <summary>
        /// Viết hoa các chữ cái đầu của mỗi từ trong một văn bản
        /// </summary>
        /// <param name="value">Chuỗi cần viết hoa</param>
        /// <returns>Chuỗi đã viết hoa</returns>
        public static string ToUpperWords(this string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        /// <summary>
        /// Chuyển một số về dạng tiền tệ
        /// </summary>
        /// <param name="number">Giá trị cần chuyển (int, decimal, double...)</param>
        /// <returns>String dạng tiền tệ</returns>
        public static string ToCurrency(this object number)
        {
            return number.FormatNumber("{0:c}");
        }

        /// <summary>
        /// Chuyển một số về dạng hiển thị có định dạng
        /// </summary>
        /// <param name="number">Giá trị cần chuyển (int, decimal, double...)</param>
        /// <returns>String của số đã được định dạng</returns>
        public static string ToNumberString(this object number)
        {
            return number.FormatNumber("{0:##,#}");
        }

        private static string FormatNumber(this object value, string format)
        {
            if (!value.IsNumericType())
                throw new ArgumentException("\"" + value + "\" is not a number.");
            CultureInfo DefaultCultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("vi-VN").Clone();
            DefaultCultureInfo.NumberFormat.CurrencyDecimalDigits = 0;
            DefaultCultureInfo.NumberFormat.CurrencyPositivePattern = 1;

            var result = string.Format(DefaultCultureInfo, format, value);
            if (result.StartsWith("0"))
                return string.Empty;
            return result;
        }

        public static bool IsValidVietNamPhoneNumber(this string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
                return false;
            const string phonePattern = @"^((09(\d){8})|(01(\d){9}))$";
            return Regex.IsMatch(phoneNum.Trim(), phonePattern);
        }

        public static string ToHexString(this string value)
        {
            var valueBytes = Encoding.ASCII.GetBytes(value);
            return BitConverter.ToString(valueBytes).Replace("-", "");
        }

        public static bool IsPhoneNumber(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;
            const string phonePattern = @"^((09(\d){8})|(01(\d){9}))$";
            return Regex.IsMatch(phone.Trim(), phonePattern);
        }

        public static bool IsEmail(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            const string sMailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.IsMatch(email.Trim(), sMailPattern);
        }
        public static bool IsTaxCode(this string taxtCode)
        {
            try
            {
                taxtCode = taxtCode.Trim();
                if (taxtCode.Trim() == "")
                {
                    return true;
                }
                if (taxtCode.Contains(" "))
                {
                    return false;
                }
                if (taxtCode.Length != 10 &&
                    taxtCode.Length != 14)
                {
                    return false;
                }
                string str = taxtCode.Substring(0, 10);
                if (str == "0000000000")
                {
                    return false;
                }
                Hashtable hashtable = new Hashtable
                {
                    {0, 0x1f},
                    {1, 0x1d},
                    {2, 0x17},
                    {3, 0x13},
                    {4, 0x11},
                    {5, 13},
                    {6, 7},
                    {7, 5},
                    {8, 3}
                };
                decimal num = 0M;
                int num2 = 0;
                foreach (char ch in str)
                {
                    if (num2 == 9)
                    {
                        break;
                    }
                    num += Convert.ToInt32(ch.ToString()) * Convert.ToDecimal(hashtable[num2]);
                    num2++;
                }
                bool bolResult = (Convert.ToDecimal(str.Substring(9, 1)) == (10M - (num % 11M)));
                if (bolResult)
                {
                    if (taxtCode.Length == 14)
                    {
                        str = taxtCode.Substring(10, 4);
                        // Nếu là 13 số thì phải có dấu - sau 10 số
                        bolResult = str[0] == '-' && CheckIsNumber(str.Substring(1, 3));
                    }
                }
                return bolResult;
            }
            catch
            {
                return false;
            }
        }
        private static bool CheckIsNumber(string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber))
            {
                return false;
            }
            string strExp = "[0-9]{" + strNumber.Length + "}";
            return Regex.IsMatch(strNumber, strExp);
        }
        #endregion
    }


}