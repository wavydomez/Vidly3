﻿using System.Web;
using System.Web.Mvc;

namespace Vidly3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //to apply Authorization globally. allow anonymous where you want unauthenticated viewers
            filters.Add(new AuthorizeAttribute());
            //prevent access by http after enabling ssl
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
