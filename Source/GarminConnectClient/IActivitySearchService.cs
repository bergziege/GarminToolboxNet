using System.Collections.Generic;
using GarminConnectClient.Data;

namespace GarminConnectClient
{
    public interface IActivitySearchService
    {
        ActivitySearchResultsContainer FindActivities();
        ActivitySearchResultsContainer FindActivities(ActivitySearchFilters filters);
        List<Activity> FindAllActivities(out IList<string> errors);
        List<Activity> FindAllActivities(ActivitySearchFilters filters, out IList<string> errors);
    }
}