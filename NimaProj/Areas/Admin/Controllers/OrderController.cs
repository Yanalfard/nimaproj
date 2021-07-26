using Microsoft.AspNetCore.Mvc;
using DataLayer.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReflectionIT.Mvc.Paging;
using System.Globalization;
using DataLayer.Utilities;
using DataLayer.ViewModels;
using NimaProj.Utilities;

namespace GhasreMobile.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class OrderController : Controller
    {
        Core _core = new Core();

        public async Task<IActionResult> Index(OrdersInAdminVm ordersInAdmin)
        {
            ViewBag.OrderId = ordersInAdmin.OrderId;
            ViewBag.TellNo = ordersInAdmin.TellNo;
            ViewBag.StartDate = ordersInAdmin.StartDate;
            ViewBag.EndDate = ordersInAdmin.EndDate;
            ViewBag.StatusSelected = ordersInAdmin.StatusSelected;
            List<TblOrder> orders = _core.Order.Get(i => i.IsFinaly).ToList();
            int count = orders.Count;

            if (ordersInAdmin.OrderId != 0)
            {
                orders = orders.Where(i => i.OrdeId == ordersInAdmin.OrderId).ToList();

            }
            if (ordersInAdmin.StatusSelected != -2)
            {
                orders = orders.Where(i => i.Status == ordersInAdmin.StatusSelected).ToList();
            }
            if (ordersInAdmin.TellNo != null)
            {
                orders = orders.Where(i => i.Client.TellNo.Contains(ordersInAdmin.TellNo)).ToList();

            }
            if (ordersInAdmin.StartDate != null)
            {
                PersianCalendar pc = new PersianCalendar();
                string[] Start = ordersInAdmin.StartDate.Split('/');
                DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                orders = orders.Where(i => i.DateSubmited.Date >= startTime.Date).ToList();
            }
            if (ordersInAdmin.EndDate != null)
            {
                PersianCalendar pc = new PersianCalendar();
                string[] Start = ordersInAdmin.EndDate.Split('/');
                DateTime endTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                orders = orders.Where(i => i.DateSubmited.Date <= endTime.Date).ToList();

            }

            count = orders.Count();

            ViewBag.pageid = ordersInAdmin.PageId;

            ViewBag.PageCount = count / ordersInAdmin.InPageCount;

            ViewBag.InPageCount = ordersInAdmin.InPageCount;
            var skip = (ordersInAdmin.PageId - 1) * ordersInAdmin.InPageCount;
            return View(PagingList.Create(orders.OrderByDescending(b => b.OrdeId), 20, ordersInAdmin.PageId));

        }
        public async Task<IActionResult> AllOrder(OrdersInAdminVm ordersInAdmin)
        {
            try
            {
                ViewBag.OrderId = ordersInAdmin.OrderId;
                ViewBag.TellNo = ordersInAdmin.TellNo;
                ViewBag.StartDate = ordersInAdmin.StartDate;
                ViewBag.EndDate = ordersInAdmin.EndDate;
                ViewBag.StatusSelected = ordersInAdmin.StatusSelected;
                List<TblOrder> orders = _core.Order.Get().ToList();
                int count = orders.Count;

                if (ordersInAdmin.OrderId != 0)
                {
                    orders = orders.Where(i => i.OrdeId == ordersInAdmin.OrderId).ToList();

                }
                if (ordersInAdmin.StatusSelected != -2)
                {
                    orders = orders.Where(i => i.Status == ordersInAdmin.StatusSelected).ToList();
                }
                if (ordersInAdmin.TellNo != null)
                {
                    orders = orders.Where(i => i.Client.TellNo.Contains(ordersInAdmin.TellNo)).ToList();

                }
                if (ordersInAdmin.StartDate != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    string[] Start = ordersInAdmin.StartDate.Split('/');
                    DateTime startTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                    orders = orders.Where(i => i.DateSubmited.Date >= startTime.Date).ToList();
                }
                if (ordersInAdmin.EndDate != null)
                {
                    PersianCalendar pc = new PersianCalendar();
                    string[] Start = ordersInAdmin.EndDate.Split('/');
                    DateTime endTime = pc.ToDateTime(Convert.ToInt32(Start[0]), Convert.ToInt32(Start[1]), Convert.ToInt32(Start[2]), 0, 0, 0, 0).Date;
                    orders = orders.Where(i => i.DateSubmited.Date <= endTime.Date).ToList();

                }

                count = orders.Count();

                ViewBag.pageid = ordersInAdmin.PageId;

                ViewBag.PageCount = count / ordersInAdmin.InPageCount;

                ViewBag.InPageCount = ordersInAdmin.InPageCount;
                var skip = (ordersInAdmin.PageId - 1) * ordersInAdmin.InPageCount;

                return await Task.FromResult(View(PagingList.Create(orders.OrderByDescending(b => b.OrdeId), 20, ordersInAdmin.PageId)));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        public async Task<IActionResult> Info(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("OrderInfoAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> CodeMarsole(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("CodeMarsoleAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        public async Task<string> SendOrderAsync(int id)
        {
            TblOrder order = _core.Order.GetById(id);
            order.Status = 1;
            //await Sms.SendSms(order.Client.TellNo, order.OrdeId.ToString(), "GhasrMobileSendOrder");
            _core.Order.Update(order);
            _core.Save();
            return await Task.FromResult("true");

        }

        public async Task<string> DeleteFractional(int id)
        {
            TblOrder order = _core.Order.GetById(id);
            order.Status = -1;
            _core.Order.Update(order);
            _core.Save();
            return await Task.FromResult("true");
        }
        public void ChangeSendOrderStatus(int id)
        {
            TblOrder Order = _core.Order.GetById(id);
            Order.Status = 0;
            _core.Order.Update(Order);
            _core.Save();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _core.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<string> RedreshFractionalAsync(int id)
        {
            TblOrder order = _core.Order.GetById(id);
            order.Status = 0;
            _core.Order.Update(order);
            _core.Save();
            return await Task.FromResult("true");
        }
 
    }
}
