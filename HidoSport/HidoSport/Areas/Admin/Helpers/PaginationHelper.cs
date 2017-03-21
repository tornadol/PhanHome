using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HidoSport.Areas.Admin.Helpers
{
    public class PaginationHelper
    {
        public static int GetTotal(int select, int total)
        {
            return total % select == 0 ? total / select : total / select + 1;
        }

        public static int GetSkip(int select, int totalpage)
        {
            if (totalpage == 0)
                return 0;
            return select * (totalpage - 1);
        }

        public static List<int> GetPage(int index, int total, int siblings)
        {
            if (total == 0)
                return new List<int>();
            var res = new List<int>();
            if (index == 1)
            {
                for (var i = index; i <= (index + siblings); i++)
                {
                    res.Add(i);
                    if (i == total) break;
                }
                if (index + siblings + 1 <= total)
                    res.Add(total);
            }
            else if (index == total)
            {
                for (var i = index; i >= (index - siblings); i--)
                {
                    res.Add(i);
                    if (i == 1) break;
                }
                if (index - siblings - 1 >= 1)
                    res.Add(1);
            }
            else
            {
                if (index - siblings <= 1)
                    for (var i = 1; i < index; i++)
                        res.Add(i);
                else
                {
                    for (var i = (index - siblings); i < index; i++)
                        res.Add(i);
                    res.Add(1);
                }

                res.Add(index);

                if (index + siblings >= total)
                    for (var i = index + 1; i <= total; i++)
                        res.Add(i);
                else
                {
                    for (var i = index + 1; i <= (index + siblings); i++)
                        res.Add(i);
                    res.Add(total);
                }
            }
            res.Sort((a, b) => a.CompareTo(b));
            return res;
        }
    }
}