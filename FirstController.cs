using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static ImageWebModel imageModel = new ImageWebModel();
        static LogModel logModel = new LogModel();
        static SettingsModel settingsModel = new SettingsModel();
        static PhotosModel photoModel= new PhotosModel();

        public FirstController()
        {
            logModel.update += onLogUpdate;
            settingsModel.update += onConfigUpdate;
        }

        public void onLogUpdate(Object sender, string message)
        {
            Logs();
        }

        public void onConfigUpdate(Object sender, string message)
        {
            int i = -1;
            Config(i);
        }

        public ActionResult Index()
        {
            return View(imageModel);
        }


        public ActionResult Config(int? numHandler)
        {
            if (numHandler != null)
            {
                if (numHandler.Value != -1)
                {
                    string handlerName = settingsModel.handlers[numHandler.Value];
                    settingsModel.DeleteHandler(handlerName);
                }
            }
           
            ViewBag.Title = "Config";
            return View(settingsModel);
        }

        public ActionResult Logs()
        {
            ViewBag.Title = "Logs";
            return View(logModel);
        }

        [HttpPost]
        public ActionResult Logs(LogModel logM)
        {
            if (logM != null && logM.type != "")
            {
                logModel.type = logM.type;
                logModel.makeLogByType();
            }
            ViewBag.Title = "Logs";

            return View(logModel);
        }
   
        public ActionResult viewImage(int numImage)
        {
            Photo photo = new Photo(photoModel.photos.Find(item => item.num == numImage).pathThumbnail, numImage);
            return View(photo);
        }

        public ActionResult deleteImage(int numImage)
        {
            Photo photo = new Photo(photoModel.photos.Find(item => item.num == numImage).pathThumbnail, numImage);
            return View(photo);
        }

        
        public ActionResult deleteHandler(int numHandler)
        {
            string handlerName = settingsModel.handlers[numHandler];
            Handler handler = new Handler(handlerName, numHandler);
            return View(handler);
        }

        public ActionResult Photos(int? numImage)
        {
            if (numImage != null)
            {
                if (numImage.Value != -1)
                {
                    photoModel.removePhoto(numImage.Value);
                    int num = Convert.ToInt32(imageModel.NumOfPic) - 1;
                    imageModel.NumOfPic = num.ToString();
                }
            }
            return View(photoModel);
        }

        public JObject GetLogByType(string type)
        {
            foreach (var empl in logModel.LogsList)
            {
                if (empl.Type == type)
                {
                    
                }
            }
            return null;
        }
    }
}
