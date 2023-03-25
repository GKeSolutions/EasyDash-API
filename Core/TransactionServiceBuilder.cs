using Core.XmlBuilder;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;
using System.Text;

namespace Core
{
    public class TransactionServiceBuilder
    {
        private IConfiguration Configuration { get; set; }
        private TransactionService.TransactionServiceClient Api;


        public TransactionServiceBuilder(IConfiguration configuration)
        {
            Configuration = configuration;
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.OpenTimeout = binding.CloseTimeout =
            binding.SendTimeout = binding.ReceiveTimeout = TimeSpan.FromMinutes(1);
            binding.MaxReceivedMessageSize = 20971520;
            binding.MaxBufferPoolSize = binding.MaxBufferSize = 20971520;
            binding.ReaderQuotas.MaxArrayLength =
            binding.ReaderQuotas.MaxStringContentLength =
            binding.ReaderQuotas.MaxBytesPerRead =
            binding.ReaderQuotas.MaxNameTableCharCount = 2097152;
            binding.TextEncoding = Encoding.UTF8;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            binding.TransferMode = TransferMode.Buffered;
            binding.AllowCookies = false;
            Api = new TransactionService.TransactionServiceClient(binding, new System.ServiceModel.EndpointAddress(Configuration["InstanceConfiguration:TransactionServiceURL"]));
            Api.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            Api.ClientCredentials.Windows.ClientCredential.UserName = "EliteAdmDev";
            Api.ClientCredentials.Windows.ClientCredential.Password = "SnEvs7nd3C";
        }

        public async Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId)
        {
            var xml = ReassignXml.BuildReassignXML(processCode, procItemId, reassignToUserId);
            var result = await Api.ExecuteProcessAsync(xml, 1);
            return result.Body.ToString();
        }

        public async Task<string> CancelProcess(string processCode, Guid procItemId)
        {
            var xml = CancelProcessXml.BuildCancelProcessXML(processCode, procItemId);
            var result = await Api.ExecuteProcessAsync(xml, 1);
            return result.Body.ToString();
        }  
    }
}
