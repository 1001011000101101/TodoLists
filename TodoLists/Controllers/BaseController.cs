using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoLists.Models;
using TodoLists.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace TodoLists.Controllers
{
    public abstract class BaseController : Controller
    {
        public const string ZipContentType = System.Net.Mime.MediaTypeNames.Application.Zip;
        private readonly ApplicationDbContext Db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());

        protected ApplicationDbContext db
        {
            get
            {
                return Db;
            }
        }

        protected string CurrentUserName
        {
            get
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;

                if (!currentUser.Identity.IsAuthenticated)
                {
                    return string.Empty;
                }

                return currentUser.Identity.Name;
            }
        }

        protected ActionResult ViewWithData(string Name, object Data, string Page, string ParentPage = "")
        {
            SetViewBag(Page, ParentPage);
            return ViewWithData(Name, Data);
        }

        protected ActionResult ViewWithData(string Name, object Data)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Data);
            return View(Name);
        }

        protected ActionResult ViewWithData(object Model, object Data)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Data);
            return View(Model);
        }

        protected ActionResult ViewWithData(string Name, object Model, object Data)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Data);
            return View(Name, Model);
        }

        protected ActionResult ViewWithData(object Data)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Data);
            return View();
        }

        protected ActionResult ViewWithData(object Data, string Page, string ParentPage = "")
        {
            SetViewBag(Page, ParentPage);
            return ViewWithData(Data);
        }

        protected ActionResult ViewWithData(object Model, object Data, string Page, string ParentPage = "")
        {
            SetViewBag(Page, ParentPage);
            return ViewWithData(Model, Data);
        }

        protected void SetViewBag(string Page, string ParentPage = "")
        {
            ViewBag.Page = Page;
            ViewBag.ParentPage = ParentPage;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
