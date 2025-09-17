using RegistrySvc.Models;

namespace RegistrySvc.Services; 

public interface IDeviceStore 
{
    Task<bool> AddDeviceAsync(DeviceRecord record);
    Task<DeviceRecord?> GetDeviceAsync(string deviceId);
    Task<bool> UpdateDeviceAsync(DeviceRecord record);
}