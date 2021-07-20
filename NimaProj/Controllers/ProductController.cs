using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.Controllers
{
    public class ProductController : Controller
    {
        private Core _core = new Core();
        public IActionResult Index(int page = 1, int id = 0, string name = "")
        {
           
            List<TblProduct> list = new List<TblProduct>();
            if (id != 0)
            {
                ViewBag.name = name;
                list.AddRange(_core.Product.Get(i => i.CatagoryId == id && i.IsDeleted == false,
               orderBy: i => i.OrderByDescending(i => i.ProductId)));
            }
            else
            {
                ViewBag.name = "محصولات";
                list.AddRange(_core.Product.Get(i => i.IsDeleted == false,
                    orderBy: i => i.OrderByDescending(i => i.ProductId)));
            }
            //    return View(PagingList.Create(list, 2, page));
            return View(list);

        }
        [Route("ViewProduct/{id}/{name}")]
        public IActionResult ProductView(int id, string name = "")
        {
            return View(_core.Product.GetById(id));
        }
    }
}
