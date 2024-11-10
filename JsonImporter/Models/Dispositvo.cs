namespace JsonImporter.Models;

public struct Dispositivo
{
    public Guid Id { get; set; }
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public List<Foto> Fotos { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? MovementDetection { get; set; }
}
