using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTreeView.Models;

namespace MVCTreeView.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            CompositeThing com = new CompositeThing();
            List<CompositeThing> test = new List<CompositeThing>();

            CompositeThing child1 = new CompositeThing();
            child1.Name = "C1";

            CompositeThing child2 = new CompositeThing();
            child2.Name = "C2";
            test.Add(child1);
            test.Add(child2);

            com.Children = test;
            

            CompositeThing Parent = new CompositeThing();
            Parent.Name = "Main";
            com.Parent = Parent;
            return View(com);
        }

    }
}
