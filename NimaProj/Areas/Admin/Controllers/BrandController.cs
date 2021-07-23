using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Services.Services;
using ReflectionIT.Mvc.Paging;
using NimaProj.Utilities;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class BrandController : Controller
    {
        Core _core = new Core();
        public IActionResult Index(int page = 1)
        {
            IEnumerable<TblBrand> catagories = PagingList.Create(_core.Brand.Get(), 10, page);
            return View(catagories);
        }

        [HttpGet]
        public IActionResult Create(int? Id)
        {
            return ViewComponent("CreateBrandAdmin", new { Id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblBrand brand, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                {
                    brand.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                    string savePath = Path.Combine(
                                            Directory.GetCurrentDirectory(), "wwwroot/Images/Brand", brand.ImageUrl
                                        );
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    };
                    /// #region resize Image
                    ImageConvertor imgResizer = new ImageConvertor();
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand/thumb", brand.ImageUrl);
                    imgResizer.Image_resize(savePath, thumbPath, 300);
                    /// #endregion
                }

                _core.Brand.Add(brand);
                _core.Save();
                return Redirect("/Admin/Brand");
            }
            return PartialView("Create", brand);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            return ViewComponent("EditBrandAdmin", new { Id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblBrand brand, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                {
                    try
                    {
                        var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand", brand.ImageUrl);
                        if (System.IO.File.Exists(deleteImagePath))
                        {
                            System.IO.File.Delete(deleteImagePath);
                        }
                        var deleteImagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand/thumb", brand.ImageUrl);
                        if (System.IO.File.Exists(deleteImagePath2))
                        {
                            System.IO.File.Delete(deleteImagePath2);
                        }
                    }
                    catch
                    {

                    }
                    brand.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                    string savePath = Path.Combine(
                                               Directory.GetCurrentDirectory(), "wwwroot/Images/Brand", brand.ImageUrl
                                           );
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    };
                    /// #region resize Image
                    ImageConvertor imgResizer = new ImageConvertor();
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand/thumb", brand.ImageUrl);
                    imgResizer.Image_resize(savePath, thumbPath, 300);
                    /// #endregion
                }
                _core.Brand.Update(brand);
                _core.Save();
                return Redirect("/Admin/Brand");
            }
            return View(brand);
        }

        [HttpGet]
        public IActionResult ShowChilds(int Id)
        {
            return ViewComponent("ShowChildsBrandAdmin", new { Id = Id });
        }

        public string Delete(int id)
        {
            if (_core.Brand.GetById(id).TblProducts.Count() > 0)
            {
                return "false";
            }
            else
            {
                TblBrand selectedCategory = _core.Brand.GetById(id);
                if (selectedCategory.ImageUrl != null)
                {
                    var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand", selectedCategory.ImageUrl);
                    if (System.IO.File.Exists(deleteImagePath))
                    {
                        System.IO.File.Delete(deleteImagePath);
                    }
                    var deleteImagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brand", selectedCategory.ImageUrl);
                    if (System.IO.File.Exists(deleteImagePath2))
                    {
                        System.IO.File.Delete(deleteImagePath);
                    }
                }
                _core.Brand.DeleteById(id);
                _core.Save();
                return "true";
            }
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
