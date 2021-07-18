using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.Admin.ContactUs
{
    public class ContactUsInfoAdmin:ViewComponent
    {
        Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            Core core = new Core();
            return await Task.FromResult((IViewComponentResult)View("/Areas/Admin/Views/ContactUs/Components/ContactUsInfo.cshtml",core.ContactU.GetById(id)));
        }
    }
}
