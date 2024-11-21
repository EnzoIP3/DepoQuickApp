namespace HomeConnect.WebApi.Controllers.DeviceImportFiles.Models;

public sealed record GetImportFilesResponse
{
    public List<string> ImportFiles { get; set; } = [];
}
