using DataLayer.Models;
using DataLayer.Security;
using DataLayer.Utilities;
using DataLayer.ViewModels;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NimaProj.Controllers
{
    public class AccountController : Controller
    {
        private Core db = new Core();
        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(ICaptchaValidator captchaValidator)
        {
            _captchaValidator = captchaValidator;
        }

        // Login
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.FindFirstValue(ClaimTypes.Role).ToString() == "user")
                    {
                        return Redirect("/User");
                    }
                    else if (User.FindFirstValue(ClaimTypes.Role).ToString() == "admin")
                    {
                        return Redirect("/Admin");
                    }
                }
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVm login, string ReturnUrl = "/")
        {
            try
            {
                if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
                {
                    ModelState.AddModelError("TellNo", "لطفا دوباره امتحان کنید");
                    return View(login);
                }
                if (ModelState.IsValid)
                {
                    string password = PasswordHelper.EncodePasswordMd5(login.Password);
                    if (db.Client.Get().Any(i => i.TellNo == login.TellNo && i.Password == password))
                    {
                        TblClient user = db.Client.Get().FirstOrDefault(i => i.TellNo == login.TellNo);
                        if (user.IsActive)
                        {
                            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.ClientId.ToString()),
                        new Claim(ClaimTypes.Name,user.TellNo),
                        new Claim(ClaimTypes.Role,db.Role.GetById(user.RoleId).Name.Trim()),
                    };
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            var properties = new AuthenticationProperties
                            {
                                IsPersistent = login.RememberMe
                            };
                            await HttpContext.SignInAsync(principal, properties);
                            ViewBag.IsSuccess = true;
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("TellNo", "حساب کاربری شما فعال نیست");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("TellNo", "شماره تلفن  یا رمز اشتباه است");
                    }
                }
                return await Task.FromResult(View(login));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        // Register
        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.FindFirstValue(ClaimTypes.Role).ToString() != "user")
                    {
                        return Redirect("/User");
                    }
                    else if (User.FindFirstValue(ClaimTypes.Role).ToString() != "admin")
                    {
                        return Redirect("/Admin");
                    }
                }
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterVm register)
        {
            try
            {
                if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
                {
                    ModelState.AddModelError("TellNo", "لطفا دوباره امتحان کنید");
                    return View(register);
                }
                if (ModelState.IsValid)
                {
                    if (!db.Client.Get().Any(i => i.TellNo == register.TellNo))
                    {
                        //var CodeCreator = Guid.NewGuid().ToString();
                        string Code = RandomNomber.RandomNo().ToString();
                        TblClient addUser = new TblClient();
                        addUser.DateCreated = DateTime.Now;
                        addUser.Auth = Code;
                        addUser.RoleId = 2;
                        addUser.Balance = 0;
                        addUser.IsActive = false;
                        addUser.Password = PasswordHelper.EncodePasswordMd5(register.Password);
                        addUser.TellNo = register.TellNo;
                        addUser.Name = register.Name;
                        db.Client.Add(addUser);
                        db.Save();
                        string message = addUser.Auth;
                        await Sms.SendSms(addUser.TellNo, message, "AsamedcoRegister");
                        return await Task.FromResult(Redirect("/Verify/" + addUser.TellNo));
                    }
                    else
                    {
                        ModelState.AddModelError("TellNo", "شماره تلفن تکراریست");
                    }
                }
                return await Task.FromResult(View(register));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("Verify/{tellNo}")]
        public async Task<IActionResult> Verify(string tellNo)
        {
            try
            {
                return await Task.FromResult(View(new ActiveVm()
                {
                    Tell = tellNo
                }));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [Route("Verify/{tellNo}")]
        [HttpPost]
        public async Task<IActionResult> VerifyAsync(ActiveVm active)
        {
            try
            {

                if (!await _captchaValidator.IsCaptchaPassedAsync(active.Captcha))
                {
                    ModelState.AddModelError("Auth", "ورود غیر مجاز");
                    return View(active);
                }
                if (ModelState.IsValid)
                {
                    if (db.Client.Get().Any(i => i.TellNo == active.Tell))
                    {
                        TblClient selectedUser = db.Client.Get().FirstOrDefault(i => i.TellNo == active.Tell);
                        if (selectedUser.Auth == active.Auth)
                        {
                            selectedUser.IsActive = true;
                            //var CodeCreator = Guid.NewGuid().ToString();
                            string Code = RandomNomber.RandomNo().ToString();
                            selectedUser.Auth = Code;
                            db.Save();
                            return await Task.FromResult(Redirect("/Login?Active=true"));
                        }
                        else
                        {
                            ModelState.AddModelError("Auth", "کد فعال سازی اشتباه است");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Auth", "شماره تلفن یافت نشد");
                    }
                }
                return await Task.FromResult(View(active));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            try
            {
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordVm forget)
        {
            try
            {
                if (!await _captchaValidator.IsCaptchaPassedAsync(forget.Captcha))
                {
                    ModelState.AddModelError("TellNo", "لطفا دوباره امتحان کنید");
                    return View(forget);
                }
                if (ModelState.IsValid)
                {
                    if (db.Client.Get().Any(i => i.TellNo == forget.TellNo))
                    {
                        TblClient forgotPassword = db.Client.Get().FirstOrDefault(i => i.TellNo == forget.TellNo);
                        string message = forgotPassword.Auth;
                        await Sms.SendSms(forgotPassword.TellNo, message, "AsamedcoForget");
                        return await Task.FromResult(Redirect("/ChangePassword/" + forgotPassword.TellNo));
                    }
                    else
                    {
                        ModelState.AddModelError("TellNo", "شماره تلفن یافت نشد");
                    }
                }
                return await Task.FromResult(View(forget));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [Route("ChangePassword/{tell}")]
        public async Task<IActionResult> ChangePassword(string tell)
        {
            try
            {
                return await Task.FromResult(View(new ChangePasswordVm()
                {
                    Tell = tell,
                }));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("ChangePassword/{tell}")]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordVm change)
        {
            try
            {

                if (!await _captchaValidator.IsCaptchaPassedAsync(change.Captcha))
                {
                    ModelState.AddModelError("Auth", "ورود غیر مجاز");
                    return View(change);
                }
                if (ModelState.IsValid)
                {
                    if (db.Client.Get().Any(i => i.TellNo == change.Tell && i.Auth == change.Auth))
                    {
                        TblClient selectedUser = db.Client.Get().FirstOrDefault(i => i.TellNo == change.Tell);
                        selectedUser.Password = PasswordHelper.EncodePasswordMd5(change.Password);
                        string Code = RandomNomber.RandomNo().ToString();
                        selectedUser.Auth = Code;
                        db.Client.Update(selectedUser);
                        db.Save();
                        return await Task.FromResult(Redirect("/Login?ChangePassword=true"));
                    }
                    else
                    {
                        ModelState.AddModelError("Auth", "شماره تلفن یا کد فعال سازی اشتباه است");
                    }
                }
                return await Task.FromResult(View(change));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
       
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return await Task.FromResult(Redirect("/"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        #region CheckTell
        public JsonResult VerifyTell(string tell)
        {

            if (db.Client.Any(i => i.TellNo == tell))
            {
                return Json($"شماره تلفن {tell} تکراری است");
            }
            return Json(true);
        }
        #endregion
    }
}
