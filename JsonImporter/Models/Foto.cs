using System.Text.Json.Serialization;

namespace JsonImporter.Models;

public struct Foto
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("es_principal")]
    public bool EsPrincipal { get; set; }
}
