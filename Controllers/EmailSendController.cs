using Amazon;
using Amazon.Pinpoint;
using Amazon.Pinpoint.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebhookCustomChannel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSendController : ControllerBase
    {
        private static readonly string region = "us-east-1"; //Closest Supported Region to you
        private static readonly string appId = "44728463236047608fcaffb8868497db"; //Pinpoint application unique id.
        string charset = "UTF-8";


        [HttpPost]
        public string SendEmail(string EventData)
        {
            Console.WriteLine(EventData);

            var awsCredentials = new StoredProfileAWSCredentials("cxloyalty-dev");
            using (var client = new AmazonPinpointClient(awsCredentials, RegionEndpoint.GetBySystemName(region)))
            {
                SendMessagesRequest sendRequest = new SendMessagesRequest
                {
                    ApplicationId = appId,
                    MessageRequest = new MessageRequest
                    {
                        Addresses = new Dictionary<string, AddressConfiguration>
                        {
                            {
                                "sneha.dange@tavisca.com",
                                new AddressConfiguration
                                {
                                    ChannelType = "EMAIL"
                                }
                            }
                        },
                        MessageConfiguration = new DirectMessageConfiguration
                        {
                            EmailMessage = new EmailMessage
                            {
                                FromAddress = "sneha.dange@tavisca.com",
                                SimpleEmail = new SimpleEmail
                                {
                                    HtmlPart = new SimpleEmailPart
                                    {
                                        Charset = charset,
                                        Data = "<html><body><h3>Hello From Webhook custom channel</h1></body></html>"
                                    },
                                    Subject = new SimpleEmailPart
                                    {
                                        Charset = charset,
                                        Data = "Webhook Custom Channel"
                                    }
                                }
                            }
                        }
                    }
                };

                try
                {
                    Console.WriteLine("Sending message...");
                    Task<SendMessagesResponse> response = client.SendMessagesAsync(sendRequest);
                    Console.WriteLine(response + " Message sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The message wasn't sent. Error message: " + ex.Message);
                }
                return "hello";
            }
        }
    }
}
