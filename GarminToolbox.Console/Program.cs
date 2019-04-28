using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Dommel;
using GarminConnectClient;
using GarminConnectClient.Data;
using GarminConnectClient.Demo;
using GarminToolbox.Core.Config;
using GarminToolbox.Core.Domain;
using GarminToolbox.Core.Persistence;
using GarminToolbox.Core.Persistence.Impl;
using GarminToolbox.Core.Service;
using Unity;
using Unity.Lifetime;
using ActivityService = GarminToolbox.Core.Service.Impl.ActivityService;
using IActivityService = GarminToolbox.Core.Service.IActivityService;

namespace GarminToolbox.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                .WithNotParsed(HandleParseError);

            System.Console.WriteLine("... finished ...");
            System.Console.ReadLine();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            System.Console.Write(string.Join(Environment.NewLine, errs.Select(x=>x.Tag)));
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            IUnityContainer unity = new UnityContainer();
            new DBConfiguration(unity);
            unity.RegisterType<IActivityMetadataDao, ActivityMetadataDao>();
            unity.RegisterType<ISessionService, SessionService>(new ContainerControlledLifetimeManager());
            unity.RegisterType<IActivitySearchService, ActivitySearchService>();
            unity.RegisterType<IActivityService, ActivityService>();
            unity.RegisterType<GarminConnectClient.IActivityService, GarminConnectClient.ActivityService>();

            ISessionService sessionService = unity.Resolve<ISessionService>();
            try
            {
                sessionService.SignOut();
            }
            catch (Exception ex)
            {
            }
            sessionService.SignIn(opts.UserName, opts.Password);
            IActivityService activityService = unity.Resolve<IActivityService>();
            activityService.SyncLatestMetadata();
            activityService.SyncFiles();
            activityService.CleanFiles();
            sessionService.SignOut();
        }
    }
}
