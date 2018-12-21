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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TodoLists.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        [Route("Tasks/{ListID:long}")]
        public IActionResult Index(long ListID)
        {
            JsonResult result = new JsonResult(null);

            Models.List list = db.Lists.FirstOrDefault(x => x.ID == ListID && x.UserName == CurrentUserName);
            if (list == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            var tasks = db.Tasks.Where(x => x.ListID == ListID).Select(x => x.ToJson()).ToList();

            var data = new
            {
                tasks,
                List = list.ToJson()
            };

            return ViewWithData(data);
        }

        [AllowAnonymous]
        [Route("Tasks/{UrlShared}")]
        public IActionResult Index(string UrlShared)
        {
            JsonResult result = new JsonResult(null);

            Models.List list = db.Lists.FirstOrDefault(x => x.UrlShared == UrlShared);

            if (list == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            var tasks = db.Tasks.Where(x => x.ListID == list.ID).Select(x => x.ToJson()).ToList();

            var data = new
            {
                tasks,
                List = list.ToJson()
            };

            return ViewWithData(data);
        }

        [HttpPost]
        [Route("Tasks/Add")]
        public ActionResult AddTask(long ListID, string Name, string Description)
        {
            JsonResult result = new JsonResult(null);

            Models.List list = db.Lists.FirstOrDefault(x => x.ID == ListID && x.UserName == CurrentUserName);

            if (list == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            Models.Task task = new Models.Task() { ListID = ListID, Name = Name, Description = Description };
            db.Tasks.Add(task);
            db.SaveChanges();

            result.Value = new
            {
                Success = true,
                ID = task.ID,
                Message = ""
            };

            return result;
        }

        [HttpPost]
        [Route("Tasks/Remove")]
        public ActionResult RemoveTask(long ID)
        {
            JsonResult result = new JsonResult(null);

            Models.Task task = db.Tasks.FirstOrDefault(x => x.ID == ID && x.List.UserName == CurrentUserName);

            if (task == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            result.Value = new
            {
                Success = true,
                Message = ""
            };

            return result;
        }

        [HttpPost]
        [Route("Tasks/Edit")]
        public ActionResult EditTask(long ID, string Name, string Description, bool Completed)
        {
            JsonResult result = new JsonResult(null);

            Models.Task task = db.Tasks.FirstOrDefault(x => x.ID == ID);

            if (task == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            if (task.List.UserName == CurrentUserName)
            {
                task.Name = Name;
                task.Description = Description;
            }

            task.Completed = Completed;
            db.SaveChanges();

            result.Value = new
            {
                Success = true,
                Message = ""
            };

            return result;
        }
    }
}
