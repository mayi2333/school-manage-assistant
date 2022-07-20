using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.RequestPayload.系统管理.OperationLog
{
    public class SaveErrorLoggerPayload
    {
        public int Code { get; set; }
        public string Mes { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
