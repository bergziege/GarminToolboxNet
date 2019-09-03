using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ActivityArchive.Config;
using ActivityArchive.Infrastructure;
using ActivityArchive.Persistence;
using ActivityArchive.Persistence.Impl;
using ActivityArchive.Services;
using ActivityArchive.Services.Impl;
using CommandLine;
using GarminConnectExporter.Config;
using GarminConnectExporter.Infrastructure;
using GarminConnectExporter.Services;
using GarminConnectExporter.Services.Impl;
using Mailer.Config;
using Mailer.Services;
using Mailer.Services.Impl;
using Microsoft.Extensions.Configuration;
using ReportGenerator.Services;
using ReportGenerator.Services.Impl;
using Unity;
using Unity.Lifetime;

namespace GarminConnectExporter
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(args[0], optional: true, reloadOnChange: true)
                .Build();

            DbSettings dbSettings = new DbSettings();
            config.GetSection("DbSettings").Bind(dbSettings);

            GarminConfiguration garminSettings = new GarminConfiguration();
            config.GetSection("GarminSettings").Bind(garminSettings);

            Console.WriteLine($"Garmin-User: {garminSettings.Username}");

            MailConfiguration mailSettings = new MailConfiguration();
            config.GetSection("MailSettings").Bind(mailSettings);

            FileSystemConfiguration fileSystemSettings = new FileSystemConfiguration();
            config.GetSection("FileSystem").Bind(fileSystemSettings);

            IUnityContainer unity = new UnityContainer();
            new DBConfiguration(unity, dbSettings);
            unity.RegisterType<IActivityMetadataDao, ActivityMetadataDao>();
            unity.RegisterType<ISessionService, SessionService>(new ContainerControlledLifetimeManager());
            unity.RegisterType<IActivitySearchService, ActivitySearchService>();
            unity.RegisterType<IActivityService, ActivityService>();
            unity.RegisterType<IReportService, ReportService>();
            unity.RegisterType<IMailService, MailService>();
            unity.RegisterType<IActivityArchiveService, ActivityArchiveService>();

            ISessionService sessionService = unity.Resolve<ISessionService>();
            try
            {
                sessionService.SignOut();
            }
            catch (Exception ex)
            {
            }
            sessionService.SignIn(garminSettings);
            IActivityService activityService = unity.Resolve<IActivityService>();
            activityService.SyncLatestMetadata();
            activityService.SyncFiles(fileSystemSettings);
            activityService.CleanGpxFiles(fileSystemSettings);
            try
            {
                sessionService.SignOut();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            IReportService resportService = unity.Resolve<IReportService>();
            string report = resportService.CreateTextReportForLastSevenDays();
            IMailService mailService = unity.Resolve<IMailService>();
            await mailService.SendMailAsync(mailSettings, report);
        }
    }
}
