using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APP.Web.eCommerce.Website.Mvc;
using APP.Web.eCommerce.Website.Models;

namespace APP.Web.eCommerce.Website.Controllers
{
    public class UserController : PublicControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult _Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(UserViewModel LoginModel)
        {
            string userData = string.Empty;
            return Json(userData,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Registration(UserViewModel LoginModel)
        {
            string userData = string.Empty;
            return Json(userData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Deposit(BankingViewModel LoginModel)
        {

            string userData = string.Empty;
            return Json(userData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Withdrawal()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Withdrawal(BankingViewModel LoginModel)
        {

            string userData = string.Empty;
            return Json(userData, JsonRequestBehavior.AllowGet);
        }

    }
}