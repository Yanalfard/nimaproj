using DataLayer.Models;
using DataLayer.ViewModels.View;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using ReflectionIT.Mvc.Paging;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.Controllers
{
    public class BlogController : Controller
    {
        private Core _core = new Core();
        [Route("Blog/{id?}/{name?}")]
        public async Task<IActionResult> Index(int page = 1, int id = 0, string name = "")
        {
            try
            {
                List<TblBlog> list = new List<TblBlog>();
                if (id != 0)
                {
                    ViewBag.name = name;
                    list.AddRange(_core.Blog.Get(i => i.CatagoryId == id || i.Catagory.ParentId == id,
                   orderBy: i => i.OrderByDescending(i => i.BlogId)));
                }
                else
                {
                    ViewBag.name = "همه مقاله ها";
                    list.AddRange(_core.Blog.Get(orderBy: i => i.OrderByDescending(i => i.BlogId)));
                }
                //    return View(PagingList.Create(list, 2, page));
                return await Task.FromResult(View(list));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }


        }

        [Route("ViewBlog/{id}/{name}")]
        public async Task<IActionResult> BlogView(int id, string name = "")
        {
            try
            {
                return await Task.FromResult(View(_core.Blog.GetById(id)));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
    }
}
