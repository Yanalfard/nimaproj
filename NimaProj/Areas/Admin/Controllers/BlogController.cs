using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using ReflectionIT.Mvc.Paging;
using Services.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using NimaProj.Utilities;
using NimaProj;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class BlogController : Controller
    {
        Core _core = new Core();
        public async Task<IActionResult> Index(int page = 1, string Search = null)
        {
            try
            {
                ViewBag.namBlog = Search;
                if (!string.IsNullOrEmpty(Search))
                {
                    IEnumerable<TblBlog> blogs = PagingList.Create(_core.Blog.Get(b => b.Title.Contains(Search)), 30, page);
                    return await Task.FromResult(View(blogs));
                }
                else
                {
                    IEnumerable<TblBlog> blogs = PagingList.Create(_core.Blog.Get().OrderByDescending(b => b.BlogId), 30, page);
                    return await Task.FromResult(View(blogs));
                }
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }


        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == true);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TblBlog blog, IFormFile MainImage)
        {
            if (ModelState.IsValid)
            {
                if (MainImage != null && MainImage.IsImages() && MainImage.Length < 3000000)
                {
                    blog.MainImage = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                    string saveDirectory = Path.Combine(
                                                    Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs");
                    string savePathAlbum = Path.Combine(
                                        Directory.GetCurrentDirectory(), saveDirectory, blog.MainImage
                                    );
                    if (!Directory.Exists(saveDirectory))
                    {
                        Directory.CreateDirectory(saveDirectory);
                    }
                    using (var stream = new FileStream(savePathAlbum, FileMode.Create))
                    {
                        await MainImage.CopyToAsync(stream);
                    }
                    /// #region resize Image
                    ImageConvertor imgResizer = new ImageConvertor();
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs/thumb", blog.MainImage);
                    imgResizer.Image_resize(savePathAlbum, thumbPath, 300);
                    /// #endregion
                }

                _core.Blog.Add(blog);
                _core.Save();
                return await Task.FromResult(Redirect("/Admin/Blog"));
            }
            ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == true);
            return View(blog);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == true);
            return View(_core.Blog.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(TblBlog blog, IFormFile MainImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TblBlog Editblog = _core.Blog.GetById(blog.BlogId);
                    if (MainImage != null && MainImage.IsImages() && MainImage.Length < 3000000)
                    {
                        try
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", Editblog.MainImage);

                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }


                            var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs/thumb", Editblog.MainImage);

                            if (System.IO.File.Exists(imagePath2))
                            {
                                System.IO.File.Delete(imagePath2);
                            }

                        }
                        catch
                        {

                        }
                        blog.MainImage = Guid.NewGuid().ToString() + Path.GetExtension(MainImage.FileName);
                        string savePathAlbum = Path.Combine(
                                            Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", blog.MainImage
                                        );

                        using (var stream = new FileStream(savePathAlbum, FileMode.Create))
                        {
                            await MainImage.CopyToAsync(stream);
                        }
                        /// #region resize Image
                        ImageConvertor imgResizer = new ImageConvertor();
                        string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs/thumb", blog.MainImage);
                        imgResizer.Image_resize(savePathAlbum, thumbPath, 300);
                        /// #endregion
                    }

                    Editblog.CatagoryId = blog.CatagoryId;
                    Editblog.Title = blog.Title;
                    Editblog.Description = blog.Description;
                    Editblog.BodyHtml = blog.BodyHtml;
                    Editblog.MainImage = blog.MainImage;
                    _core.Blog.Update(Editblog);
                    _core.Save();
                    return await Task.FromResult(Redirect("/Admin/Blog"));
                }
                ViewBag.Parentcatagories = _core.Catagory.Get(c => c.ParentId == null && c.IsBlog == true);

                return await Task.FromResult(View(blog));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }


        }

        public void Delete(int id)
        {

            TblBlog blog = _core.Blog.GetById(id);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs", blog.MainImage);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            var imagePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Blogs/thumb", blog.MainImage);

            if (System.IO.File.Exists(imagePath2))
            {
                System.IO.File.Delete(imagePath2);
            }
            _core.Blog.Delete(blog);
            _core.Save();
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
