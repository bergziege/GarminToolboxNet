namespace GarminToolbox.Core.Service
{
    public interface IActivityService
    {
        void SyncLatestMetadata();
        void SyncFiles();
        void CleanFiles();
    }
}