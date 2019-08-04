using System.Data;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using GarminConnectExporter.Config;
using GarminConnectExporter.Persistence.Mapping;
using MySql.Data.MySqlClient;
using Unity;

namespace GarminConnectExporter.Infrastructure
{
    public class DBConfiguration
    {
        public DBConfiguration(IUnityContainer unity, DbSettings dbSettings)
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ActivityMetadataMap());
                config.ForDommel();
            });

            IDbConnection connection = new MySqlConnection($"Server={dbSettings.Server};Database={dbSettings.Database};Uid={dbSettings.Username};Pwd={dbSettings.Password};");
            unity.RegisterInstance(connection);
        }
    }
}