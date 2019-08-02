using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using GarminConnectExporter.Infrastructure;
using GarminConnectExporter.Persistence;
using GarminConnectExporter.Persistence.Impl;
using GarminConnectExporter.Services;
using GarminConnectExporter.Services.Impl;
using Unity;
using Unity.Lifetime;

namespace GarminConnectExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);

            System.Console.WriteLine("... finished ...");
            System.Console.ReadLine();
        }

        private static void HandleParseError(IEnumerable<Error> obj)
        {
            System.Console.Write(string.Join(Environment.NewLine, obj.Select(x => x.Tag)));
        }

        private static void Run(Options options)
        {
            IUnityContainer unity = new UnityContainer();
            new DBConfiguration(unity);
            unity.RegisterType<IActivityMetadataDao, ActivityMetadataDao>();
            unity.RegisterType<ISessionService, SessionService>(new ContainerControlledLifetimeManager());
            unity.RegisterType<IActivitySearchService, ActivitySearchService>();
            unity.RegisterType<IActivityService, ActivityService>();
            unity.RegisterType<IActivityService, ActivityService>();

            ISessionService sessionService = unity.Resolve<ISessionService>();
            try
            {
                sessionService.SignOut();
            }
            catch (Exception ex)
            {
            }
            sessionService.SignIn(options.UserName, options.Password);
            IActivityService activityService = unity.Resolve<IActivityService>();
            activityService.SyncLatestMetadata();
            activityService.SyncFiles();
            activityService.CleanFiles();
            sessionService.SignOut();
        }
    }
}
