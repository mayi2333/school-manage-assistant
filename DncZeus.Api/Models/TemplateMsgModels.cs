using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncZeus.Api.Models
{
    public class TemplateMsgAttendModel
    {
        public TemplateMsgAttendModel()
        { 
        }
        public TemplateMsgAttendModel(string fullName, string className, string dateTime, string deduct, string surplus)
        {
            FullName = new MsgData() { value = fullName, color = "#173177" };
            ClassName = new MsgData() { value = className, color = "#173177" };
            DateTime = new MsgData() { value = dateTime, color = "#173177" };
            Deduct = new MsgData() { value = deduct, color = "#173177" };
            Surplus = new MsgData() { value = surplus, color = "#173177" };
        }
        public MsgData FullName { get; set; }
        public MsgData ClassName { get; set; }
        public MsgData DateTime { get; set; }
        public MsgData Deduct { get; set; }
        public MsgData Surplus { get; set; }
    }

    public class MsgData
    {
        public string value { get; set; }
        public string color { get; set; }
    }
}
