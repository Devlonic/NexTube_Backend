using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NexTube.Application.Common.Interfaces;
using NexTube.Persistence.Settings.Configurations;
using System.Net;
using System.Text.Json.Serialization;
using IHttpClientFactory = NexTube.Application.Common.Interfaces.IHttpClientFactory;

namespace NexTube.Persistence.Services {
    public class ReCaptchaResponce {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public IEnumerable<string>? ErrorCodes { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }
    public class ReCaptchaValidatorService : ICaptchaValidatorService {
        private readonly IHttpClientFactory httpFactory;
        private readonly JsonSerializer jsonSerializer;
        private readonly ReCaptchaSettings options;

        public ReCaptchaValidatorService(IHttpClientFactory httpFactory, IOptions<ReCaptchaSettings> options, JsonSerializer jsonSerializer) {
            this.httpFactory = httpFactory;
            this.jsonSerializer = jsonSerializer;
            this.options = options.Value;
        }
        public async Task<bool> IsCaptchaPassedAsync(string token) {
            var responce = await GetCaptchaResultAsync(token);
            return responce?.Success ?? false;
        }
        private async Task<ReCaptchaResponce?> GetCaptchaResultAsync(string token) {
            using var http = httpFactory.CreateClient();
            var uri = string.Format(options.RemoteAddress, options.PrivateKey_V2, token);
            var responce = await http.GetAsync(uri);
            if ( responce.StatusCode != HttpStatusCode.OK )
                throw new HttpRequestException(responce.ReasonPhrase);

            var captcha_responce_string = await responce.Content.ReadAsStringAsync();
            var captcha_responce = await Task.Run(() => {
                return jsonSerializer.Deserialize(new StringReader(captcha_responce_string), typeof(ReCaptchaResponce)) as ReCaptchaResponce;
            });
            return captcha_responce;
        }
    }
}
