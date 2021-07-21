using DataLayer.ViewModels.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.SendCommentHome
{
    public class SendCommentHomeComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            SendCommentVm commentVm = new SendCommentVm();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Shared/Components/SendCommentHomeComponent/SendCommentHomeComponent.cshtml", commentVm));
        }
    }
}
