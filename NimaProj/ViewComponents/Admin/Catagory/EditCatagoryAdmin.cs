﻿using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.Admin.Catagory
{
    public class EditCatagoryAdmin : ViewComponent
    {
        Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            return await Task.FromResult((IViewComponentResult)View("/Areas/Admin/Views/Catagory/Components/Edit.cshtml", _core.Catagory.GetById(Id)));
        }
    }
}
