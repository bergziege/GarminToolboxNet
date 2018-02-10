namespace GarminToolbox.Core.Service
{
    public interface IActivityService
    {
        void SyncLatestMetadata();
        void SyncOriginalFiles();
        void SyncGpx();
        void SyncFiles();
        void CleanFiles();
    }
}