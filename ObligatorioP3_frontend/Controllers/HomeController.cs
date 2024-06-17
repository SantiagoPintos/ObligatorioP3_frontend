using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using ObligatorioP3_frontend.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ObligatorioP3_frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpClient cliente = new HttpClient();
            Uri uri = new Uri("http://localhost:5029/Login");
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);                                    
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Usuario");
            }           
            return RedirectToAction("Login", "Home");
            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string clave)
        {            
            HttpClient cliente = new HttpClient();
            Uri uri = new Uri("http://localhost:5029/api/Login/Login");
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);            
            UsuarioModel model = new UsuarioModel();
            model.Email = email;
            model.Clave = clave;
            var json = JsonConvert.SerializeObject(model);
            HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
            solicitud.Content = contenido;
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                var usuario = JsonConvert.DeserializeObject<TokenModel>(objetoComoTexto);
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("Token", usuario.Token);
                return RedirectToAction("Index", "Usuario");
            }

            return View();
        }
    }
}
