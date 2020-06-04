using ManageIntegration.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageIntegration
{
    public interface IAppDb
    {
        void Dispose();
        Task<Config> GetConfigAsync();
        Task<ManageConfig> GetManageConfigAsync();
        Task<List<ThreatLockerOrganization>> GetThreatLockerOrganizationsAsync();
        Task SaveManageCompanies(List<ManageCompany> manageCompanies);
        Task SaveManageCompany(ManageCompany manageCompany);
        Task SaveThreatLockerOrganization(ThreatLockerOrganization threatLockerOrganization);
        Task SaveThreatLockerOrganizations(List<ThreatLockerOrganization> threatLockerOrganizations);
        Task SaveConfigAsync(Config config);
        Task SaveManageConfigAsync(ManageConfig manageConfig);
    }
}