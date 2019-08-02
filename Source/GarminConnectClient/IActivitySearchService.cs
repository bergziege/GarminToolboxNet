using System.Collections.Generic;
using GarminConnectClient.Data;

namespace GarminConnectClient
{
    public interface IActivitySearchService
    {
        List<Activity> FindActivities(int start, int limit);
    }
}