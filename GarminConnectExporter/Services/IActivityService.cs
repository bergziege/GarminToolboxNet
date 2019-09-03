using System.Collections.Generic;
using ActivityArchive.Config;
using ActivityArchive.Domain;
using GarminConnectExporter.Config;
using GarminConnectExporter.Domain;

namespace GarminConnectExporter.Services
{
    public interface IActivityService
    {
        void SyncLatestMetadata();
        void SyncFiles(FileSystemConfiguration fileSystemConfiguration);
        void CleanGpxFiles(FileSystemConfiguration fileSystemConfiguration);
        void Export(string activityId, string fileName, ExportFileType fileType);
        
    }
}