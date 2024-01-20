using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RoleBasedMenuSystem.Core.Utilities.Response;

public class DataResult
{
    [JsonProperty("isError")] public bool IsError => ErrorMessageList?.Count > 0;
    [JsonProperty("errorMessageList")] public List<string> ErrorMessageList { get; set; } = new();
    [JsonProperty("total")] public int Total { get; set; }
    [JsonProperty("data")] public object Data { get; set; }
}

public static class DataResultHelper
{
    public static IActionResult HttpResponse(this DataResult dataResult)
    {
        HttpStatusCode statusCode = HttpStatusCode.OK;

        return new ObjectResult(dataResult)
        {
            StatusCode = (int?)statusCode
        };
    }
}