using System.Collections.Generic;
using GarminConnectExporter.Domain;

namespace GarminConnectExporter.Services
{
    public interface IActivitySearchService
    {
        List<Activity> FindActivities(int start, int limit);
    }
}