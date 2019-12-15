using System.Collections.Generic;
using Facebook;
using LGS.Models.Leads;
using LGS.Models.Webhooks;
using Newtonsoft.Json;

namespace LGS.Helpers.FacebookLeadGen
{
    public class FacebookClientHelper
    {
        public List<FacebookLead> GetPageInfo(string pageAccessToken, string pageId, string appId, string appSecret)
        {
            if (!string.IsNullOrEmpty(pageAccessToken) && !string.IsNullOrEmpty(pageId) &&
                !string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appSecret))
            {
                var fb = new FacebookClient
                {
                    AccessToken = pageAccessToken,
                    AppId = appId,
                    AppSecret = appSecret,
                };
                var pagePath = pageId + "?fields=leadgen_forms{leads}";
                var userPageObject = fb.Get(pagePath);
                var userFbLead = JsonConvert.DeserializeObject<FbLead>(userPageObject.ToString());
                if (userFbLead != null && userFbLead.leadgen_forms != null && userFbLead.leadgen_forms.data != null)
                {
                    var facebookLeadList = new List<FacebookLead>();
                    var facebookLeadDetailList = new List<FacebookLeadDetail>();

                    foreach (var data in userFbLead.leadgen_forms.data)
                    {
                        if (data.leads != null && data.leads.data != null)
                        {
                            foreach (var lead in data.leads.data)
                            {
                                if (lead != null && lead.field_data != null)
                                {
                                    foreach (var fieldData in lead.field_data)
                                    {
                                        string valueComplete = null;
                                        foreach (var value in fieldData.values)
                                        {
                                            valueComplete = valueComplete + " " + value;
                                        }

                                        var facebookLeadDetail = new FacebookLeadDetail
                                        {
                                            ColumnName = fieldData.name,
                                            ColumnValue = valueComplete,
                                            PageAccessToken = pageAccessToken,
                                            PageId = pageId,
                                            CreatedDateTime = lead.created_time
                                        };
                                        facebookLeadDetailList.Add(facebookLeadDetail);
                                    }

                                    var facebookLead = new FacebookLead
                                    {
                                        FacebookLeadDetails = facebookLeadDetailList,
                                        CreatedDateTime = lead.created_time,
                                        PageAccessToken = pageAccessToken,
                                        PageId = pageId
                                    };
                                    facebookLeadList.Add(facebookLead);
                                }
                            }
                        }
                    }

                    return facebookLeadList;
                }
            }
            return null;
        }

        public object GetUserInfo(string userAccessToken, string pageId, string appId, string appSecret)
        {
            if (!string.IsNullOrEmpty(userAccessToken) && !string.IsNullOrEmpty(pageId) &&
                !string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appSecret))
            {
                var fb = new FacebookClient
                {
                    AccessToken = userAccessToken,
                    AppId = appId,
                    AppSecret = appSecret,
                };
                var userAccountObject = fb.Get("me/accounts");
                return userAccountObject;
            }

            return null;
        }
    }
}