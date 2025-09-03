using Core.Beans.Enums;
using Core.DTOs;
using Data.Entity;

namespace Services.Interfaces;

public interface IManufacturerService
{
    Task<List<ManufacturerDTO>> GetAllManufacturersAsync();
    Task<ResponseResult<ManufacturerDTO>> GetManufacturerByIdAsync(int id);
    Task<ResponseResult<bool>> CreateManufacturerAsync(ManufacturerDTO manufacturer);
    Task<ResponseResult<bool>> UpdateManufacturerAsync(int id, ManufacturerDTO manufacturer);
    // Task<ResponseResult<bool>> DeleteManufacturerAsync(int id);
}

