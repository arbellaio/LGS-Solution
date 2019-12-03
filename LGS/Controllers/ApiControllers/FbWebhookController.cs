using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using LGS.Models.FbWebhooks;
using Newtonsoft.Json;

namespace LGS.Controllers.ApiControllers
{
    public class FbWebhookController : ApiController
    {
//        [HttpGet]
//        public string Get(int id)
//        {
//            return "value";
//        }

        #region Get Request
        [HttpGet]
        public HttpResponseMessage Get()
        {
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(HttpContext.Current.Request.QueryString["hub.challenge"])
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
//                const string token = "EAAnZBWKZBAav0BALeBujBZA2TTrkBKIhWiPym9LoZBbHgdo3KlFRZAZC2q2oKDH2Tfz8L0c0yVGSixsadaQSpTUWIUAHWEM1BNZA2hVNBuxfY5GzU9y97MZCEjprmBO4Jf4XZAFc2PqmEcxiNZAJk1P0ZAAScfz79S2uZAzxXIXY9NzJtk3sGEWTfZBwx";
                const string token = "EAANiZATdB9WsBAB8ZAWiHCiH1gZC7BpULtkFTMT9yrDOa4wZAuiMsECBOLfLAK9CC3xPdHz3BKWNLjaumOjZBda3jo51J5ZAP9cGOp4l0lmfMMtcmNZCyxd6iW1tCSayvuvJZBauDafvmzWMw96jF8GITFa2fj9RwooEjCFnqAUilh4nbrP7XqpTm08bCNJ4e0b4x5WrJCfZBZBGA8IW6RpCRC";
                const string atoken = "EAAnZBWKZBAav0BAPFsmBBkZBgIvFjT2qepZCeGpxrZCe2ZAbaKDIZCHC6esW3ZADZBW2DdzgJ2ETPjFhYXB9UtrFEvTkRnSSwlgniO9me0mdMPwMstczRAiCpArhFcRBArXf5Br9jZAmU9I2yHb9ZBBadDUwgzUJGiM9iCVXYZCmP4LroAso5QvpJ5vZABUyvNE5MeilfZCWl7vhr9qQZDZD";

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
