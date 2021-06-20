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
    /// Область запросов для парсинга отдельных строк
    /// </summary>
    /// <response code="204">Ошибка: отсутвуют входные данные</response>
    /// <response code="200">Удачный запрос</response>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Error), 204)]
    public class ParserController : Controller
    {
        /// <summary>
        /// Метод для получения списка кодов МКБ-10 из строки
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     Get api/Parser/GetCode/?data=i25.1 i 11.9
        ///
        /// </remarks>
        /// <param name="data">Строка, которую необходимо изучить</param>
        [HttpGet]
        [Route("GetCode")]
        [ProducesResponseType(typeof(JsonGetCode), 200)]
        public JsonResult GetCode(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
                return Json(new JsonGetCode
                {
                    Code = 200,
                    LastName = data,
                    Search = ParserMkb.GetCode(data)
                });
            return Json(JsonSerializer.Serialize(new Error { Code = 204 }));
        }
        /// <summary>
        /// Метод для получения списка заболеваний из строки
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get api/Parser/GetDisease/?data=СД тип 2
        ///
        /// </remarks>
        /// <param name="data">Строка, которую необходимо изучить</param>
        [HttpGet]
        [Route("GetDisease")]
        [ProducesResponseType(typeof(JsonGetCode), 200)]
        public JsonResult GetDisease(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
                return Json(new JsonGetCode
                {
                    Code = 200,
                    LastName = data,
                    Search = ParserMkb.GetDiases(ParserUpdateName.UpdateNameDiases(data))
                });
            return Json(JsonSerializer.Serialize(new Error { Code = 204 }));
        }
    }
}
