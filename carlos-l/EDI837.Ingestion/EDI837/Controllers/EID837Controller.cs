using EDI837.src.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDI837.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EID837Controller : ControllerBase
    {
        private readonly IEdi837FileService _edi837FileService; 
        public EID837Controller(IEdi837FileService edi837FileService)
        {
            _edi837FileService = edi837FileService; 
        }

        // GET: api/EID837/GetEdi837FileByName
        [HttpGet("GetEdi837FileByName/{fileName}")]
        public IActionResult GetEdi837FileByName(string fileName)
        {
            this._edi837FileService.GetFileByName(fileName);
            return Ok("Test");
        }

        //// GET: api/EID837
        //[HttpGet]
        //public IActionResult Parse837File()
        //{

        //    return Ok("EDI837 Controller is working.");
        //}
    }
}
