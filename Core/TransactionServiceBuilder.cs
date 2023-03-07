using Microsoft.Extensions.Configuration;
using System.ServiceModel;
using System.Text;
using TransactionService;

namespace Core
{
    public class TransactionServiceBuilder
    {
        private IConfiguration Configuration { get; set; }
        private TransactionService.TransactionServiceClient Api;
        const string XML_C_ReassignSrv_GK = "<C_ReassignProcess_GK xmlns=\"http://elite.com/schemas/transaction/process/write/C_ReassignProcess_GK\">";
        const string XML_C_ReassignSrv_GK_END = "</C_ReassignProcess_GK>";
        const string XML_C_InializeReassignSrv_GK = "<Initialize xmlns=\"http://elite.com/schemas/transaction/object/write/C_ReassignProcess_GK\">";
        const string XML_C_InializeReassignSrv_GK_END = "</Initialize>";
        const string XML_C_Add_GK = "<Add>";
        const string XML_C_Add_GK_END = "</Add>";
        const string XML_C_ReassignProcess_GK = "<C_ReassignProcess_GK>";
        const string XML_C_ReassignProcess_GK_END = "</C_ReassignProcess_GK>";
        const string XML_C_Attributes_GK = "<Attributes>";
        const string XML_C_Attributes_GK_END = "</Attributes>";
        const string XML_C_ProcessCode_GK = "<ProcessCode>";
        const string XML_C_ProcessCode_GK_END = "</ProcessCode>";
        const string XML_C_ProcItemId_GK = "<ProcItemId>";
        const string XML_C_ProcItemId_GK_END = "</ProcItemId>";
        const string XML_C_ReassignToUserId_GK = "<ReassignToUserId>";
        const string XML_C_ReassignToUserId_GK_END = "</ReassignToUserId>";

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
            var xml = BuildReassignXML(processCode, procItemId, reassignToUserId);
            var result = await Api.ExecuteProcessAsync(xml, 1);
            return result.Body.ToString();
        }
        private static string BuildReassignXML(string processCode, Guid procItemId, Guid reassignToUserId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(XML_C_ReassignSrv_GK);
            sb.Append(XML_C_InializeReassignSrv_GK);
            sb.Append(XML_C_Add_GK);
            sb.Append(XML_C_ReassignProcess_GK);
            sb.Append(XML_C_Attributes_GK);

            sb.Append(XML_C_ProcessCode_GK);
            sb.Append(processCode);
            sb.Append(XML_C_ProcessCode_GK_END);

            sb.Append(XML_C_ProcItemId_GK);
            sb.Append(procItemId);
            sb.Append(XML_C_ProcItemId_GK_END);

            sb.Append(XML_C_ReassignToUserId_GK);
            sb.Append(reassignToUserId);
            sb.Append(XML_C_ReassignToUserId_GK_END);

            sb.Append(XML_C_Attributes_GK_END);
            sb.Append(XML_C_ReassignProcess_GK_END);
            sb.Append(XML_C_Add_GK_END);
            sb.Append(XML_C_InializeReassignSrv_GK_END);
            sb.Append(XML_C_ReassignSrv_GK_END);

            return sb.ToString();
        }
    }
}
