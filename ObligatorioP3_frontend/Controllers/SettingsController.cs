using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObligatorioP3_frontend.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ObligatorioP3_frontend.Controllers
{
    public class SettingsController : Controller
    {
        private HttpClient _cliente;
        private string _url;
        private static IEnumerable<ArticuloModel> _articulos;
        private static IEnumerable<TipoMovimientoModel> _tiposMovimientos;


        public SettingsController()
        {
            _cliente = new HttpClient();
            _url = "http://localhost:5029/api/";
        }
        public IActionResult Index(string mensaje, string mensajeError)
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            ViewBag.mensaje = mensaje;  
            ViewBag.mensajeError = mensajeError;
            return View();
        }

        [HttpPost]
        public ActionResult ModificarSettings(SettingsModel settings)
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Uri uri = new Uri(_url + "Settings/" + settings.Nombre);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
            SettingsModel settingsModel = new SettingsModel();
            settingsModel.Nombre = settings.Nombre;
            settingsModel.Valor = settings.Valor;
            var json = JsonConvert.SerializeObject(settingsModel);
            HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
            solicitud.Content = contenido;
            Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                ViewBag.mensaje = "Settings modificados correctamente";                
                return RedirectToAction("Index", new { mensaje = "Settings modificados correctamente" });
            }
            return RedirectToAction("Index" , new { mensajeError = "Error al modificar settings" });
        }




    }
}
