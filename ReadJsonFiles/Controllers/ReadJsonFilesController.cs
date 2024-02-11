using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadJsonFiles.Interface;

namespace ReadJsonFiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadJsonFilesController : ControllerBase
    {
        private readonly IReadJsonFile _myService;

        public ReadJsonFilesController(IReadJsonFile myService)
        {
            _myService = myService;
        }
        [HttpGet("GetReadJsonFile", Name = "GetReadJsonFile")]
        public IActionResult GetReadJsonFile()
        {
            return Ok(_myService.ReadJsonFileAsync());
        }
        [HttpGet("GetReadLEIJsonFile", Name = "GetReadLEIJsonFile")]
        public IActionResult GetReadLEIJsonFile()
        {
            return Ok(_myService.ReadLEIJsonFileAsync());
        }
        [HttpGet("GetReadExceptionJsonFile", Name = "GetReadExceptionJsonFile")]
        public IActionResult GetReadExceptionJsonFile()
        {
            return Ok(_myService.ReadEcpJsonFileAsync());
        }

        [HttpGet("CreateRRRJsonFile", Name = "CreateRRRJsonFile")]
        public IActionResult CreateRRRJsonFile()
        {
            return Ok(_myService.CreateRRRJsonFile());
        }
        [HttpGet("CreateLeiXmlFile", Name = "CreateLeiXmlFile")]
        public IActionResult CreateLeiXmlFile()
        {
            return Ok(_myService.CreateLeiXmlFile());
        }
    }
}
