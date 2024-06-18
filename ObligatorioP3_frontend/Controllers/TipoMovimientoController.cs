using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObligatorioP3_frontend.Models;
using System.Net.Http.Headers;

namespace ObligatorioP3_frontend.Controllers
{
    public class TipoMovimientoController : Controller
    {

        private HttpClient _cliente;
        private string _url;

        public TipoMovimientoController()
        {
            _cliente = new HttpClient();
            _url = "http://localhost:5029/api/";
        }

        // GET: TipoMovimientoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TipoMovimientoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoMovimientoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoMovimientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoMovimientoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TipoMovimientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoMovimientoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TipoMovimientoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult VolverAlInicio()
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuario");

            return RedirectToAction("Index", "Usuario");
        }
        public ActionResult ListarTiposDeMovimiento()
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                if (token == null) return RedirectToAction("Login", "Home");
                _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Uri uri = new Uri(_url + "TipoMovimiento/listarTiposDeMovimientos");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
                respuesta.Wait();
                if (respuesta.Result.IsSuccessStatusCode)
                {
                    var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                    var listaTiposDeMovimiento = JsonConvert.DeserializeObject<TipoMovimientoModel>(objetoComoTexto);
                    return View(listaTiposDeMovimiento);
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home", new { message = "Algo salió mal" });
            }
            return View();
        }

    }
}
