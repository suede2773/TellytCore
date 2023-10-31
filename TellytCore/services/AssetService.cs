using TellytCore.Models;
using TellytCore.services.interfaces;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.MediaConvert;
using Amazon.MediaConvert.Model;
using Amazon.Runtime;
using System.Collections.Generic;

namespace TellytCore.services
{
  public class AssetService : IAssetService
  {
    private readonly IConfiguration _configuration;
    //private readonly IAssetMeta _assetMeta;
    //private readonly IAsset _asset;
    //private readonly IAssetLog _assetLog;
    //private readonly ICustomerMeta _customerMeta;
    private readonly IHelper _helper;
    //public AssetService(IAsset assetDataAccess, IAssetLog assetLogDataAccess,
    //  ICustomerMeta customerMeta, IHelper helper, IConfiguration configuration,
    //  IAssetMeta assetMeta)
    //{
    //  _asset = assetDataAccess;
    //  _assetLog = assetLogDataAccess;
    //  _assetMeta = assetMeta;
    //  _customerMeta = customerMeta;
    //  _helper = helper;
    //  _configuration = configuration;
    //}
    public AssetService(IHelper helper, IConfiguration configuration)
    {
      _helper = helper;
      _configuration = configuration;
    }

    public async Task<AssetModel> AddAsset(SaveAssetModel asset)
    {
      var assetModel = new AssetModel
      {
        CustomerId = asset.CustomerId,
        FileName = asset.FileName,
        Status = "Saving",
        Path = asset.Path
      };
      //var metaList = await _customerMeta.GetCustomerMeta(asset.CustomerId);
      //var bucketName = metaList.First(m => m.MetaKey == "uploadbucket").MetaValue;
      //var domain = metaList.First(m => m.MetaKey == "activedomain").MetaValue;

      //assetModel.BucketName = asset.BucketName;

      //var saveAssetModel = new SaveAssetModel
      //{
      //  CustomerId = asset.CustomerId,
      //  FileName = asset.FileName,
      //  OriginalFileName = asset.OriginalFileName,
      //  BucketName = asset.BucketName,
      //  Path = asset.Path,
      //  Status = "Saving",
      //  Title = string.IsNullOrEmpty(asset.Title) ? string.Empty : asset.Title,
      //  Size = asset.Size
      //};

      //var assetCode = _helper.GenerateAssetCode(8);
      //var existingAsset = await _asset.GetAssetByCode(assetCode);
      //while (existingAsset.Id > 0)
      //{
      //  assetCode = _helper.GenerateAssetCode(8);
      //  existingAsset = await _asset.GetAssetByCode(assetCode);
      //}

      //saveAssetModel.AssetCode = assetCode;
      //assetModel.Id = await _asset.SaveAsset(saveAssetModel);
      //assetModel.AssetCode = assetCode;

      //if (!string.IsNullOrEmpty(asset.JobType))
      //{
      //  _assetMeta.InsertMeta(assetModel.Id, "jobtype", asset.JobType);
      //}

      //if (!string.IsNullOrEmpty(asset.VideoGuid))
      //{
      //  _assetMeta.InsertMeta(assetModel.Id, "videoguid", asset.VideoGuid);
      //}

      //var saveAssetLogModel = new SaveAssetLogModel
      //{
      //  Status = (asset.Url.Length > 0) ? "Set for Download" : "Record Saved",
      //  AssetId = assetModel.Id,
      //  Message = (asset.Url.Length > 0) ? "Video set for Download" : "Asset record saved"
      //};
      //_assetLog.SaveAssetLog(saveAssetLogModel);

      return assetModel;
    }

    public async void CreateBucket(string bucketName)
    {
      try
      {
        var accessKey = _configuration.GetValue<string>("AWSAccessKey");
        var secretKey = _configuration.GetValue<string>("AWSSecretKey");
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        var config = new AmazonS3Config()
        {
          RegionEndpoint = Amazon.RegionEndpoint.USEast1
        };
        var client = new AmazonS3Client(credentials, config);

        await client.PutBucketAsync(bucketName);
      }
      catch (Exception ex)
      {
        var message = ex.Message;
      }
    }


    public async Task<ServiceResult> MoveFile(int assetId, string path, string fileName, string bucket, bool removeOriginal = true)
    {

      var result = new ServiceResult
      {
        ErrorMessage = "",
        Success = false,
        ReturnData = ""
      };

      var dropFolder = _configuration.GetValue<string>("DropFolder");
      var accessKey = _configuration.GetValue<string>("AWSAccessKey");
      var secretKey = _configuration.GetValue<string>("AWSSecretKey");

      if (File.Exists($"{dropFolder}\\{fileName}"))
      {
        try
        {
          var credentials = new BasicAWSCredentials(accessKey, secretKey);
          var config = new AmazonS3Config()
          {
            RegionEndpoint = Amazon.RegionEndpoint.USEast1
          };
          var client = new AmazonS3Client(credentials, config);

          //_assetLog.SaveAssetLog(new SaveAssetLogModel
          //{
          //  Status = "File Located",
          //  AssetId = assetId,
          //  Message = "File located at " + $"{dropFolder}\\{fileName}"
          //});
          var transferUtil = new TransferUtility(client);
          var keyName = (path.Length > 0) ? path + "/" + fileName : fileName;
          transferUtil.UploadAsync(new TransferUtilityUploadRequest
          {
            BucketName = bucket,
            Key = keyName,
            FilePath = $"{dropFolder}\\{fileName}"
          }).Wait();
          result.Success = true;
          result.ErrorMessage = "";
          result.ReturnData = "";
          //_assetLog.SaveAssetLog(new SaveAssetLogModel
          //{
          //  Status = "File Uploaded",
          //  AssetId = assetId,
          //  Message = "File uploaded to S3 bucket " + bucket
          //});
          return result;
        }
        catch (Exception s3Ex)
        {
          result.Success = false;
          result.ErrorMessage = s3Ex.Message;
          //_assetLog.SaveAssetLog(new SaveAssetLogModel
          //{
          //  Status = "Unable to upload file",
          //  AssetId = assetId,
          //  Message = s3Ex.Message
          //});
          return result;
        }
        finally
        {
          if (removeOriginal)
          {
            File.Delete($"{dropFolder}\\{fileName}");
          }
        }
      }
      result.ErrorMessage = "File Not Found";
      //_assetLog.SaveAssetLog(new SaveAssetLogModel
      //{
      //  Status = "Unable to upload file",
      //  AssetId = assetId,
      //  Message = "File Not Found"
      //});
      return result;
    }

    public void MoveAsset(int customerId, int assetId, string bucketName, string sourcePath, string destinationPath)
    {
      //var accessKey = _configuration.GetValue<string>("AWSAccessKey");
      //var secretKey = _configuration.GetValue<string>("AWSSecretKey");
      //var saveAssetLogModel = new SaveAssetLogModel
      //{
      //  Status = "Moving",
      //  AssetId = assetId,
      //  Message = "Moving asset to " + destinationPath
      //};
      //_assetLog.SaveAssetLog(saveAssetLogModel);

      //var assetModel = _asset.GetAssetDetails(customerId, assetId).Result;
      //var sourceKey = (sourcePath.Length > 0) ? sourcePath + "/" + assetModel.FileName : assetModel.FileName;
      //var destinationKey = (destinationPath.Length > 0) ? destinationPath + "/" + assetModel.FileName : assetModel.FileName;

      //var credentials = new BasicAWSCredentials(accessKey, secretKey);
      //var config = new AmazonS3Config()
      //{
      //  RegionEndpoint = Amazon.RegionEndpoint.USEast1
      //};
      //var client = new AmazonS3Client(credentials, config);
      //var copyObjectRequest = new CopyObjectRequest
      //{
      //  DestinationBucket = bucketName,
      //  DestinationKey = destinationKey,
      //  SourceBucket = bucketName,
      //  SourceKey = sourceKey
      //};
      //client.CopyObjectAsync(copyObjectRequest).Wait();
      //client.DeleteObjectAsync(bucketName, sourceKey, null).Wait();
    }

    public async Task DeleteAsset(int customerId, int assetId, string bucketName, string keyName)
    {
      var accessKey = _configuration.GetValue<string>("AWSAccessKey");
      var secretKey = _configuration.GetValue<string>("AWSSecretKey");

      var credentials = new BasicAWSCredentials(accessKey, secretKey);
      var config = new AmazonS3Config()
      {
        RegionEndpoint = Amazon.RegionEndpoint.USEast1
      };
      var client = new AmazonS3Client(credentials, config);
      client.DeleteObjectAsync(bucketName, keyName, null);
    }
  }
}
