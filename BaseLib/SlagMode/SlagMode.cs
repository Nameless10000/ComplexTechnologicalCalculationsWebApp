using BaseLib.SlagMode.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BaseLib.SlagMode
{
    public class SlagMode : IMathLibrary<RequestData, ResponseData>
    {
        public readonly RestClient _restClient;
        public readonly string _serverAddress;

        public SlagMode(IOptions<ExternalServerDomain> serverAddress)
        {
            _restClient = new RestClient();
            _serverAddress = serverAddress.Value.Domain;
        }

        public ResponseData Calulate(RequestData request)
        {
            var auth = new RestSharp
                .Authenticators
                .JwtAuthenticator(GetTokenFromServer(request.User));

            var restRequest = new RestRequest
            {
                Resource = ($"{_serverAddress}/Calculate"),
                RequestFormat = DataFormat.Json,
                Authenticator = auth,
                Method = Method.Post
            };

            restRequest.AddJsonBody(request);

            var res = _restClient.Execute<ResponseData>(restRequest);

            if (res is { IsSuccessful: true, Data: not null })
                return res.Data;
            return new ResponseData();
        }

        private string GetTokenFromServer(UserAuthData user)
        {
            var restRequest = new RestRequest
            {
                Resource = ($"{_serverAddress}/Login"),
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
