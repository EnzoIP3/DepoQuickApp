using System.Text.Json;
using DeviceImporter;
using DeviceImporter.Models;
using JsonImporter.Models;

namespace JsonImporter;

public class JsonDeviceImporter : IDeviceImporter
{
    private readonly Dictionary<string, string> _params = new()
    {
        ["fileName"] = "fileName"
    };
    private readonly string _directoryRoute = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportFiles");
    public List<DeviceArgs> ImportDevices(Dictionary<string, string> parameters)
    {
        var route = parameters[_params["fileName"]];
        route = Path.Combine(_directoryRoute, route);
        Console.WriteLine(route);
        EnsureFileExists(route);
        var json = File.ReadAllText(route);
        EnsureJsonIsNotNull(json);
        Root? deviceList = DeserializeJson(json);
        EnsureDeviceListIsNotNull(deviceList);
        return deviceList.Value.Dispositivos!.Select(ToDeviceArgs).ToList();
    }

    public List<string> GetParams()
    {
        return _params.Keys.ToList();
    }

    private static void EnsureFileExists(string route)
    {
        if (!File.Exists(route))
        {
            throw new FileNotFoundException("File not found", route);
        }
    }

    private static void EnsureDeviceListIsNotNull(Root? deviceList)
    {
        if (deviceList == null)
        {
            throw new InvalidOperationException("Error deserializing json");
        }
    }

    private static Root DeserializeJson(string json)
    {
        Root deviceList;
        try
        {
            deviceList = JsonSerializer.Deserialize<Root>(json);
        }
        catch (Exception ex) when (ex is JsonException || ex is NotSupportedException)
        {
            throw new JsonException("Error deserializing json", ex);
        }

        return deviceList;
    }

    private static void EnsureJsonIsNotNull(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new InvalidOperationException("File is empty");
        }
    }

    private DeviceArgs ToDeviceArgs(Dispositivo device)
    {
        DeviceArgs deviceArgs;
        var tipo = device.Tipo switch
        {
            "camera" => "Camera",
            "sensor-open-close" => "Sensor",
            "sensor-movement" => "MotionSensor",
            _ => throw new InvalidOperationException("Invalid device type")
        };
        if (tipo != "Camera")
        {
            deviceArgs = new DeviceArgs
            {
                Name = device.Nombre,
                ModelNumber = device.Modelo,
                MainPhoto = device.Fotos.First(foto => foto.EsPrincipal).Path,
                SecondaryPhotos = device.Fotos.Where(foto => !foto.EsPrincipal).Select(foto => foto.Path).ToList(),
                Type = tipo,
                Description = "Imported device with model number " + device.Modelo + " and name " + device.Nombre
            };
        }
        else
        {
            deviceArgs = new DeviceArgs
            {
                Name = device.Nombre,
                ModelNumber = device.Modelo,
                MainPhoto = device.Fotos.First(foto => foto.EsPrincipal).Path,
                SecondaryPhotos = device.Fotos.Where(foto => !foto.EsPrincipal).Select(foto => foto.Path).ToList(),
                Type = tipo,
                Description = "Imported device with model number " + device.Modelo + " and name " + device.Nombre,
                MotionDetection = device.MovementDetection,
                PersonDetection = device.PersonDetection
            };
        }

        return deviceArgs;
    }
}
