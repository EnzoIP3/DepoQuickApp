using System.Text.Json.Serialization;

namespace JsonImporter.Models;

public struct Dispositivo
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }

    [JsonPropertyName("modelo")]
    public string Modelo { get; set; }

    [JsonPropertyName("fotos")]
    public List<Foto> Fotos { get; set; }

    [JsonPropertyName("person_detection")]
    public bool? PersonDetection { get; set; }

    [JsonPropertyName("movement_detection")]
    public bool? MovementDetection { get; set; }
}
