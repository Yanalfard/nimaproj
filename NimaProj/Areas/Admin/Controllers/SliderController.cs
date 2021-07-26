using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Services.Services;
using ReflectionIT.Mvc.Paging;
using NimaProj.Utilities;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class SliderController : Controller
    {
        Core _core = new Core();
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<TblBannerAndSlide> AllSlider = PagingList.Create(_core.BannerAndSlide.Get().OrderByDescending(bas => bas.BannerAndSlideId), 30, page);
                return await Task.FromResult(View(AllSlider));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                return await Task.FromResult(ViewComponent("CreateSliderAdmin"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TblBannerAndSlide slider, IFormFile ImageUrl, int SliderTime)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    TblBannerAndSlide NewSlider = new TblBannerAndSlide();
                    NewSlider.Name = slider.Name;
                    NewSlider.ActiveTill = DateTime.Now.AddDays(SliderTime);
                    NewSlider.IsBanner = slider.IsBanner;
                    NewSlider.Link = slider.Link;
                    if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                    {
                        NewSlider.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                        string savePath = Path.Combine(
                                                Directory.GetCurrentDirectory(), "wwwroot/Images/Slider", NewSlider.ImageUrl
                                            );
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await ImageUrl.CopyToAsync(stream);
                        };
                    }

                    _core.BannerAndSlide.Add(NewSlider);
                    _core.Save();
                    return Redirect("/Admin/Slider");
                }
                return await Task.FromResult(View(slider));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("EditSliderAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(TblBannerAndSlide slider, IFormFile ImageUrl, int SliderTime)
        {
            try
            {


                TblBannerAndSlide FirstSlider = _core.BannerAndSlide.GetById(slider.BannerAndSlideId);
                FirstSlider.Name = slider.Name;
                FirstSlider.IsBanner = slider.IsBanner;
                FirstSlider.Link = slider.Link;
                FirstSlider.ActiveTill = DateTime.Now.AddDays(SliderTime);
                if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                {
                    try
                    {
                        var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Slider", FirstSlider.ImageUrl);
                        if (System.IO.File.Exists(deleteImagePath))
                        {
                            System.IO.File.Delete(deleteImagePath);
                        }
                    }
                    catch
                    {

                    }
                    FirstSlider.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                    string savePath = Path.Combine(
                                               Directory.GetCurrentDirectory(), "wwwroot/Images/Slider", FirstSlider.ImageUrl
                                           );
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    };
                }

                _core.BannerAndSlide.Update(FirstSlider);
                _core.Save();
                return await Task.FromResult(Redirect("/Admin/Slider"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public string RemoveSlider(int id)
        {
            TblBannerAndSlide slider = _core.BannerAndSlide.GetById(id);

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Slider", slider.ImageUrl);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _core.BannerAndSlide.DeleteById(id);
            _core.Save();
            return "true";
        }
        public ActionResult ActiveBaner(int id)
        {
            TblBannerAndSlide updateUser = _core.BannerAndSlide.GetById(id);
            updateUser.IsBanner = !updateUser.IsBanner;
            _core.BannerAndSlide.Update(updateUser);
            _core.Save();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _core.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
