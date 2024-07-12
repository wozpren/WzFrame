using Masuit.Tools.Core.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Shared.ApiResult;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class EShopServics
    {

        private readonly string UserId;
        private readonly string UserKey;
        private readonly HttpClient client;


        public EShopServics(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            //set from Configration
            UserId = configuration["Eshop:UserId"] ?? throw new ArgumentNullException();
            UserKey = configuration["Eshop:UserKey"] ?? throw new ArgumentNullException();
            client = httpClientFactory.CreateClient("eshop");
        }


        public async Task<EshopResult> GetAllGoods(int page)
        {
            var param = new Dictionary<string, string>
            {
                { "userid", UserId },
                { "limit", "10" },
                { "page", page.ToString() }
            };

            param.Add("sign", GetKkySign(param, UserKey));
            var content = new FormUrlEncodedContent(param);
            var response = await client.PostAsync("/dockapi/v2/getallgoodsgroup", content);
            return await response.Content.ReadFromJsonAsync<EshopResult>();

            //application/x-www-form-urlencoded


            //var response = await client.PostAsync("/dockapi/v2/getallgoodsgroup", param);
        }


        public string GetKkySign(Dictionary<string, string> param, string userkey)
        {
            var sortedParam = param.OrderBy(p => p.Key).ToList();
            string signtext = "";

            foreach (var item in sortedParam)
            {
                if (string.IsNullOrEmpty(item.Value) || item.Key == "sign")
                    continue;

                if (!string.IsNullOrEmpty(signtext))
                    signtext += "&";

                signtext += $"{item.Key}={item.Value}";
            }

            signtext += userkey;

            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(signtext));
                var strResult = BitConverter.ToString(result);
                var newSign = strResult.Replace("-", "");
                return newSign.ToLower();
            }
        }
    }
}
