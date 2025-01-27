using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using ProjectLab.WebUI.Models;

namespace ProjectLab.WebUI.Controllers
{
    public class HomeController(IToastNotification toastNotification,ILogger<HomeController> logger) : Controller
    {
        //IToastNotification _toastNotification = toastNotification;
        //public HomeController(IToastNotification toastNotification)
        //{
        //    _toastNotification = toastNotification;
        //}

        public IActionResult Index()
        {
            toastNotification.AddInfoToastMessage("Hoş geldiniz!",new ToastrOptions
            {
                Title = "Bilgi",

            });
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
