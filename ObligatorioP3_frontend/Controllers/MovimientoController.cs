using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObligatorioP3_frontend.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ObligatorioP3_frontend.Controllers
{
    public class MovimientoController : Controller
    {
        private HttpClient _cliente;
        private string _url;
        private static IEnumerable<ArticuloModel> _articulos;
        private static IEnumerable<TipoMovimientoModel> _tiposMovimientos;


        public MovimientoController()
        {
            _cliente = new HttpClient();
            _url = "http://localhost:5029/api/";
        }


        // GET: MovimientoController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VolverAlInicio()
        {
            return RedirectToAction("Index", "Usuario");
        }

        // GET: MovimientoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovimientoController/Create
        public ActionResult Create(string mensaje)
        {
            
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Listar articulos
            Uri uri = new Uri(_url + "Articulo");
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);    
            Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
            respuesta.Wait();

            // Listar Tipos de Movimiento
            Uri uriTipos = new Uri(_url + "TipoMovimiento");
            HttpRequestMessage solicitudTipos = new HttpRequestMessage(HttpMethod.Get, uriTipos);
            Task<HttpResponseMessage> respuestaTipos = _cliente.SendAsync(solicitudTipos);
            respuestaTipos.Wait();
            if (respuesta.Result.IsSuccessStatusCode && respuestaTipos.Result.IsSuccessStatusCode)
            {
                var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<IEnumerable<ArticuloModel>>(objetoComoTexto);
                var objetoComoTextoTipos = respuestaTipos.Result.Content.ReadAsStringAsync().Result;
                var jsonTipos = JsonConvert.DeserializeObject<IEnumerable<TipoMovimientoModel>>(objetoComoTextoTipos);
                _articulos = json;
                _tiposMovimientos = jsonTipos;
                ViewBag.tipos = jsonTipos;
                ViewBag.articulos = json;                
            }
            ViewBag.Mensaje = mensaje;
            return View();
        }



        // POST: MovimientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovimientoModel movimientoModel, int ArticuloId, int tipoMovimientoId)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                string email = HttpContext.Session.GetString("Email");
                if (token == null) return RedirectToAction("Login", "Home");
                _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                ArticuloModel articulo = null;
                foreach(ArticuloModel articuloLista in _articulos)
                {
                    bool encontrado = false;
                    if (articuloLista.Id == ArticuloId && !encontrado)
                    {
                        articulo = articuloLista;
                        encontrado = true;
                    }
                }
                TipoMovimientoModel tipoMovimiento = null;
                foreach (TipoMovimientoModel tipoMovimientoLista in _tiposMovimientos)
                {
                    bool encontrado = false;
                    if (tipoMovimientoLista.Id == tipoMovimientoId)
                    {
                        tipoMovimiento = tipoMovimientoLista;
                        encontrado = true;
                    }
                }


                Uri uri = new Uri(_url + "MovimientoStock");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);                
                MovimientoModel movimiento = new MovimientoModel();
                movimiento.Articulo = articulo;
                movimiento.Cantidad = movimientoModel.Cantidad;
                movimiento.TipoMovimiento = tipoMovimiento;
                movimiento.Usuario = email;
                movimiento.Fecha = movimientoModel.Fecha;                
                var json = JsonConvert.SerializeObject(movimiento);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
                respuesta.Wait();
                if (respuesta.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Create", new { mensaje = "Creado exitosamente" });
                }
                return RedirectToAction("Index", "Usuario");
            }
            catch
            {
                return View();
            }
        }
       



        // GET: MovimientoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MovimientoController/Edit/5
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

        // GET: MovimientoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovimientoController/Delete/5
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
    }
}
