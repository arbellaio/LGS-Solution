using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LGS.AppProperties;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace LGS.Helpers.TwilioHelper
{
    public class EmailSmsHelper
    {
        public static async Task<Response> SendEmail(string fromEmail, string fromName, string toEmail,string toName, string subject, string htmlContent, string plainTextContent)
        {
            var client = new SendGridClient(AppKeys.SendGuardApiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail, toName);

            // plainTextContent = "WazzUp Chiknay"; // doesn't show up in Email

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public static async Task<string> SendSms(string messageContent, string phoneNumberTo)
        {
            try
            {
                const string accountSid = AppKeys.TwilioAccountSid;
                const string authToken = AppKeys.TwilioAuthToken;
                const string phoneNumberFrom = AppKeys.TwilioFromPhoneNumber;

                TwilioClient.Init(accountSid, authToken);

                var message = await MessageResource.CreateAsync(
                    body: messageContent,
                    from: new Twilio.Types.PhoneNumber(phoneNumberFrom),
                    to: new Twilio.Types.PhoneNumber(phoneNumberTo)
                );

                Console.WriteLine(message.Sid);
                return messageContent;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Twilio Error {e.Code} - {e.MoreInfo}");
                return $"Twilio Error {e.Message} - {e.Code}";
            }

        }
    }
}