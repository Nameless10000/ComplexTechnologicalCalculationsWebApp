using System.Reflection.Metadata;
using System.Text.Json;
using BaseLib.SlagMode.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace BaseLib.SlagMode
{
    public class SlagMode : IMathLibrary<RequestData, ResponseData>
    {
        private readonly RestClient _restClient;
        private readonly string _serverAddress;

        public SlagMode(IOptions<ExternalServerDomain> serverAddress)
        {
            _restClient = new RestClient();
            _serverAddress = serverAddress.Value.Domain;
        }

        public ResponseData Calculate(RequestData request)
        {
            string rawResponse = GetTokenFromServer(request.User);
            
            string? jwtToken = null;
            if (!string.IsNullOrWhiteSpace(rawResponse))
            {
                try
                {
                    using var doc = JsonDocument.Parse(rawResponse);
                    jwtToken = doc.RootElement
                        .GetProperty("token")
                        .GetString();
                }
                catch
                {
                    // ignored
                }
            }

            if (string.IsNullOrEmpty(jwtToken))
                throw new InvalidOperationException("Не удалось получить JWT-токен от сервера.");
            
            var authenticator = new 
                JwtAuthenticator(jwtToken);

            var restRequest = new RestRequest
            {
                Resource = ($"https://{_serverAddress}/api/SlagMode/Calculate"),
                RequestFormat = DataFormat.Json,
                Authenticator = authenticator,
                Method = Method.Post
            };

            restRequest.AddJsonBody(request);

            var res = _restClient.Execute<ResponseData>(restRequest);

            if (res is { IsSuccessful: true, Data: not null })
                return res.Data;
            return new ResponseData();
        }

        public string GetTokenFromServer(UserAuthData user)
        {
            var restRequest = new RestRequest
            {
                Resource = ($"https://{_serverAddress}/api/SlagMode/Login"),
                RequestFormat = DataFormat.Json,
                Method = Method.Post
            };

            restRequest.AddJsonBody(user);

            var res = _restClient.Execute(restRequest);
            if (res is { Content: { Length: > 0 }, IsSuccessful: true })
                return res.Content;
            return null;
        }
    }
}
