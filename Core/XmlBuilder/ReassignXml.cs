﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.XmlBuilder
{
    public static class ReassignXml
    {
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

        public static string BuildReassignXML(string processCode, Guid procItemId, Guid reassignToUserId)
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
