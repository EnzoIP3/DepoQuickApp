namespace HomeConnect.WebApi.Controllers.BusinessOwner;

public struct CreateCameraRequest
{
    public BusinessLogic.BusinessOwners.Entities.Business Business { get; set; }
    public string Description { get; set; }
    public string MainPhoto { get; set; }
    public int ModelNumber { get; set; }
    public string Name { get; set; }
    public List<string> SecondaryPhotos { get; set; }
    public bool MotionDetection { get; set; }
    public bool PersonDetection { get; set; }
    public bool IsExterior { get; set; }
    public bool IsInterior { get; set; }
}
