using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObligatorioP3_frontend.Models;
using System.Net.Http.Headers;

namespace ObligatorioP3_frontend.Controllers
{
    public class ConsultasController : Controller
    {
        private HttpClient _cliente;
        private string _url;
        private static IEnumerable<ArticuloModel> _articulos;
        private static IEnumerable<TipoMovimientoModel> _tiposMovimientos;
        private static int _paginaActual = -1;
        private static int _articuloId = -1;
        private static string _tipoMovimientoNombre = null;

        public ConsultasController()
        {
            _cliente = new HttpClient();
            _url = "http://localhost:5029/api/";
        }
        public IActionResult Index()
        {
            _articuloId = -1;
            _paginaActual = -1;
            _tipoMovimientoNombre = null;
            return View();
        }

        public ActionResult ObtenerMovimientos(string mensaje, string mensajeError)
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
                    ViewBag.mensaje = mensaje;
                    ViewBag.mensajeError = mensajeError;
                return View();
        }

        public ActionResult ObtenerMovimientosBusqueda(int idArticulo, string tipoMovimientoNombre, string mensaje, string mensajeError, int paginas)
        {   
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ViewBag.mensaje = mensaje;
            ViewBag.mensajeError = mensajeError;

            if(_articuloId == -1 ) _articuloId = idArticulo;
            if(String.IsNullOrEmpty(_tipoMovimientoNombre)) _tipoMovimientoNombre = tipoMovimientoNombre;

            if (_paginaActual < 1) _paginaActual = paginas;
            // Listar movimientos por id y tipo de movimiento

            Uri uri = new Uri(_url + "Consultas/MovimientosIdTipo/" + _articuloId + "/" + _tipoMovimientoNombre + "/" + _paginaActual);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<IEnumerable<MovimientoModel>>(objetoComoTexto);
                if (json == null)
                {
                    return RedirectToAction("ObtenerMovimientos", new { mensajeError = "No se encontraron movimientos" });
                }
                ViewBag.tipos = _tiposMovimientos;
                ViewBag.articulos = _articulos;
                ViewBag.movimientos = json;               
            }
            return View();

        }

        [HttpPost]
        public ActionResult Next()
        {
            try
            {
                string mensaje = "";
                _paginaActual++;
                if (_paginaActual < 1)
                {
                    _paginaActual = 1;
                    mensaje = "Página no válida";
                }

                return RedirectToAction("ObtenerMovimientosBusqueda", new { mensajeError = mensaje });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Usuario", new { mensajeError = "Algo salió mal" });
            }
        }

        [HttpPost]
        public ActionResult Previous()
        {
            try
            {
                string mensaje = "";
                _paginaActual--;
                if (_paginaActual < 1)
                {
                    _paginaActual = 1;
                    mensaje = "Página no válida";
                }

                return RedirectToAction("ObtenerMovimientosBusqueda", new { mensajeError = mensaje });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Usuario", new { mensajeError = "Algo salió mal" });
            }
        }

        public ActionResult ObtenerMovimientosEntreFechas()
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");                   
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerMovimientosEntreFechas(DateTime fechaInicial, DateTime fechaFinal, int paginas)
        {
            string fechaInicialFormateada = fechaInicial.ToString("dd-MM-yyyy");    
            string fechaFinalFormateada = fechaFinal.ToString("dd-MM-yyyy");
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Listar movimientos por id y tipo de movimiento
            Uri uri = new Uri(_url + "Consultas/" + "MovimientosEntreFechas/" + fechaInicialFormateada + "/" + fechaFinalFormateada);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<IEnumerable<ArticuloModel>>(objetoComoTexto);
                if (json == null)
                {
                    return RedirectToAction("ObtenerMovimientosEntreFechas", new { mensajeError = "No se encontraron movimientos" });
                }
                ViewBag.articulosAMostrar = json;
            }
            return View();
        }




        public ActionResult ResumenCantidadMovidas(string mensajeError)
        {
            string token = HttpContext.Session.GetString("Token");
            if (token == null) return RedirectToAction("Login", "Home");
            _cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Uri uri = new Uri(_url + "Consultas/" + "ResumenCantidades");
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = _cliente.SendAsync(solicitud);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                var objetoComoTexto = respuesta.Result.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<IEnumerable<ResumenModel>>(objetoComoTexto);
                if (json == null)
                {
                    return RedirectToAction("ResumenCantidadMovidas", new { mensajeError = "No se encontraron movimientos" });
                }
                ViewBag.resumenes = json;
                ViewBag.mensajeError = mensajeError;    
            }
            return View();
        }






    }
}
