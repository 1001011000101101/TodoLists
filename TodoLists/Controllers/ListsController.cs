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
using Newtonsoft.Json;
using System.IO.Compression;
using System.IO;

namespace TodoLists.Controllers
{
    [Authorize]
    public class ListsController : BaseController
    {
        //[Route("Lists")]
        public IActionResult Index()
        {
            var lists = db.Lists.Where(x => x.UserName == CurrentUserName).Select(x => x.ToJson()).ToList();

            var data = new
            {
                lists
            };

            return ViewWithData(data);
        }

        [HttpPost]
        [Route("Lists/Add")]
        public ActionResult AddList(string Name, string UrlShared)
        {
            JsonResult result = new JsonResult(null);
           
            Models.List list = new Models.List() { Name = Name, UrlShared = UrlShared, UserName = CurrentUserName };
            db.Lists.Add(list);
            db.SaveChanges();

            result.Value = new
            {
                Success = true,
                ID = list.ID,
                Message = ""
            };

            return result;
        }

        [HttpPost]
        [Route("Lists/Remove")]
        public ActionResult RemoveList(long ID)
        {
            JsonResult result = new JsonResult(null);

            Models.List list = db.Lists.FirstOrDefault(x => x.ID == ID && x.UserName == CurrentUserName);

            if (list == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            db.Lists.Remove(list);
            db.SaveChanges();

            result.Value = new
            {
                Success = true,
                Message = ""
            };

            return result;
        }

        [HttpPost]
        [Route("Lists/Edit")]
        public ActionResult EditList(long ID, string Name, string UrlShared)
        {
            JsonResult result = new JsonResult(null);

            Models.List list = db.Lists.FirstOrDefault(x => x.ID == ID && x.UserName == CurrentUserName);

            if (list == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            list.Name = Name;
            list.UrlShared = UrlShared;
            db.SaveChanges();

            result.Value = new
            {
                Success = true,
                Message = ""
            };

            return result;
        }


        [Route("Lists/Download")]
        public ActionResult DownloadLists()
        {
            byte[] content = null;
            JsonResult result = new JsonResult(null);
            string id = Guid.NewGuid().ToString();
            FileResult fileResult;

            var lists = db.Lists.Where(x => x.UserName == CurrentUserName).ToList();

            if (lists == null)
            {
                result.Value = new
                {
                    Success = false,
                    Message = Settings.NotFoundMessage
                };
            }

            var serialized = JsonConvert.SerializeObject(lists.Select(x => x.ToJson("download")));

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var zip = archive.CreateEntry("Lists.txt");

                    using (var entryStream = zip.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(serialized);
                    }
                }
                content = memoryStream.ToArray();
                fileResult = File(content, ZipContentType, "Lists.zip");
            }

            return fileResult;
        }
    }
}
