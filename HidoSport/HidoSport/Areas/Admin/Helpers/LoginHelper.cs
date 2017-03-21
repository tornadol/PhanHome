using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace HidoSport.Areas.Admin.Helpers
{
    public class LoginHelper
    {
        public int CheckUser(string user, string pass)
        {
            PhanHomeEntities ctx = new PhanHomeEntities();
            int key = (from a in ctx.Admin_Login
                       where a.Pass == pass && a.Username == user
                       select a.Id).FirstOrDefault();
            return key;
        }
    }
}