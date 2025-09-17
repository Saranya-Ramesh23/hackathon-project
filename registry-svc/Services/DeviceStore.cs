using RegistrySvc.Models;
using System.Collections.Concurrent;

namespace RegistrySvc.Services; 
public class DeviceStore : IDeviceStore {
    
    private readonly ConcurrentDictionary<string, DeviceRecord> _store = new();

    public Task<bool> AddDeviceAsync(DeviceRecord record) {
        var added = _store.TryAdd(record.DeviceId, record);
        return Task.FromResult(added);
    }

    public Task<DeviceRecord?> GetDeviceAsync(string deviceId) {
        _store.TryGetValue(deviceId, out var rec);
        return Task.FromResult(rec);
    }

    public Task<bool> UpdateDeviceAsync(DeviceRecord record) {
        _store[record.DeviceId] = record;
        return Task.FromResult(true);
    }
}
