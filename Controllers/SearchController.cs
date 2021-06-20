using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiMedMyStem.Controllers
{     
      /// <summary>
      /// Область запросов для получения нахождения соответвий по названию элементов, с учетом всех аббревиатур, опечаток, сокращений и т.д.
      /// </summary>
      /// <response code="204">Ошибка: отсутвуют входные данные</response>
      /// <response code="200">Удачный запрос</response>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ProducesResponseType(typeof(Error), 204)]
    public class SearchController : Controller
    {
        /// <summary>
        /// Метод для поиска услуг с кодами по строке
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Search/GetServices/?data=оам
        ///
        /// </remarks>
        /// <param name="data">Строка, которую необходимо изучить</param>
        [HttpGet]
        [Route("GetServices")]
        [ProducesResponseType(typeof(JsonServices), 200)]
        public JsonResult GetServices(string data)
        {
            return PostServices(new List<string> { data });
        }
        /// <summary>
        /// Метод для поиска услуг с кодами по массиву строк
        /// </summary>
        /// <param name="data">Массив строк, которые необходимо изучить</param>
        [HttpPost]
        [Route("PostServices")]
        [ProducesResponseType(typeof(JsonServices), 200)]
        public JsonResult PostServices([FromQuery] List<string> data)
        {
            if (data.Any() && data.Select(u => !string.IsNullOrWhiteSpace(u)).Any(u => u) && data.Count < MaxElement.MaxGet)
                return Json(new JsonServices(data));
            return Json(new Error { Code = 204 });
        }
        /// <summary>
        /// Метод для поиска лекарств в строке
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Search/GetMedicines/?data=НИТРОКСОЛИН ФТОРУРАЦИЛ
        ///
        /// </remarks>
        /// <param name="data">Строка, которую необходимо изучить</param>
        [HttpGet]
        [Route("GetMedicines")]
        [ProducesResponseType(typeof(JsonMedicines), 200)]
        public JsonResult GetMedicines(string data)
        {
            if (!string.IsNullOrWhiteSpace(data)) return Json(new JsonMedicines(data));
            return Json(new Error { Code = 204 });
        }
        /// <summary>
        /// Метод для поиска заболеваний в строке 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Search/GetMkb/?data=H35.8 - Другие уточненные ретинальные нарушения
        ///
        /// </remarks>
        /// <param name="data">Строка, которую необходимо изучить</param>
        [HttpGet]
        [Route("GetMkb")]
        [ProducesResponseType(typeof(JsonMkb), 200)]
        public JsonResult GetMkb(string data)
        {
            return PostMkbSet(new List<string> { data });
        }
        /// <summary>
        /// Метод для поиска заболеваний в массиве строк 
        /// </summary>
        /// <param name="data">Массив строк, которые необходимо изучить</param>
        [HttpPost]
        [Route("PostMkbSet")]
        [ProducesResponseType(typeof(JsonMkb), 200)]
        public JsonResult PostMkbSet([FromQuery] List<string> data)
        {
            if (data.Any() && data.Select(u => !string.IsNullOrWhiteSpace(u)).Any(u => u) && data.Count < MaxElement.MaxGet)
                return Json(new JsonMkb(data));
            return Json(new Error { Code = 204 });
        }
    }
}
