using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.BlogsHome
{
    public class BlogsHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblBlog> list = _core.Blog.Get(orderBy: i => i.OrderByDescending(i => i.BlogId)).ToList();
            list.ShuffleList();
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/BlogsHomeComponent/" +
                "BlogsHomeComponent.cshtml"
                , list));
        }
    }
}
