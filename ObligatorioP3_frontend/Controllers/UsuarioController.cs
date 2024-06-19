using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObligatorioP3_frontend.Models;
using System.Net.Http.Headers;

namespace ObligatorioP3_frontend.Controllers
{
    public class UsuarioController : Controller
    {
        private HttpClient _cliente;
        private string _url;
        public UsuarioController()
        {
            _cliente = new HttpClient();
            _url = "http://localhost:5029/api/";
        }
        // GET: UsuarioController
        public ActionResult Index()
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return View();
        }

    }
}
