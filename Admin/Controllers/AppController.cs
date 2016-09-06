using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using Admin.Repository;
namespace Admin.Controllers
{
    public class AppController : Controller
    {
        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }
    }
}