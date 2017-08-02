using System;
using HockeyApp.Utils;
using Newtonsoft.Json;

namespace HockeyApp.Models
{
    internal class HockeyAppResponse : IResponseObject
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "bundle_identifier")]
        public string BundleIdentifier { get; set; }

        [JsonProperty(PropertyName = "public_identifier")]
        public string PublicIdentifier { get; set; }

        [JsonProperty(PropertyName = "device_family")]
        public string DeviceFamily { get; set; }

        [JsonProperty(PropertyName = "minimum_os_version")]
        public string MinimumOsVersion { get; set; }

        [JsonProperty(PropertyName = "release_type")]
        public string ReleaseType { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "config_url")]
        public string ConfigUrl { get; set; }

        [JsonProperty(PropertyName = "public_url")]
        public string PublicUrl { get; set; }

        public void ToConsole()
        {
            var builder = new TableBuilder();
            builder.AddTitleRow("Key", "Value");
            builder.AddRow("Title", Title);
            builder.AddRow("Bundle ID", BundleIdentifier);
            builder.AddRow("Public ID", PublicIdentifier);
            builder.AddRow("Device Family", DeviceFamily);
            builder.AddRow("Minimum OS Version", MinimumOsVersion);
            builder.AddRow("Release Type", ReleaseType);
            builder.AddRow("Platform", Platform);
            builder.AddRow("Status", Status);
            builder.AddRow("Config URL", ConfigUrl);
            builder.AddRow("Public URL", PublicUrl);
            builder.WriteToConsole(Console.Out);
        }
    }
}