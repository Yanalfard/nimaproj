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
    public class CatagoryBlogController : Controller
    {
        Core _core = new Core();
        public IActionResult Index(int page = 1)
        {
            IEnumerable<TblCatagory> catagories = PagingList.Create(_core.Catagory.Get(c => c.ParentId == null && c.IsBlog == true), 10, page);
            return View(catagories);
        }

        [HttpGet]
        public IActionResult Create(int? Id)
        {
            return ViewComponent("CreateCatagoryBlogAdmin", new { Id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblCatagory catagory, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                catagory.IsBlog = true;
                if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                {
                    catagory.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                    string savePath = Path.Combine(
                                            Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog", catagory.ImageUrl
                                        );
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    };
                    /// #region resize Image
                    ImageConvertor imgResizer = new ImageConvertor();
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog/thumb", catagory.ImageUrl);
                    imgResizer.Image_resize(savePath, thumbPath, 300);
                    /// #endregion
                }
                if (catagory.ParentId == null)
                {

                    _core.Catagory.Add(catagory);
                    _core.Save();
                    return Redirect("/Admin/CatagoryBlog");
                }
                else
                {
                    catagory.IsOnFirstPage = true;
                    _core.Catagory.Add(catagory);
                    _core.Save();
                    return Redirect("/Admin/CatagoryBlog");
                }
            }
            return PartialView("Create", catagory);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            return ViewComponent("EditCatagoryBlogAdmin", new { Id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblCatagory catagory, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                catagory.IsBlog = true;
                if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                {
                    try
                    {
                        var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog", catagory.ImageUrl);
                        if (System.IO.File.Exists(deleteImagePath))
                        {
                            System.IO.File.Delete(deleteImagePath);
                        }
                        var deleteImagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog/thumb", catagory.ImageUrl);
                        if (System.IO.File.Exists(deleteImagePath2))
                        {
                            System.IO.File.Delete(deleteImagePath2);
                        }
                    }
                    catch
                    {

                    }
                    catagory.ImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                    string savePath = Path.Combine(
                                               Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog", catagory.ImageUrl
                                           );
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    };
                    /// #region resize Image
                    ImageConvertor imgResizer = new ImageConvertor();
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog/thumb", catagory.ImageUrl);
                    imgResizer.Image_resize(savePath, thumbPath, 300);
                    /// #endregion
                }
                _core.Catagory.Update(catagory);
                _core.Save();
                return Redirect("/Admin/CatagoryBlog");
            }
            return View(catagory);
        }

        [HttpGet]
        public IActionResult ShowChilds(int Id)
        {
            return ViewComponent("ShowChildsCatagoryBlogAdmin", new { Id = Id });
        }

        public string Delete(int id)
        {
            if (_core.Catagory.GetById(id).InverseParent.Count() > 0 || _core.Catagory.GetById(id).TblProducts.Count() > 0)
            {
                return "false";
            }
            else
            {
                TblCatagory selectedCategory = _core.Catagory.GetById(id);
                if (selectedCategory.ImageUrl != null)
                {
                    var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog", selectedCategory.ImageUrl);
                    if (System.IO.File.Exists(deleteImagePath))
                    {
                        System.IO.File.Delete(deleteImagePath);
                    }
                    var deleteImagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/CatagoryBlog", selectedCategory.ImageUrl);
                    if (System.IO.File.Exists(deleteImagePath2))
                    {
                        System.IO.File.Delete(deleteImagePath);
                    }
                }
                _core.Catagory.DeleteById(id);
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
