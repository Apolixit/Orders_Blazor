﻿namespace Blazor_Orders.Repository
{
    public static class UrlServices
    {
        public const string ApiUrl = "https://localhost:7058";
        public const string ApiVersion = "v1";
        public const string Schedule = $"{ApiUrl}/api/{ApiVersion}/Calendrier";
    }

    public static class UrlNavigation
    {
        public const string Index = "/";
        public const string Scheduler = "/scheduler";
    }
}
