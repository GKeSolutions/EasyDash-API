using System.Text;

namespace Core.XmlBuilder
{
    public static class CancelProcessXml
    {
        const string XML_C_CancelProcessSrv_GK = "<C_CancelProcess_GK xmlns=\"http://elite.com/schemas/transaction/process/write/C_CancelProcess_GK\">";
        const string XML_C_CancelProcessSrv_GK_END = "</C_CancelProcess_GK>";
        const string XML_C_InializeCancelProcessSrv_GK = "<Initialize xmlns=\"http://elite.com/schemas/transaction/object/write/C_CancelProcess_GK\">";
        const string XML_C_InializeCancelProcessSrv_GK_END = "</Initialize>";
        const string XML_C_Add_GK = "<Add>";
        const string XML_C_Add_GK_END = "</Add>";
        const string XML_C_CancelProcessProcess_GK = "<C_CancelProcess_GK>";
        const string XML_C_CancelProcessProcess_GK_END = "</C_CancelProcess_GK>";
        const string XML_C_Attributes_GK = "<Attributes>";
        const string XML_C_Attributes_GK_END = "</Attributes>";
        const string XML_C_ProcessCode_GK = "<ProcessCode>";
        const string XML_C_ProcessCode_GK_END = "</ProcessCode>";
        const string XML_C_ProcItemId_GK = "<ProcItemId>";
        const string XML_C_ProcItemId_GK_END = "</ProcItemId>";

        public static string BuildCancelProcessXML(string processCode, Guid procItemId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(XML_C_CancelProcessSrv_GK);
            sb.Append(XML_C_InializeCancelProcessSrv_GK);
            sb.Append(XML_C_Add_GK);
            sb.Append(XML_C_CancelProcessProcess_GK);
            sb.Append(XML_C_Attributes_GK);

            sb.Append(XML_C_ProcessCode_GK);
            sb.Append(processCode);
            sb.Append(XML_C_ProcessCode_GK_END);

            sb.Append(XML_C_ProcItemId_GK);
            sb.Append(procItemId);
            sb.Append(XML_C_ProcItemId_GK_END);

            sb.Append(XML_C_Attributes_GK_END);
            sb.Append(XML_C_CancelProcessProcess_GK_END);
            sb.Append(XML_C_Add_GK_END);
            sb.Append(XML_C_InializeCancelProcessSrv_GK_END);
            sb.Append(XML_C_CancelProcessSrv_GK_END);

            return sb.ToString();
        }
    }
}
