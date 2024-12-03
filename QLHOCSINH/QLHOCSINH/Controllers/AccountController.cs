using QLHOCSINH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QLHOCSINH.Controllers
{
    public class AccountController : Controller
    {
        QLHOCSINHEntities db = new QLHOCSINHEntities();

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> login_post(ACOUNT ac)
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> register_post(ACOUNT ac)
        {
            if (db.ACOUNTS.SingleOrDefault(x => x.username == ac.username) != null)
            {
                return Json(new { Message = "Tài khoản này đã tồn tại", success = false }, JsonRequestBehavior.AllowGet);
            }
            var check_user = await db.ACOUNTS.FirstOrDefaultAsync(x => x.username == ac.username);
            if (check_user == null)
            {
                var add_user = new ACOUNT
                {
                    username = ac.username,
                    password = ac.password,
                    id_role = 1,
                    count_failed_password = 0,
                    is_locked = false,
                    LocketEndTime = null
                };
                db.ACOUNTS.Add(add_user);
            }
            await db.SaveChangesAsync();
            return Json(new { url = "/Account/Login", success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}