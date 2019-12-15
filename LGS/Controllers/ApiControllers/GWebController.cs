using LGS.Data;
using LGS.Data.Services.ClientServices;
using LGS.Models.Leads;
using LGS.Models.Webhooks;
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

namespace LGS.Controllers.ApiControllers
{
    public class GWebController : ApiController
    {
        #region Constructor Inits Service Inits

        private IClientService _clientService;
        private ApplicationUserManager _userManager;

        public GWebController()
        {
        }

        public GWebController(IClientService clientService)
        {
            Service = clientService ?? throw new ArgumentNullException("clientService");
        }

        public IClientService Service
        {
            get { return _clientService = new ClientService(new ApplicationDbContext()); }
            private set { _clientService = value; }
        }

        #endregion

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
        public async Task<HttpResponseMessage> Post(WebhookLead webLeadData)
        {
            try
            {
                if (ModelState.IsValid && webLeadData != null)
                {
                    var googleLeadDetailList = new List<GoogleLeadDetail>();
                    if (webLeadData.user_column_data != null && webLeadData.user_column_data.Any())
                    {
                        foreach (var webUserLeadColumnData in webLeadData.user_column_data)
                        {
                            var googleLeadDetail = new GoogleLeadDetail
                            {
                                FormId = webLeadData.form_id,
                                CampaignId = webLeadData.campaign_id,
                                GclId = webLeadData.gcl_id,
                                GoogleKey = webLeadData.Google_key,
                                LeadId = webLeadData.lead_id,
                                ColumnName = webUserLeadColumnData.column_name,
                                ColumnValue = webUserLeadColumnData.string_value,
                            };
                            googleLeadDetailList.Add(googleLeadDetail);
                        }
                    }

                    var googleLead = new GoogleLead
                    {
                        ApiVersion = webLeadData.api_version,
                        FormId = webLeadData.form_id,
                        CampaignId = webLeadData.campaign_id,
                        GclId = webLeadData.gcl_id,
                        GoogleKey = webLeadData.Google_key,
                        LeadId = webLeadData.lead_id,
                        GoogleLeadDetails = googleLeadDetailList,
                    };

                    await Service.SaveGoogleLeadData(googleLead,googleLeadDetailList);
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