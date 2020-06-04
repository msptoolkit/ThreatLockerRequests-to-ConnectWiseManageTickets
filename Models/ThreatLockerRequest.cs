using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class ThreatLockerRequest
    {
        public string ApprovalRequestId { get; set; }
        public string Json { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime Date { get; set; }
    }

    public class ThreatLockerAction
    {
        public string FullPath { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public List<ThreatLockerCert> Certs { get; set; }
        public string SerialNumber = "";
        public string DeviceType = "";
        public string ActionType = "execute";
        public string ApprovalLink { get; set; }
    }

    public class ThreatLockerCert
    {
        public string Sha { get; set; }
        public string Subject { get; set; }
    }
}
