using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using ProjectLab.WebUI.Common;
using ProjectLab.WebUI.Models;

namespace ProjectLab.WebUI.Controllers
{
    public class HomeController(IAlert alert,ILogger<HomeController> logger) : Controller
    {
        //IToastNotification _toastNotification = toastNotification;
        //public HomeController(IToastNotification toastNotification)
        //{
        //    _toastNotification = toastNotification;
        //}

        public IActionResult Index()
        {
            alert.Info("Hoş geldiniz!");
            logger.Log(LogLevel.Error,"Burda bir hata meydana geldi");

            Person person = new Person
            {
                Id = 1,
                FirstName = "Ali",
                LastName = "Veli",
            };
            return View(person);
        }
        [HttpPost]
        public string PersonDelete(int id)
        {
            return $"Silme işlemi Başarıya gerçekleşti Silinen Personel Id= {id}";
        }
    }
}
