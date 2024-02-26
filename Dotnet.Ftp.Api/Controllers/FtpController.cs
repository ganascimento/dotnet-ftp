using Dotnet.Ftp.Api.Models;
using Dotnet.Ftp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Ftp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FtpController : ControllerBase
{
    private readonly FtpService _ftpService;

    public FtpController(FtpService ftpService)
    {
        _ftpService = ftpService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var fileNames = await _ftpService.GetAsync();

            return Ok(new
            {
                files = fileNames
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("download")]
    public async Task<IActionResult> Download([FromQuery] string fileName, [FromQuery] string? contentType)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName))
                throw new Exception("Invalid file name");

            var fileStream = await _ftpService.DownloadAsync(fileName);

            return File(fileStream, contentType ?? "text/plain", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload([FromForm] FormFileModel model)
    {
        try
        {
            var filePath = Path.GetTempFileName();
            string fileName = "";
            var formFile = model.File;
            if (formFile.Length > 0)
            {
                using (var inputStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(inputStream);
                    byte[] array = new byte[inputStream.Length];
                    inputStream.Seek(0, SeekOrigin.Begin);
                    inputStream.Read(array, 0, array.Length);
                    fileName = formFile.FileName;
                }
            }

            await _ftpService.UploadAsync(filePath, fileName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string fileName)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName))
                throw new Exception("Invalid file name");

            await _ftpService.DeleteAsync(fileName);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}