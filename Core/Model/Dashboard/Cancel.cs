using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Dashboard
{
    public class Cancel
    {
        public string ProcessCode { get; set; }
        public Guid ProcItemId { get; set; }
        public Guid UserId { get; set; }
    }
}
