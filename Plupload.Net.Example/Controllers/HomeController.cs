﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plupload.Net.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomConfig()
        {
            return View();
        }

        public ActionResult AjaxLoading()
        {
            return View();
        }

        public ActionResult LoadPlupload() 
        {
            return this.View();
        }
    }
}
