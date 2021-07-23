using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;

namespace NimaProj.ViewComponents.Admin.Brand
{
    public class CreateBrandAdmin : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Areas/Admin/Views/Brand/Components/Create.cshtml"));
        }
    }
}
