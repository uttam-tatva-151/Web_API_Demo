using Microsoft.AspNetCore.Mvc;
using Core.DTOs;
using Services.Interfaces;
using Core.Beans.Enums;

namespace Web_API.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturersController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        /// <summary>
        /// Get all manufacturers with their cars
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ManufacturerDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllManufacturers()
        {
            List<ManufacturerDTO> manufacturers = await _manufacturerService.GetAllManufacturersAsync();
            return Ok(manufacturers);
        }

        /// <summary>
        /// Get a manufacturer by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ResponseResult<ManufacturerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetManufacturerById(int id)
        {
            ResponseResult<ManufacturerDTO> manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
            if (manufacturer == null)
                return NotFound(new { Message = $"Manufacturer with ID {id} not found." });

            return Ok(manufacturer);
        }

        /// <summary>
        /// Create a new manufacturer
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateManufacturer([FromBody] ManufacturerDTO manufacturerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseResult<bool> { Status = ResponseStatus.Error, Message = "Invalid data." });

            ResponseResult<bool> result = await _manufacturerService.CreateManufacturerAsync(manufacturerDto);

            if (result.Status == ResponseStatus.Error)
                return BadRequest(result);

            return Ok(result);
            // return CreatedAtAction(nameof(GetManufacturerById), new { id = manufacturerDto.Id, version = "1.0" }, result);
        }

        /// <summary>
        /// Update an existing manufacturer
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateManufacturer(int id, [FromBody] ManufacturerDTO manufacturerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseResult<bool> { Status = ResponseStatus.Error, Message = "Invalid data." });

            ResponseResult<bool> result = await _manufacturerService.UpdateManufacturerAsync(id, manufacturerDto);

            if (result.Status == ResponseStatus.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        ///// <summary>
        ///// Delete a manufacturer by ID
        ///// </summary>
        //[HttpDelete("{id:int}")]
        //[ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteManufacturer(int id)
        //{
        //    var result = await _manufacturerService.DeleteManufacturerAsync(id);
        //    if (!result.Success)
        //        return NotFound(result);
        //
        //    return Ok(result);
        //}
    }
}
