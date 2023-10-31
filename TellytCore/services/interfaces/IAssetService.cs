using TellytCore.Models;

namespace TellytCore.services.interfaces
{
  public interface IAssetService
  {
    public Task<ServiceResult> MoveFile(int assetId, string path, string fileName, string bucket, bool removeOriginal = true);
    public Task<AssetModel> AddAsset(SaveAssetModel asset);
    public Task DeleteAsset(int customerId, int assetId, string bucketName, string keyName);
    public void CreateBucket(string bucketName);
    public void MoveAsset(int customerId, int assetId, string bucketName, string sourcePath, string destinationPath);
  }
}
