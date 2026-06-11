
using TrainineeAPI.DTOs;

public static class UtilityFunctions
{
    public static bool CheckHasFilterQuery(FilterTraineeDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search) || filter.Status.HasValue || filter.PageNumber.HasValue || filter.PageSize.HasValue)
        {
            return true;
        }

        return false;
    }
}