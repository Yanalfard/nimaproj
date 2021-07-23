using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.Admin.Brand
{
    public class EditBrandAdmin : ViewComponent
    {
        Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            return await Task.FromResult((IViewComponentResult)View("/Areas/Admin/Views/Brand/Components/Edit.cshtml", _core.Brand.GetById(Id)));
        }
    }
}
