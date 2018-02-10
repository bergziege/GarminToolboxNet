namespace GarminConnectClient
{
    public interface IActivityService
    {
        void Export(string activityId, string fileName, ExportFileType fileType);
    }
}