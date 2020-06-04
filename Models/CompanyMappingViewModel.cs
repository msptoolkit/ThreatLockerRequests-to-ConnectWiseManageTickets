using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class CompanyMappingViewModel
    {
        public List<ThreatLockerOrganization> ThreatLockerOrganizations { get; set; }
        public List<ManageCompany> ManageCompanies { get; set; }
        public ThreatLockerOrganization DefaultOrganization { get; set; }
    }
}
