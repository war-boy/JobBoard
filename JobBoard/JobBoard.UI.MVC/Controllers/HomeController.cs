using System.Web.Mvc;

namespace JobBoard.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
