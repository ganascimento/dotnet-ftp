using System.Net;

namespace Dotnet.Ftp.Api.Services;

public class FtpService
{
    private readonly string _ftpUserName;
    private readonly string _ftpPassword;
    private readonly string _ftpHost;

    public FtpService(IConfiguration configuration)
    {
        _ftpUserName = configuration["FTP:UserName"] ?? throw new Exception("UserName not found");
        _ftpPassword = configuration["FTP:Password"] ?? throw new Exception("Password not found");
        _ftpHost = configuration["FTP:Host"] ?? throw new Exception("Host not found");
    }

    public async Task<List<string>> GetAsync()
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpHost);
        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

        FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
        string line = "";
        List<string> fileNames = new List<string>();
        using (Stream responseStream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(responseStream))
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    fileNames.Add(line.Substring(56));
                    line = reader.ReadLine();
                }
            }
        }

        return fileNames;
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpHost + FormatFileName(fileName));
        request.Method = WebRequestMethods.Ftp.DownloadFile;
        request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

        FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();

        Stream responseStream = response.GetResponseStream();

        return responseStream;
    }

    public async Task UploadAsync(string filePath, string fileName)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpHost + FormatFileName(fileName));
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (Stream requestStream = request.GetRequestStream())
        {
            await fileStream.CopyToAsync(requestStream);
        }
    }

    public async Task DeleteAsync(string fileName)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpHost + FormatFileName(fileName));
        request.Method = WebRequestMethods.Ftp.DeleteFile;
        request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

        await request.GetResponseAsync();
    }

    private string FormatFileName(string fileName)
    {
        return fileName.Replace(" ", "_");
    }
}