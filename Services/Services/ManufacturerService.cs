using Common.ConstantHandler;
using Common.ConstantHandler.Enums;
using Core.Beans;
using Core.Beans.Enums;
using Core.DTOs;
using Data.Entity;
using Data.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;

namespace Services.Services;

public class ManufacturerService(IUnitOfWork unitOfWork, IMemoryCache cache) : IManufacturerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IMemoryCache _cache = cache;
    public async Task<ResponseResult<bool>> CreateManufacturerAsync(ManufacturerDTO manufacturer)
    {
        try
        {
            Manufacturer newManufacturer = new()
            {
                Name = manufacturer.Name,
                Country = manufacturer.Country,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Manufacturers.AddAsync(newManufacturer);
            int successFlag = await _unitOfWork.CompleteAsync();

            return new ResponseResult<bool>
            {
                Status = successFlag > 0
                            ? ResponseStatus.Success
                            : ResponseStatus.Error,
                Message = successFlag > 0
                            ? MessageHelper.GetSuccessMessage(OperationType.Add, Constants.MANUFACTURER)
                            : MessageHelper.GetErrorMessage(OperationType.Add, Constants.MANUFACTURER),
                Data = successFlag > 0
            };
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessage(OperationType.Read, Constants.MANUFACTURER), ex);
        }
    }

    public async Task<List<ManufacturerDTO>> GetAllManufacturersAsync(CancellationToken cancellationToken)
    {
        try
        {
            List<ManufacturerDTO> manufacturerList = [];
            // Try to get from cache first
            if (!_cache.TryGetValue("AllManufacturers", out manufacturerList))
            {
                // Cache miss: fetch from DB
                IEnumerable<Manufacturer> manufacturers = await _unitOfWork.Manufacturers.GetListAsync(cancellationToken: cancellationToken);

                manufacturerList = [.. manufacturers.Select(m => new ManufacturerDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Country = m.Country,
                    CreatedAt = m.CreatedAt
                })];

                // Set cache options
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Sliding window: reset expiration on access
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Max cache life

                // Save data in cache
                _cache.Set("AllManufacturers", manufacturerList, cacheEntryOptions);
            }
            return manufacturerList;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessage(OperationType.Read, Constants.MANUFACTURER), ex);
        }
    }

    public async Task<ResponseResult<ManufacturerDTO>> GetManufacturerByIdAsync(int id)
    {
        try
        {
            ResponseResult<ManufacturerDTO> result = new();
            ManufacturerDTO? manufacturer = await _unitOfWork.Manufacturers.GetFirstOrDefaultAsync
                                                        ( new QueryOptions<Manufacturer, ManufacturerDTO>
                                                            {
                                                                Predicate = m => m.Id == id,
                                                                Selector = m => new ManufacturerDTO
                                                                {
                                                                    Id = m.Id,
                                                                    Name = m.Name,
                                                                    Country = m.Country,
                                                                    CreatedAt = m.CreatedAt
                                                                },
                                                                // AsNoTracking defaults to true; leave as-is for read-only projection
                                                            }
                                                        );
            if (manufacturer == null)
            {
                result.Status = ResponseStatus.Error;
                result.Message = $"Manufacturer with ID {id} not found.";
                result.Data = null;
            }
            else
            {
                result.Status = ResponseStatus.Success;
                result.Message = MessageHelper.GetSuccessMessage(OperationType.Read, Constants.MANUFACTURER);
                result.Data = manufacturer;
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessage(OperationType.Read, Constants.MANUFACTURER), ex);
        }
    }

    public async Task<ResponseResult<bool>> UpdateManufacturerAsync(int id, ManufacturerDTO manufacturer)
    {
        try
        {
            ResponseResult<bool> result = new();
            Manufacturer? existingManufacturer = await _unitOfWork.Manufacturers.
                                                        GetFirstOrDefaultAsync(new QueryOptions<Manufacturer>
                                                                            {
                                                                                Predicate = m => m.Id == id,
                                                                                AsNoTracking = false // match original call which used asNoTracking: false
                                                                            });
            if (existingManufacturer == null)
            {
                result.Status = ResponseStatus.NotFound;
                result.Message = $"Manufacturer with ID {id} not found.";
                result.Data = false;
                return result;
            }
            else
            {
                existingManufacturer.Name = manufacturer.Name;
                existingManufacturer.Country = manufacturer.Country;

                _unitOfWork.Manufacturers.Update(existingManufacturer);
                int successFlag = await _unitOfWork.CompleteAsync();

                result.Status = successFlag > 0
                            ? ResponseStatus.Success
                            : ResponseStatus.Error;
                result.Message = successFlag > 0
                            ? MessageHelper.GetSuccessMessage(OperationType.Update, Constants.MANUFACTURER)
                            : MessageHelper.GetErrorMessage(OperationType.Update, Constants.MANUFACTURER);
                result.Data = successFlag > 0;
                return result;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessage(OperationType.Update, Constants.MANUFACTURER), ex);
        }
    }

}
