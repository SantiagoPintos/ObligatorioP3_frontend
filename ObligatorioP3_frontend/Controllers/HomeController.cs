using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using ObligatorioP3_frontend.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace ObligatorioP3_frontend.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _cliente;
        private string _url;

        public HomeController()
        {
            _cliente = new HttpClient();
            _url = "http://localhost:5029/api/";
        }

        public IActionResult Index()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                if (token == null) return RedirectToAction("Login", "Home");
                _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);        
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home", new { message = "Algo salió mal"});
            }

        }

        public IActionResult Login(string message)
        {
            ViewBag.Mensaje = message;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string clave)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Uri uri = new Uri(_url + "Login/Login");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);            
                UsuarioModel model = new UsuarioModel();
                model.Email = email;
                model.Clave = clave;
                var json = JsonConvert.SerializeObject(model);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
                respuesta.Wait();
                if (respuesta.Result.IsSuccessStatusCode)
                {
                    var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                    var usuario = JsonConvert.DeserializeObject<TokenModel>(objetoComoTexto);
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetString("Token", usuario.Token);
                    return RedirectToAction("Index", "Usuario");
                }
                return RedirectToAction("Login", "Home", new { message = "Usuario o contraseña incorrectos" });
            } catch (Exception e)
            {
                return RedirectToAction("Login", "Home", new { message = "Algo salió mal" });
            }
        }


        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}
