using LGS.Models.Webhooks;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LGS.Controllers.ApiControllers
{
    public class FbWebhookController : ApiController
    {
        #region Get Request
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var token = HttpContext.Current.Request.QueryString["hub.verify_token"];
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(HttpContext.Current.Request.QueryString["hub.challenge"]),
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return response;
        }
        #endregion Get Request


        

        #region Post Request

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] JsonData data)
        {
            try
            {
                var entry = data.Entry.FirstOrDefault();
                var change = entry?.Changes.FirstOrDefault();
                if (change == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);

                //Generate user access token here https://developers.facebook.com/tools/accesstoken/
                const string token = "EAANiZATdB9WsBALYlx5BOdLhfGFxD9SndVH3MHDXidAth5zaY8WZCCXDDmWz6fTathf4hP634ZApBHTPuB8ZBifmNSVSZARHjZAz97Ipv9tXeC1OFc559iJ7512X2ernHSAW8KvD5gDCffpZCR6sTOpbaNbPPKdhaPmXSg63CbTUSzeqFZCDZBeF7lha3ptZBUA2MZD";

                var leadUrl = $"https://graph.facebook.com/v5.0/{change.Value.LeadGenId}?access_token={token}";
                var formUrl = $"https://graph.facebook.com/v5.0/{change.Value.FormId}?access_token={token}";

                using (var httpClientLead = new HttpClient())
                {
                    var response = await httpClientLead.GetStringAsync(formUrl);
                    if (!string.IsNullOrEmpty(response))
                    {
                        var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(response);
                        //jsonObjLead.Name contains the lead ad name

                        //If response is valid get the field data
                        using (var httpClientFields = new HttpClient())
                        {
                            var responseFields = await httpClientFields.GetStringAsync(leadUrl);
                            if (!string.IsNullOrEmpty(responseFields))
                            {
                                var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);
                                //jsonObjFields.FieldData contains the field value
                            }
                        }
                    }
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error-->{ex.Message}");
                Trace.WriteLine($"StackTrace-->{ex.StackTrace}");
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
        }

        #endregion Post Request



    }
}
