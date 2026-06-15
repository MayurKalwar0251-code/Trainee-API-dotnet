
using TrainineeAPI.DTOs;

public static class UtilityFunctions
{
    public static bool CheckHasFilterQuery(FilterTraineeDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search) || !string.IsNullOrEmpty(filter.Status) || filter.PageNumber.HasValue || filter.PageSize.HasValue)
        {
            return true;
        }

        return false;
    }
}