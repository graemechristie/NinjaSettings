using System.Web.Mvc;

namespace NinjaSettings.Example.Autofac.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppSettings _appSettings;

        public HomeController(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public ActionResult Index()
        {
            
            return View(_appSettings);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}