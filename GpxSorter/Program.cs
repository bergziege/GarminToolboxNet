using System;
using System.Collections.Generic;
using System.IO;
using ActivityArchive.Config;
using ActivityArchive.Domain;
using ActivityArchive.Infrastructure;
using ActivityArchive.Persistence;
using ActivityArchive.Persistence.Impl;
using ActivityArchive.Services;
using ActivityArchive.Services.Impl;
using Unity;

namespace GpxSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            DbSettings dbSettings = new DbSettings();
            dbSettings.Server = "192.168.2.101";
            dbSettings.Database = "garmin_toolbox_net";
            dbSettings.Username = "garmintoolboxnet";
            dbSettings.Password = "ydumuquOjsa10KUNOw2R";

            FileSystemConfiguration fileSystem = new FileSystemConfiguration();
            fileSystem.GpxFileDownloadDirectory = "U:\\";

            DirectoryInfo gpxOutputDir = new DirectoryInfo(@"C:\Temp\OrderedGpx");
            if (!gpxOutputDir.Exists)
            {
                gpxOutputDir.Create();
            }

            IUnityContainer container = new UnityContainer();
            new DBConfiguration(container, dbSettings);
            container.RegisterType<IActivityArchiveService, ActivityArchiveService>();
            container.RegisterType<IActivityMetadataDao, ActivityMetadataDao>();

            IActivityArchiveService archive = container.Resolve<IActivityArchiveService>();
            IList<ActivityMetadata> allWithGpx = archive.GetAllWithGpx();

            foreach (ActivityMetadata activityMetadata in allWithGpx)
            {
                FileInfo gpxFile = new FileInfo(Path.Combine(fileSystem.GpxFileDownloadDirectory,
                    $"{activityMetadata.ActivityId}.gpx"));
                if (gpxFile.Exists && activityMetadata.Start.HasValue)
                {
                    DirectoryInfo destinationFolder = new DirectoryInfo(Path.Combine(gpxOutputDir.FullName,
                        activityMetadata.Start.Value.Year.ToString(),
                        activityMetadata.Start.Value.Month.ToString("D2")));
                    if (!destinationFolder.Exists)
                    {
                        destinationFolder.Create();
                    }

                    string destinationFile =
                        $"{activityMetadata.Start.Value:yyyy-MM-dd-HH-mm-ss}-{activityMetadata.ActivityType}-{activityMetadata.ActivityId}{gpxFile.Extension}";

                    gpxFile.CopyTo(Path.Combine(destinationFolder.FullName, destinationFile));
                }
            }
        }
    }
}