using System.Data;
using ActivityArchive.Config;
using ActivityArchive.Persistence.Mappings;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using MySql.Data.MySqlClient;
using Unity;

namespace ActivityArchive.Infrastructure
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