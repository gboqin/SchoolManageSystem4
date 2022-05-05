using SchoolManageSystem.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolManageSystem.Entities
{
    /// <summary>
    /// 日志表
    /// </summary>
    [Table("Sys_Logrecord")]
    [Description("日志表")]
    public class SysLogrecord : IEntity
    {
        public int Id { get; set; }
        public string LogDate { get; set; }
        public string LogLevel { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string MachineName { get; set; }
        public string MachineIp { get; set; }
        public string NetRequestMethod { get; set; }
        public string NetRequestUrl { get; set; }
        public string NetUserIsauthenticated { get; set; }
        public string NetUserAuthtype { get; set; }
        public string NetUserIdentity { get; set; }
    }
}
