using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class ManageCompany
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public ManageCompanyStatus Status { get; set; }
        public List<ManageCompanyType> Types { get; set; }
    }

    public class ManageCompanyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ManageCompanyStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
