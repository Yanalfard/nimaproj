﻿using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhasreMobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        Core db = new Core();

        // GET: api/Shop
        [HttpGet]
        public int Get()
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var sessions = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
            if (sessions != null)
            {
                list = sessions as List<ShopCartItem>;
            }
            int id = list.Sum(l => l.Count);
            return list.Sum(l => l.Count);
        }
        // GET: api/Shop/5
        [HttpGet("{id}")]
        public int Get(int id)
        {
            TblProduct selectedP = db.Product.GetById(id);

            if (!selectedP.IsOfflineOrder)
            {

                List<ShopCartItem> list = new List<ShopCartItem>();
                var sessions = HttpContext.Session.GetComplexData<List<ShopCartItem>>("ShopCart");
                if (sessions != null)
                {
                    list = sessions as List<ShopCartItem>;
                }
                if (list.Any(p => p.ProductID == id))
                {
                    int index = list.FindIndex(p => p.ProductID == id);
                    list[index].Count += 1;
                }
                else
                {
                    list.Add(new ShopCartItem()
                    {
                        ProductID = id,
                        Count = 1,
                        Price = selectedP.Price
                    });
                }
                HttpContext.Session.SetComplexData("ShopCart", list);
            }

            return Get();
        }


    }
}
