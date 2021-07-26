using DataLayer.Models;
using DataLayer.Security;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace NimaProj.Areas.User.Controllers
{
    [Area("User")]
    [PermissionChecker("user,admin")]
    public class AccountController : Controller
    {
        private Core _core = new Core();
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.FromResult(View());
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        public async Task<IActionResult> Info()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.First().Value);
                InfoVm info = new InfoVm()
                {
                    Name = Main.SelectUser(userId).Name,
                    ImageUrl = Main.SelectUser(userId).MainImage
                };
                return await Task.FromResult(View(info));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Info(InfoVm info, IFormFile MainImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = Convert.ToInt32(User.Claims.First().Value);
                    TblClient updateUser = _core.Client.GetById(Main.SelectUser(userId).ClientId);

                    if (MainImage != null && MainImage.IsImages() && MainImage.Length < 3000000)
                    {
                        try
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/", info.ImageUrl);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }
                        catch
                        {

                        }
                        info.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                        string saveDirectory = Path.Combine(
                                                Directory.GetCurrentDirectory(), "wwwroot/Images/User/");
                        string savePath = Path.Combine(saveDirectory, info.ImageUrl);

                        if (!Directory.Exists(saveDirectory))
                        {
                            Directory.CreateDirectory(saveDirectory);
                        }

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await MainImage.CopyToAsync(stream);
                        }
                        /// #region resize Image
                        ImageConvertor imgResizer = new ImageConvertor();
                        string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/thumb", info.ImageUrl);
                        imgResizer.Image_resize(savePath, thumbPath, 300);
                        /// #endregion
                    }
                    updateUser.Name = info.Name;
                    updateUser.MainImage = info.ImageUrl;
                    _core.Client.Update(updateUser);
                    _core.Save();
                    return await Task.FromResult(Redirect("/User/Account/Info?Resetinfo=true"));

                }
                return await Task.FromResult(View(info));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        public async Task<IActionResult> ChangePassword()
        {
            try
            {
                return await Task.FromResult(View());
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetChangePasswordVm change)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = Convert.ToInt32(User.Claims.First().Value);
                    TblClient updateUser = _core.Client.GetById(Main.SelectUser(userId).ClientId);
                    string pass = PasswordHelper.EncodePasswordMd5(change.OldPassword);
                    if (pass != updateUser.Password)
                    {
                        ModelState.AddModelError("OldPassword", "رمز قدیمی اشتباه است");
                    }
                    else
                    {
                        updateUser.Password = PasswordHelper.EncodePasswordMd5(change.Password);
                        _core.Client.Update(updateUser);
                        _core.Save();
                        return await Task.FromResult(Redirect("/User/Account/ChangePassword?ResetPass=true"));
                    }
                }
                return await Task.FromResult(View(change));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
    }
}
