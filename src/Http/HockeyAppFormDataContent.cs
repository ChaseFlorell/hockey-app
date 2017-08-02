using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using HockeyApp.Utils;
using Resx = HockeyApp.Properties.Resources;

namespace HockeyApp.Http
{
    public class HockeyAppFormDataContent: MultipartFormDataContent
    {
        public HockeyAppFormDataContent(string boundary):base(boundary)
        {
            Headers.Remove("Content-Type");
            Headers.TryAddWithoutValidation("Content-Type",$"multipart/form-data; boundary={boundary}");
        }

        public bool SafeAddValue(string key, Dictionary<string, string> parameters, TableBuilder table)
        {
            if (parameters.TryGetValue(key, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    table.AddRow(key, $"[{Resx.Common_Empty}]");
                    return false;
                }
                table.AddRow(key, value);

                // if we're adding "value" data, there's a chance it lives 
                // inside a document. Read the document and get the value data.
                try
                {
                    if (File.Exists(value))
                    {
                        value = File.ReadAllText(value);
                    }
                }
                catch { /*File Read Failed, Just use the old value*/ }

                var strContent = new StringContent(value);
                Add(strContent, $"\"{key}\"");
                return true;
            }
            return false;
        }

        public bool SafeAddFile(string key, Dictionary<string, string> parameters, TableBuilder table)
        {
            if (parameters.TryGetValue(key, out string value))
            {
                // value is empty
                if (string.IsNullOrWhiteSpace(value))
                {
                    table.AddRow(key, $"[{Resx.Common_Empty}]");
                    return false;
                }

                // file exists
                if (File.Exists(value))
                {
                    table.AddRow(key, value);
                    var fileStream = new FileStream(value, FileMode.Open);
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = $"\"{key}\"",
                        FileName = $"\"{value}\""
                    };
                    Add(streamContent, key, value);
                    return true;
                }
            }
            return false;
        }
    }
}