﻿using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.Admin.Client
{
    public class ClientEditAdmin:ViewComponent
    {
        Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            ViewBag.Roles = _core.Role.Get();
            return await Task.FromResult((IViewComponentResult)View("/Areas/Admin/Views/Client/Components/Edit.cshtml", _core.Client.GetById(id)));
        }
    }
}
