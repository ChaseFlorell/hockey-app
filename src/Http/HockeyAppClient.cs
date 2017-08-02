using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HockeyApp.Models;
using HockeyApp.Utils;
using Newtonsoft.Json;

namespace HockeyApp.Http
{
    internal class HockeyAppClient : IDisposable
    {
        private const string _domain = "https://rink.hockeyapp.net/api/2/apps";
        private readonly HttpClient _httpClient;

        public HockeyAppClient()
        {
            // I can't remember why I set a long timeout here. Maybe the default was kicking me out early on long uploads???
            _httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        /// Upload an app to the 'https://rink.hockeyapp.net/api/2/apps/app_versions/upload' endpoint
        /// </summary>
        /// <param name="token">HockeyApp Token</param>
        /// <param name="appId">HockeyApp App ID</param>
        /// <param name="parameters">Dictionary of parameters used for required and optional variables</param>
        /// <returns><see cref="HockeyAppResponse"/></returns>
        public Task<HockeyAppResponse> UploadAsync(string token, string appId, Dictionary<string, string> parameters)
        {
            Validate(token, parameters,
                customValidation: () =>
                {
                    // ReSharper disable once NotResolvedInText
                    if (appId == null) throw new ArgumentNullException("-appId");
                    // ReSharper disable once NotResolvedInText
                    if (!parameters.ContainsKey("ipa")) throw new ArgumentNullException("-ipa");
                });


            var boundary = Guid.NewGuid().ToString().Replace("-", "");
            var data = new HockeyAppFormDataContent(boundary);
            var tableBuilder = new TableBuilder();
            tableBuilder.AddTitleRow("parameter", "value");
            data.SafeAddFile("ipa", parameters, tableBuilder);
            data.SafeAddFile("dsym", parameters, tableBuilder);
            data.SafeAddValue("notes_type", parameters, tableBuilder);
            data.SafeAddValue("notify", parameters, tableBuilder);
            data.SafeAddValue("status", parameters, tableBuilder);
            data.SafeAddValue("strategy", parameters, tableBuilder);
            data.SafeAddValue("tags", parameters, tableBuilder);
            data.SafeAddValue("teams", parameters, tableBuilder);
            data.SafeAddValue("users", parameters, tableBuilder);
            data.SafeAddValue("mandatory", parameters, tableBuilder);
            data.SafeAddValue("release_type", parameters, tableBuilder);
            data.SafeAddValue("notes", parameters, tableBuilder);

            tableBuilder.WriteToConsole(Console.Out);

            var endpoint = new Uri($"{_domain}/{appId}/app_versions/upload");
            return RequestAsync<HockeyAppResponse>(token, endpoint, data, HttpMethod.Post);
        }

        private async Task<T> RequestAsync<T>(string token, Uri endpoint, HockeyAppFormDataContent data, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = endpoint,
                Method = httpMethod,
                Content = data,
                Headers = { { "X-HockeyAppToken", token } }
            };

            var progressToken = new CancellationTokenSource();
            HttpResponseMessage response = null;
            var progress = Progress.Dots(progressToken.Token);
            var send = Task.Run(async () =>
            {
                try
                {
                    response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);
                }
                finally
                {
                    progressToken.Cancel(false);
                }
            }, CancellationToken.None);

            await Task.WhenAll(send, progress);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        private void Validate(string token, Dictionary<string, string> parameters, Action customValidation = null)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            customValidation?.Invoke();
        }
    }
}