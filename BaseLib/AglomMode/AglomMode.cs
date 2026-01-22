using BaseLib.AglomMode.Models;
using BaseLib.SlagMode.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseLib.AglomMode
{
    public class AglomMode : IMathLibrary<AglomRequestData, AglomResponseData>
    {
        private readonly RestClient _restClient;
        private readonly string _serverAddress;

        public AglomMode(IOptions<ExternalServerDomain> serverAddress)
        {
            _restClient = new RestClient();
            _serverAddress = serverAddress.Value.AglomDomain;
        }

        public AglomResponseData Calculate(AglomRequestData request)
        {
            var restRequest = new RestRequest
            {
                Resource = ($"http://{_serverAddress}/api/SostavOfAglom/Calculate"),
                RequestFormat = DataFormat.Json,
                Method = Method.Post
            };

            restRequest.AddJsonBody(request);

            var res = _restClient.Execute<AglomResponseData>(restRequest);

            if (res is { IsSuccessful: true, Data: not null })
                return res.Data;
            return new AglomResponseData();
        }


    }
}
