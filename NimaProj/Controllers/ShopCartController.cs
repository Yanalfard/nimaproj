using DataLayer.Models;
using DataLayer.Utilities;
using DataLayer.ViewModels;
using NimaProj.Utilities;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace GhasreMobile.Controllers
{
    public class ShopCartController : Controller
    {
        Core db = new Core();
        public async Task<IActionResult> ShowListProduct()
        {
            try
            {
                List<ShopCartItemVm> list = new List<ShopCartItemVm>();
                var sessions = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
                if (sessions != null && sessions.Count > 0)
                {
                    List<ShopCartItem> listShop = (List<ShopCartItem>)sessions;

                    foreach (var item in listShop)
                    {
                        if (db.Product.Get().Any(i => i.ProductId == item.ProductID && i.IsDeleted == false))
                        {

                            var product = db.Product.Get().Where(p => p.ProductId == item.ProductID).Select(p => new
                            {
                                p.MainImage,
                                p.Name,
                                p.Price,
                                p.Brand,
                            }).Single();
                            ShopCartItemVm shop = new ShopCartItemVm();
                            shop.Count = item.Count;
                            shop.ProductID = item.ProductID;
                            shop.Name = product.Name;
                            shop.ImageName = product.MainImage;
                            shop.Price = product.Price;
                            shop.Brand = product.Brand.Name;
                            shop.Sum = item.Count * product.Price;
                            list.Add(shop);
                        }
                        else
                        {
                            var index = listShop.FindIndex(p => p.ProductID == item.ProductID);
                            listShop[index].Count = 0;
                        }
                    }
                }
                return await Task.FromResult(PartialView(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<ShopCartItemVm> list = new List<ShopCartItemVm>();
                var sessions = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
                if (sessions != null && sessions.Count > 0)
                {
                    List<ShopCartItem> listShop = (List<ShopCartItem>)sessions;

                    foreach (var item in listShop)
                    {
                        if (db.Product.Get().Any(i => i.ProductId == item.ProductID && i.IsDeleted == false))
                        {

                            var product = db.Product.Get().Where(p => p.ProductId == item.ProductID).Select(p => new
                            {
                                p.MainImage,
                                p.Name,
                                p.Price,
                                p.Brand,
                            }).Single();
                            ShopCartItemVm shop = new ShopCartItemVm();
                            shop.Count = item.Count;
                            shop.ProductID = item.ProductID;
                            shop.Name = product.Name;
                            shop.ImageName = product.MainImage;
                            shop.Price = product.Price;
                            shop.Brand = product.Brand.Name;
                            shop.Sum = item.Count * product.Price;
                            list.Add(shop);
                        }
                        else
                        {
                            var index = listShop.FindIndex(p => p.ProductID == item.ProductID);
                            listShop[index].Count = 0;
                        }
                    }
                }
                return await Task.FromResult(View(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> Checkout()
        {
            try
            {
                List<ShopCartItemVm> list = new List<ShopCartItemVm>();
                var sessions = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
                if (sessions != null && sessions.Count > 0)
                {
                    List<ShopCartItem> listShop = (List<ShopCartItem>)sessions;

                    foreach (var item in listShop)
                    {
                        if (db.Product.Get().Any(i => i.ProductId == item.ProductID && i.IsDeleted == false))
                        {

                            var product = db.Product.Get().Where(p => p.ProductId == item.ProductID).Select(p => new
                            {
                                p.MainImage,
                                p.Name,
                                p.Price,
                                p.Brand,
                            }).Single();
                            ShopCartItemVm shop = new ShopCartItemVm();
                            shop.Count = item.Count;
                            shop.ProductID = item.ProductID;
                            shop.Name = product.Name;
                            shop.ImageName = product.MainImage;
                            shop.Price = product.Price;
                            shop.Brand = product.Brand.Name;
                            shop.Sum = item.Count * product.Price;
                            list.Add(shop);
                        }
                        else
                        {
                            var index = listShop.FindIndex(p => p.ProductID == item.ProductID);
                            listShop[index].Count = 0;
                        }
                    }
                }
                return await Task.FromResult(View(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> UpDownCount(int id, string command, string ReturnUrl = "/")
        {
            try
            {
                TblProduct selectedP = db.Product.GetById(id);
                var listShop = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
                var index = listShop.FindIndex(p => p.ProductID == id);

                switch (command)
                {
                    case "up":
                        {

                            listShop[index].Count += 1;
                            break;
                        }
                    case "down":
                        {
                            listShop[index].Count -= 1;
                            if (listShop[index].Count == 0)
                            {
                                listShop.RemoveAt(index);
                            }
                            break;
                        }
                    case "delete":
                        {

                            listShop.RemoveAt(index);

                            break;
                        }
                }
                HttpContext.Session.SetComplexData("ShopCart", listShop);
                if (listShop != null && listShop.Count == 0)
                {
                    return Redirect("/");
                }


                return await Task.FromResult(Redirect(ReturnUrl));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }

        [PermissionChecker("user,admin")]
        public async Task<IActionResult> Payment(string address, string postalCode)
        {
            try
            {
                List<ShopCartItem> sessions = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
                if (sessions != null)
                {

                    List<ShopCartItemVm> list = new List<ShopCartItemVm>();
                    TblOrder addOrder = new TblOrder();
                    addOrder.Address = address;
                    addOrder.DateSubmited = DateTime.Now;
                    addOrder.FinalPrice = 0;
                    addOrder.IsFinaly = false;
                    addOrder.Status = 0;
                    addOrder.PostalCode = postalCode;
                    addOrder.SendPrice = 0;
                    int userId = Convert.ToInt32(User.Claims.First().Value);
                    addOrder.ClientId = Main.SelectUser(userId).ClientId;
                    db.Order.Add(addOrder);
                    db.Save();
                    foreach (var item in sessions)
                    {
                        var product = db.Product.Get().Where(p => p.ProductId == item.ProductID).Select(p => new
                        {
                            p.Price,
                        }).Single();
                        TblOrderDetail addOrderDetail = new TblOrderDetail();
                        addOrderDetail.Count = item.Count;
                        addOrderDetail.FinalOrderId = addOrder.OrdeId;
                        addOrderDetail.ProductId = item.ProductID;
                        addOrderDetail.Price = (int)product.Price;
                        db.OrderDetail.Add(addOrderDetail);

                        ShopCartItemVm shop = new ShopCartItemVm();
                        shop.Price = product.Price;
                        shop.Sum = item.Count * product.Price;
                        list.Add(shop);
                    }
                    db.Save();
                    TblOrder updateOrder = db.Order.GetById(addOrder.OrdeId);
                    updateOrder.FinalPrice = (int)list.Sum(i => i.Sum);
                    db.Order.Update(updateOrder);
                    db.Save();

                    var payment = new Payment((int)list.Sum(i => i.Sum));
                    var res = payment.PaymentRequest($"پرداخت فاکتور شماره {updateOrder.OrdeId}",
                        "http://localhost:44340/ShopCart/OnlinePayment/" + updateOrder.OrdeId, "mehdi@sahandi.com", "09357035985");
                    if (res.Result.Status == 100)
                    {
                        return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return Redirect("/");
                
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = db.Order.GetById(id);
                var payment = new Payment((int)order.FinalPrice);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    order.IsFinaly = true;
                    ViewBag.IsFinaly = true;
                    db.Order.Update(order);
                    db.Save();
                    ViewBag.code = res.RefId;
                    HttpContext.Session.Clear();
                    return await Task.FromResult(View());
                }
            }
            ViewBag.IsFinaly = false;
            return await Task.FromResult(View());
        }
    }
}
