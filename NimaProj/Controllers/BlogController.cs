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
        TblClient SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblClient selectUser = _core.Client.GetById(userId);
            return selectUser;
        }
        public IActionResult Index(int page = 1, int id = 0, string name = "")
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
                ViewBag.name = "مقاله ها";
                list.AddRange(_core.Blog.Get(orderBy: i => i.OrderByDescending(i => i.BlogId)));
            }
            //    return View(PagingList.Create(list, 2, page));
            return View(list);

        }

        [Route("ViewBlog/{id}/{name}")]
        public IActionResult BlogView(int id, string name = "")
        {
            return View(_core.Blog.GetById(id));
        }
    }
}
