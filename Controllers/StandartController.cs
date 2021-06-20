using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApiMedMyStem.Controllers
{
    /// <summary>
    /// Область запросов для получения стандартов
    /// </summary>
    /// <response code="204">Ошибка: отсутвуют входные данные</response>
    /// <response code="200">Удачный запрос</response>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ProducesResponseType(typeof(Error), 204)]
    public class StandartController : Controller
    {
        /// <summary>
        /// Метод для поиска стандартов по коду мкб-10
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Standart/GetStandartMkb/?data=H35.8&sex=0&age=0
        ///
        /// </remarks>
        /// <param name="data">Код МКБ-10</param>
        /// <param name="sex">Пол пациента</param>
        /// <param name="age">Возраст пациента</param>
        [HttpGet]
        [Route("GetStandartMkb")]
        [ProducesResponseType(typeof(JsonStandart), 200)]
        public JsonResult GetStandartMkb(string data, int sex, int age)
        {
            if (!string.IsNullOrWhiteSpace(data)) return Json(new JsonStandart(0, data, sex, age));
            return Json(new Error { Code = 204 });
        }
        /// <summary>
        /// Метод для поиска стандартов в которых содержится код услуги
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Standart/GetStandartServices/?data=B01.003.001
        ///
        /// </remarks>
        /// <param name="data">Код услуги</param>
        /// <param name="sex">Пол пациента</param>
        /// <param name="age">Возраст пациента</param>
        [HttpGet]
        [Route("GetStandartServices")]
        [ProducesResponseType(typeof(JsonStandart), 200)]
        public JsonResult GetStandartServices(string data, int sex, int age)
        {
            if (!string.IsNullOrWhiteSpace(data)) return Json(new JsonStandart(1, data, sex, age));
            return Json(new Error { Code = 204 });
        }
        /// <summary>
        /// Метод для поиска стандартов в которых содержится лекарство
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Standart/GetStandartMedicament/?data=Блокаторы Н2-гистаминовых рецепторов
        ///
        /// </remarks>
        /// <param name="data">Название лекарства</param>
        /// <param name="sex">Пол пациента</param>
        /// <param name="age">Возраст пациента</param>
        [HttpGet]
        [Route("GetStandartMedicament")]
        [ProducesResponseType(typeof(JsonStandart), 200)]
        public JsonResult GetStandartMedicament(string data, int sex, int age)
        {
            if (!string.IsNullOrWhiteSpace(data)) return Json(new JsonStandart(2, data, sex, age));
            return Json(new Error { Code = 204 });
        }
    }
}
