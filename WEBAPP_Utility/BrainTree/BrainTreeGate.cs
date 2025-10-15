using Braintree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMVC_Utility;

namespace WebApp_Utility.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {
        IBraintreeGateway braintreeGateway;
        private BrainTreeSettings settings {get; set;}

        public BrainTreeGate(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            settings = configuration.GetSection("BrainTree").Get<BrainTreeSettings>();
            
        }

        public IBraintreeGateway CreateGateway()
        {
            braintreeGateway = new BraintreeGateway(settings.Environment, settings.MerchantId, settings.PublicKey, settings.PrivateKey);

            return braintreeGateway;
        }

        public IBraintreeGateway GetGateway()
        {
            if (braintreeGateway == null)
                braintreeGateway = CreateGateway();

            return braintreeGateway;
        }
    }
}
