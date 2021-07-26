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
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<TblBrand> catagories = PagingList.Create(_core.Brand.Get(), 10, page);
                return await Task.FromResult(View(catagories));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? Id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("CreateBrandAdmin", new { Id = Id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblBrand brand, IFormFile ImageUrl)
        {
            try
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
                    return await Task.FromResult(Redirect("/Admin/Brand"));

                }
                return await Task.FromResult(PartialView("Create", brand));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("EditBrandAdmin", new { Id = Id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblBrand brand, IFormFile ImageUrl)
        {
            try
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
                return await Task.FromResult(View(brand));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowChilds(int Id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("ShowChildsBrandAdmin", new { Id = Id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
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
