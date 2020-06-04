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
        public List<ThreatLockerOrganization> threatLockerOrganizations { get; set; }
        public List<ThreatLockerOrganization> selectedManageCompanies { get; set; }
    }
}
