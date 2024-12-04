using QLHOCSINH.Helpers;
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
        #region Các Interface
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
        #endregion
        #region Đăng ký lúc chưa mã hóa mật khẩu
        //[HttpPost]
        //[Route("api/register")]
        //public async Task<ActionResult> register_post(ACCOUNT ac)
        //{
        //    if (db.ACCOUNTs.SingleOrDefault(x => x.username == ac.username) != null)
        //    {
        //        return Json(new { Message = "Tài khoản này đã tồn tại", success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    var check_user = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == ac.username);
        //    if (check_user == null)
        //    {
        //        var add_user = new ACCOUNT
        //        {
        //            username = ac.username,
        //            password = ac.password,
        //            id_role = 1,
        //            count_failed_password = 0,
        //            is_locked = false,
        //            LocketEndTime = null
        //        };
        //        db.ACCOUNTs.Add(add_user);
        //    }
        //    await db.SaveChangesAsync();
        //    return Json(new { url = "/Account/Login", success = true }, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region Đăng ký khi đã mã hóa mật khẩu
        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult> register_post(ACCOUNT ac)
        {
            if (db.ACCOUNTs.SingleOrDefault(x => x.username == ac.username) != null)
            {
                return Json(new { Message = "Tài khoản này đã tồn tại", success = false }, JsonRequestBehavior.AllowGet);
            }
            var check_user = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == ac.username);
            if (check_user == null)
            {
                var add_user = new ACCOUNT
                {
                    username = ac.username,
                    password = AesEncryptionHelper.Encrypt(ac.password),
                    id_role = 1,
                    count_failed_password = 0,
                    is_locked = false,
                    LocketEndTime = null
                };
                db.ACCOUNTs.Add(add_user);
            }
            await db.SaveChangesAsync();
            return Json(new { url = "/Account/Login", success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Đăng nhập chưa bảo mật
        //[HttpPost]
        //[Route("api/login")]
        //public async Task<JsonResult> Login(ACCOUNT aCCOUNT)
        //{
        //    int MaxFailedAttemptsBeforeLockout = 3;
        //    int MaxFailedAttemptsToPermanentLock = 5;
        //    int LockoutDurationInMinutes = 1;
        //    var account = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == aCCOUNT.username);

        //    if (account == null)
        //    {
        //        return Json(new { message = "Sai thông tin đăng nhập", success = false }, JsonRequestBehavior.AllowGet);
        //    }

        //    var decryptedPassword = AesEncryptionHelper.Decrypt(account.password);
        //    if (decryptedPassword == aCCOUNT.password)
        //    {
        //        return Json(new { message = "Đăng nhập thành công", success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { message = "Sai thông tin đăng nhập", success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion
        #region Đăng nhập đã bảo mật
        [HttpPost]
        [Route("api/login")]
        public async Task<JsonResult> Login(ACCOUNT aCCOUNT)
        {
            int MaxFailedAttemptsBeforeLockout = 3; 
            int MaxFailedAttemptsToPermanentLock = 5; 
            int LockoutDurationInSeconds = 15; 

            var account = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == aCCOUNT.username);

            if (account == null)
            {
                return Json(new { message = "Tài khoản không tồn tại", success = false }, JsonRequestBehavior.AllowGet);
            }

            if (account.is_locked == true)
            {
                return Json(new { message = "Tài khoản của bạn đã bị khóa vĩnh viễn vì nhập sai quá nhiều lần", success = false }, JsonRequestBehavior.AllowGet);
            }
            if (account.LocketEndTime.HasValue && account.LocketEndTime > DateTime.Now)
            {
                return Json(new
                {
                    message = $"Tài khoản bị tạm khóa. Vui lòng thử lại sau {(account.LocketEndTime.Value - DateTime.Now).Seconds} giây.",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
            var decryptedPassword = AesEncryptionHelper.Decrypt(account.password);

            if (decryptedPassword == aCCOUNT.password)
            {
                account.count_failed_password = 0;
                account.is_locked = false;
                account.LocketEndTime = null;

                await db.SaveChangesAsync();
                return Json(new { message = "Đăng nhập thành công", success = true }, JsonRequestBehavior.AllowGet);
            }
            account.count_failed_password = (account.count_failed_password ?? 0) + 1;
            if (account.count_failed_password >= MaxFailedAttemptsToPermanentLock)
            {
                account.is_locked = true;
                account.LocketEndTime = null; 
            }
            else if (account.count_failed_password >= MaxFailedAttemptsBeforeLockout)
            {
                account.LocketEndTime = DateTime.Now.AddSeconds(LockoutDurationInSeconds);
            }
            await db.SaveChangesAsync();
            return Json(new { message = "Sai thông tin đăng nhập", success = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}