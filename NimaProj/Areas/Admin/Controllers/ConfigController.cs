using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.ViewModels;
using Services.Services;
using NimaProj.Utilities;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace NimaProj.Areas.Admin.Controllers
{

    [Area("Admin")]
    [PermissionChecker("admin")]
    public class ConfigController : Controller
    {
        Core _core = new Core();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
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
                config.BackImgHomeUnder = configs.Where(c => c.Key == "BackImgHomeUnder").Single().Value;
                config.BackImgHomeOn = configs.Where(c => c.Key == "BackImgHomeOn").Single().Value;
                config.BackTextHomeOn = configs.Where(c => c.Key == "BackTextHomeOn").Single().Value;
                config.IsInista = Convert.ToBoolean(configs.Where(c => c.Key == "IsInista").Single().Value);
                config.IsTelegram = Convert.ToBoolean(configs.Where(c => c.Key == "IsTelegram").Single().Value);
                config.IsWhatsapp = Convert.ToBoolean(configs.Where(c => c.Key == "IsWhatsapp").Single().Value);
                return await Task.FromResult(View(config));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfigVm configVm, IFormFile ImageUrl, IFormFile ImageUrl2)
        {
            try
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
                    TblConfig ConfigLinkIsInista = configs.Where(c => c.Key == "IsInista").Single();
                    TblConfig ConfigLinkIsTelegram = configs.Where(c => c.Key == "IsTelegram").Single();
                    TblConfig ConfigLinkIsWhatsapp = configs.Where(c => c.Key == "IsWhatsapp").Single();
                    TblConfig ConfigLinkBackImgHomeUnder = configs.Where(c => c.Key == "BackImgHomeUnder").Single();
                    TblConfig ConfigLinkBackImgHomeOn = configs.Where(c => c.Key == "BackImgHomeOn").Single();
                    TblConfig ConfigLinkBackTextHomeOn = configs.Where(c => c.Key == "BackTextHomeOn").Single();

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
                    ConfigLinkIsInista.Value = configVm.IsInista.ToString();
                    ConfigLinkIsTelegram.Value = configVm.IsTelegram.ToString();
                    ConfigLinkIsWhatsapp.Value = configVm.IsWhatsapp.ToString();
                    ConfigLinkBackTextHomeOn.Value = configVm.BackTextHomeOn;
                    if (ImageUrl != null && ImageUrl.IsImages() && ImageUrl.Length < 3000000)
                    {
                        try
                        {
                            var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/BackImageInHome", ConfigLinkBackImgHomeUnder.Value);
                            if (System.IO.File.Exists(deleteImagePath))
                            {
                                System.IO.File.Delete(deleteImagePath);
                            }
                        }
                        catch
                        {

                        }
                        ConfigLinkBackImgHomeUnder.Value = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);
                        string savePath = Path.Combine(
                                                Directory.GetCurrentDirectory(), "wwwroot/Images/BackImageInHome", ConfigLinkBackImgHomeUnder.Value
                                            );
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await ImageUrl.CopyToAsync(stream);
                        };
                    }
                    if (ImageUrl2 != null && ImageUrl2.IsImages() && ImageUrl2.Length < 3000000)
                    {
                        try
                        {
                            var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/BackImageInHome", ConfigLinkBackImgHomeOn.Value);
                            if (System.IO.File.Exists(deleteImagePath))
                            {
                                System.IO.File.Delete(deleteImagePath);
                            }
                        }
                        catch
                        {

                        }
                        ConfigLinkBackImgHomeOn.Value = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl2.FileName);
                        string savePath = Path.Combine(
                                                Directory.GetCurrentDirectory(), "wwwroot/Images/BackImageInHome", ConfigLinkBackImgHomeOn.Value
                                            );
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await ImageUrl2.CopyToAsync(stream);
                        };
                    }
                    _core.Config.Update(ConfigLinkBackTextHomeOn);
                    _core.Config.Update(ConfigLinkBackImgHomeOn);
                    _core.Config.Update(ConfigLinkBackImgHomeUnder);
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
                    _core.Config.Update(ConfigLinkIsInista);
                    _core.Config.Update(ConfigLinkIsTelegram);
                    _core.Config.Update(ConfigLinkIsWhatsapp);
                    _core.Save();
                    return await Task.FromResult(Redirect("/Admin/Config"));

                }
                return await Task.FromResult(View(configVm));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
    }
}
