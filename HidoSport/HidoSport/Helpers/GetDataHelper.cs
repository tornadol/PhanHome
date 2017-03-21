using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HidoSport.Models;
using HidoSport.Areas.Admin.Helpers;

namespace HidoSport.Helpers
{
    public class GetDataHelper
    {
        private static GetDataHelper _intance;
        public static GetDataHelper Intance
        {
            get
            {
                return _intance ?? (_intance = new GetDataHelper());
            }
        }
        public List<Cate> GetListCate()
        {
            var cacherKey = Cacher.CreateCacheKey("GetCate");
            var model = Cacher.Get<List<Cate>>(cacherKey);
            using (VietCuongEntities ctx = new VietCuongEntities())
            {
                if (model == null)
                {
                    model = (from i in ctx.Cates where String.IsNullOrEmpty(i.flag) && i.Status == 1 orderby i.Sort select i).ToList();
                    Cacher.Add(cacherKey, model);
                }
            }
            return model;
        }
        public List<CateChild> GetListCateChild(int cate)
        {
            var cacherKey = Cacher.CreateCacheKey("GetCate", cate);
            var model = Cacher.Get<List<CateChild>>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = (from i in ctx.CateChilds where String.IsNullOrEmpty(i.flag) && i.CateId == cate && i.Status == 1 orderby i.Sort select i).ToList();
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public Product GetProdcut(Item item)
        {
            var model = new Product()
            {
                Item = item,
                Link = "san-pham/" + item.NameCode,
                Cate = CateName(Convert.ToInt32(item.CateId)),
                CateChild = CateNameChild(Convert.ToInt32(item.CateChildId))
            };
            return model;
        }
        public string CateName(int cateId)
        {
            var cacherKey = Cacher.CreateCacheKey("GetCateCode", cateId);
            var model = Cacher.Get<string>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = (from i in ctx.Cates where i.Id == cateId && String.IsNullOrEmpty(i.flag) select i.Name).FirstOrDefault();
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public string CateNameChild(int cateChildId)
        {
            var cacherKey = Cacher.CreateCacheKey("GetCateCodeChild", cateChildId);
            var model = Cacher.Get<string>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = (from i in ctx.CateChilds where i.Id == cateChildId && String.IsNullOrEmpty(i.flag) select i.Name).FirstOrDefault();
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public Cate GetCateByCode(string code)
        {
            var cacherKey = Cacher.CreateCacheKey("GetCateByCode", code);
            var model = Cacher.Get<Cate>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = (from i in ctx.Cates
                             where i.NameCode == code
                             && String.IsNullOrEmpty(i.flag)
                             select i).FirstOrDefault();
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public CateChild GetCateChildByCode(string code)
        {
            var cacherKey = Cacher.CreateCacheKey("GetCateChildByCode", code);
            var model = Cacher.Get<CateChild>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = (from i in ctx.CateChilds
                             where i.NameCode == code
                             && String.IsNullOrEmpty(i.flag)
                             select i).FirstOrDefault();
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public ListCateItem GetProductByCateId(int id, int page, string search, bool isChild)
        {
            int pageSize = 8;
            string originLink = LinkHelper.GetBase(HttpContext.Current.Request.Url.AbsolutePath);

            var cacheKey = Cacher.CreateCacheKey("GetCateProduct", id, page, search, isChild);
            var model = Cacher.Get<ListCateItem>(cacheKey);
            var lstProduct = new List<Product>();
            var lstItem = new List<DataAccess.Item>();
            int listCount = 0;
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    lstProduct = new List<Product>();
                    if (isChild)
                    {
                        lstItem = (from i in ctx.Items
                                   where i.CateChildId == id
                                   && String.IsNullOrEmpty(i.flag)
                                   && i.Status == 1
                                   && i.NameCode.Contains(search)
                                   orderby i.Create_Day descending
                                   select i).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                        listCount = (from i in ctx.Items
                                     where i.CateChildId == id
                                     && String.IsNullOrEmpty(i.flag)
                                     && i.Status == 1
                                     && i.NameCode.Contains(search)
                                     select i).ToList().Count();
                    }
                    else
                    {
                        lstItem = (from i in ctx.Items
                                   where i.CateId == id
                                   && String.IsNullOrEmpty(i.flag)
                                   && i.Status == 1
                                   && i.NameCode.Contains(search)
                                   orderby i.Create_Day descending
                                   select i).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                        listCount = (from i in ctx.Items
                                     where i.CateId == id
                                     && String.IsNullOrEmpty(i.flag)
                                     && i.Status == 1
                                     && i.NameCode.Contains(search)
                                     select i).ToList().Count();
                    }
                    if (lstItem != null)
                    {
                        foreach (var i in lstItem)
                        {
                            var product = GetProdcut(i);
                            lstProduct.Add(product);
                        }
                        int totalpage = PaginationHelper.GetTotal(pageSize, listCount);
                        List<int> pagination = PaginationHelper.GetPage(page, totalpage, 4);

                        model = new ListCateItem()
                        {
                            LstProduct = lstProduct,
                            OriginLink = originLink,
                            Page = page,
                            Pagination = pagination,
                            Search = search
                        };
                        Cacher.Add(cacheKey, model);
                    }
                }
            }
            return model;
        }
        public Product GetDetailProduct(string namecode)
        {
            var cacherKey = Cacher.CreateCacheKey("GetDetailProduct", namecode);
            var model = Cacher.Get<Product>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    var item = (from i in ctx.Items
                                where i.NameCode == namecode
                                 && String.IsNullOrEmpty(i.flag)
                                 && i.Status == 1
                                select i).FirstOrDefault();
                    model = GetProdcut(item);
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public List<string> GetListImg(int productId, string firstImg)
        {
            var cacherKey = Cacher.CreateCacheKey("GetListImg", productId);
            var model = Cacher.Get<List<string>>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = (from i in ctx.ImageDetails
                             where i.ItemId == productId
                             && String.IsNullOrEmpty(i.flag)
                             select i.Link).ToList();
                    model.Insert(0, firstImg);
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }
        public List<Product> GetListProductById(int? id)
        {
            var cacherKey = Cacher.CreateCacheKey("GetListProductById", id);
            var model = Cacher.Get<List<Product>>(cacherKey);
            if (model == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    model = new List<Product>();
                    var lstItem = (from i in ctx.Items
                                   where i.CateChildId == id && i.Status == 1
                                   orderby i.Create_Day descending
                                   select i).Take(4).ToList();
                    foreach (var i in lstItem)
                    {
                        var product = GetProdcut(i);
                        model.Add(product);
                    }
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }

        public List<LeftMenu> GetNewMenu()
        {
            var cacherKey = Cacher.CreateCacheKey("listNewtmenu");
            var model = Cacher.Get<List<LeftMenu>>(cacherKey);
            if (model == null)
            {
                model = new List<LeftMenu>();
                var lstCate = GetListCate();
                foreach (var cate in lstCate)
                {
                    var lstCateChild = GetListCateChild(cate.Id);
                    var leftMenu = new LeftMenu()
                    {
                        Cate = cate,
                        CateChild = lstCateChild
                    };
                    model.Add(leftMenu);
                }
                Cacher.Add(cacherKey, model);
            }
            return model;
        }

        public List<New> GetNewsHome()
        {
            var cacherKey = Cacher.CreateCacheKey("GetNewsHome");
            var news = Cacher.Get<List<New>>(cacherKey);
            if (news == null)
            {
                using (VietCuongEntities ctx = new VietCuongEntities())
                {
                    news = (from i in ctx.News
                            where i.Status == 1 && String.IsNullOrEmpty(i.flag)
                            orderby i.Create_Day descending
                            select i).Take(5).ToList();
                }
                Cacher.Add(cacherKey, news);
            }
            return news;
        }
    }
}