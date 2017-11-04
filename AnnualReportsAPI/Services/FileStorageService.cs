using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.File.Protocol;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using AnnualReportsAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.IO;
using AnnualReportsAPI.Exceptions;
using System.Diagnostics;

namespace AnnualReportsAPI.Services
{
  public class FileStorageService
  {
    private FileStorageOptions _fileStorageOptions;
    private CloudStorageAccount _storageAccount;
    private CloudFileClient _cloudFileClient;
    private ILogger _logger;

    public FileStorageService(IOptions<FileStorageOptions> fileStorageOptions,
                                ILoggerFactory loggerFactory)
    {
      _fileStorageOptions = fileStorageOptions.Value;
      _logger = loggerFactory.CreateLogger<FileStorageService>();
    }

    private void Init()
    {
      if (this._storageAccount == null)
        this._storageAccount = CloudStorageAccount.Parse(this._fileStorageOptions.ConnectionString);
      if (this._cloudFileClient == null)
        this._cloudFileClient = this._storageAccount.CreateCloudFileClient();
    }

    public async Task UploadFile(Stream fileStream, string clientId, string filename)
    {
      this.Init();

      //GET share & create if needed
      CloudFileShare share = _cloudFileClient.GetShareReference(this._fileStorageOptions.ShareName);
      await share.CreateIfNotExistsAsync();

      try
      {
        //Get root directory & create if needed
        CloudFileDirectory rootDirectory = share.GetRootDirectoryReference();
        await rootDirectory.CreateIfNotExistsAsync();

        //Get clients directory & create if needed
        CloudFileDirectory clientsFolder = rootDirectory.GetDirectoryReference(clientId);
        await clientsFolder.CreateIfNotExistsAsync();

        //Get reference to file
        //If file already exists it will be overwritten
        CloudFile file = clientsFolder.GetFileReference(filename);

        //Upload file
        Stopwatch s = Stopwatch.StartNew();
        await file.UploadFromStreamAsync(fileStream);
      }
      catch (StorageException exStorage)
      {
        this._logger.LogError("Storage error ", exStorage);
        throw new ServiceOperationException("Could not upload file.[storage]");
      }
      catch (Exception ex)
      {
        this._logger.LogError("General error ", ex);
        throw new ServiceOperationException("Could not upload file.[general]");
      }
      finally
      {
        //Do cleanup if needed
      }
    }

    public async Task<MemoryStream> GetFile(string clientId, string filename)
    {
      this.Init();

      CloudFileShare share = this._cloudFileClient.GetShareReference(this._fileStorageOptions.ShareName);
      bool shareExist = await share.ExistsAsync();
      if (shareExist != true)
        throw new ServiceOperationException("No such share.");

      //Get root directory
      CloudFileDirectory rootDirectory = share.GetRootDirectoryReference();
      bool rootDirExist = await rootDirectory.ExistsAsync();
      if (rootDirExist != true)
        throw new ServiceOperationException("No such root dir.");

      //Get clients directory
      CloudFileDirectory clientsFolder = rootDirectory.GetDirectoryReference(clientId);
      bool clientsDirExist = await clientsFolder.ExistsAsync();
      if (clientsDirExist != true)
        throw new ServiceOperationException("No such client dir.");

      //Get reference to file
      //If file already exists it will be overwritten
      CloudFile file = clientsFolder.GetFileReference(filename);
      bool fileExists = await file.ExistsAsync();
      if (fileExists != true)
        throw new ServiceOperationException("No such file");

      MemoryStream ms = new MemoryStream();
      await file.DownloadToStreamAsync(ms);
      ms.Position = 0;

      return ms;
    }
  }
}
