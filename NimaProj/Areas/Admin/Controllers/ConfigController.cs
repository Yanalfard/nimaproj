﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.ViewModels;
using Services.Services;
using NimaProj.Utilities;

namespace NimaProj.Areas.Admin.Controllers
{

    [Area("Admin")]
    [PermissionChecker("admin")]
    public class ConfigController : Controller
    {
        Core _core = new Core();

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<TblConfig> configs = _core.Config.Get();
            ConfigVm config = new ConfigVm();
            config.Email = configs.Where(c => c.Key == "Email").Single().Value;
            config.Telegram = configs.Where(c => c.Key == "Telegram").Single().Value;
            config.Inista = configs.Where(c => c.Key == "Inista").Single().Value;
            config.TellHome = configs.Where(c => c.Key == "TellHome").Single().Value;
            config.TellMobile = configs.Where(c => c.Key == "TellMobile").Single().Value;
            config.Address = configs.Where(c => c.Key == "Address").Single().Value;
            config.Whatsapp = configs.Where(c => c.Key == "Whatsapp").Single().Value;
            config.SharyeteErsal = configs.Where(c => c.Key == "SharyeteErsal").Single().Value;
            config.OrderDetails = configs.Where(c => c.Key == "OrderDetails").Single().Value;
            config.DarBareyeMa = configs.Where(c => c.Key == "DarBareyeMa").Single().Value;
            return View(config);
        }

        [HttpPost]
        public IActionResult Index(ConfigVm configVm)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<TblConfig> configs = _core.Config.Get();
                TblConfig ConfigLinkEmail = configs.Where(c => c.Key == "Email").Single();
                TblConfig ConfigLinkTelegram = configs.Where(c => c.Key == "Telegram").Single();
                TblConfig ConfigLinkInista = configs.Where(c => c.Key == "Inista").Single();
                TblConfig ConfigLinkTellHome = configs.Where(c => c.Key == "TellHome").Single();
                TblConfig ConfigLinkTellMobile = configs.Where(c => c.Key == "TellMobile").Single();
                TblConfig ConfigLinkAddress = configs.Where(c => c.Key == "Address").Single();
                TblConfig ConfigLinkWhatsapp = configs.Where(c => c.Key == "Whatsapp").Single();
                TblConfig ConfigLinkSharyeteErsal = configs.Where(c => c.Key == "SharyeteErsal").Single();
                TblConfig ConfigLinkOrderDetails = configs.Where(c => c.Key == "OrderDetails").Single();
                TblConfig ConfigLinkDarBareyeMa = configs.Where(c => c.Key == "DarBareyeMa").Single();

                ConfigLinkEmail.Value = configVm.Email;
                ConfigLinkTelegram.Value = configVm.Telegram;
                ConfigLinkInista.Value = configVm.Inista;
                ConfigLinkTellHome.Value = configVm.TellHome;
                ConfigLinkTellMobile.Value = configVm.TellMobile;
                ConfigLinkAddress.Value = configVm.Address;
                ConfigLinkWhatsapp.Value = configVm.Whatsapp;
                ConfigLinkSharyeteErsal.Value = configVm.SharyeteErsal;
                ConfigLinkOrderDetails.Value = configVm.OrderDetails;
                ConfigLinkDarBareyeMa.Value = configVm.DarBareyeMa;
                _core.Config.Update(ConfigLinkEmail);
                _core.Config.Update(ConfigLinkTelegram);
                _core.Config.Update(ConfigLinkInista);
                _core.Config.Update(ConfigLinkTellHome);
                _core.Config.Update(ConfigLinkTellMobile);
                _core.Config.Update(ConfigLinkAddress);
                _core.Config.Update(ConfigLinkWhatsapp);
                _core.Config.Update(ConfigLinkSharyeteErsal);
                _core.Config.Update(ConfigLinkOrderDetails);
                _core.Config.Update(ConfigLinkDarBareyeMa);
                _core.Save();
            }
            return View(configVm);
        }

    }
}
