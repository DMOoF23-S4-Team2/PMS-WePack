using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Interfaces;

namespace PMS.API.Controllers
{    
    [Route("api/[controller]")]    
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly ICsvService _csvService;
        public CsvController(ICsvService csvService){
            _csvService = csvService;
        }         

        [HttpPost("upload-csv")]        
        public async Task<IActionResult> UploadCsv(string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {                
                return BadRequest("File path is required.");
            }
            
            try {                                                     
                await _csvService.DetermineMethod(filepath);
                return Ok();
            }
            catch (Exception ex) {                
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }
    }
}