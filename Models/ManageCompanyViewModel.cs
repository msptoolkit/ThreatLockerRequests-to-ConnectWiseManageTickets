using ManageIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLManageService.Models
{
    public class ManageCompanyViewModel
    {
        public List<ManageCompany> ManageCompanies { get; set; }
        public List<int> ManageCompanyStatusIds { get; set; }
        public List<int> ManageCompanyTypeIds { get; set; }
        public List<ManageCompanyStatus> ManageCompanyStatuses { get; set; }
        public List<ManageCompanyType> ManageCompanyTypes { get; set; }
    }
}
