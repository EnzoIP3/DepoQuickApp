using System.Text.Json.Serialization;

namespace JsonImporter.Models;

public struct Root
{
    [JsonPropertyName("dispositivos")]
    public List<Dispositivo> Dispositivos { get; set; }
}
