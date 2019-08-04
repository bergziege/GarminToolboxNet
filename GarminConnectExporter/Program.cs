using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using GarminConnectExporter.Config;
using GarminConnectExporter.Infrastructure;
using GarminConnectExporter.Persistence;
using GarminConnectExporter.Persistence.Impl;
using GarminConnectExporter.Services;
using GarminConnectExporter.Services.Impl;
using Microsoft.Extensions.Configuration;
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
                .AddJsonFile("App.config.json", optional: true, reloadOnChange: true)
                .Build();

            DbSettings dbSettings = new DbSettings();
            config.GetSection("DbSettings").Bind(dbSettings);

            GarminConfiguration garminSettings = new GarminConfiguration();
            config.GetSection("GarminSettings").Bind(garminSettings);

            MailConfiguration mailSettings = new MailConfiguration();
            config.GetSection("MailSettings").Bind(mailSettings);

            IUnityContainer unity = new UnityContainer();
            new DBConfiguration(unity, dbSettings);
            unity.RegisterType<IActivityMetadataDao, ActivityMetadataDao>();
            unity.RegisterType<ISessionService, SessionService>(new ContainerControlledLifetimeManager());
            unity.RegisterType<IActivitySearchService, ActivitySearchService>();
            unity.RegisterType<IActivityService, ActivityService>();
            unity.RegisterType<IActivityService, ActivityService>();
            unity.RegisterType<IReportService, ReportService>();
            unity.RegisterType<IMailService, MailService>();

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
            activityService.SyncFiles();
            activityService.CleanFiles();
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
