using ManageIntegration.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManageIntegration
{
    public class AppDb : IDisposable, IAppDb
    {
        public MySqlConnection Connection { get; }

        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();

        public async Task<Config> GetConfigAsync()
        {
            var config = new Config();
            await Connection.OpenAsync();

            using (var cmd = new MySqlCommand("SELECT ThreatLockerUrl, " +
                "ThreatLockerAuthorization, ManageURL, ManageAPIKeyPu, ManageAPIKeyPr, ManageCompanyName," +
                "ManageClientId, RequestCheckDelay, LastSuccessRequestSent FROM config", Connection))
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    config.ThreatLockerUrl = reader.GetString(0);
                    config.ThreatlockerAuth = reader.GetString(1);
                    config.ManageUrl = reader.GetString(2);
                    config.ManagePubKey = reader.GetString(3);
                    config.ManagePriKey = reader.GetString(4);
                    config.ManageCompanyName = reader.GetString(5);
                    config.ManageClientId = reader.GetString(6);
                    config.RequestCheckDelay = reader.GetInt32(7);
                    config.LastSuccessRequestSent = reader.GetDateTime(8);
                }
            Connection.Close();
            return config;
        }

        public async Task SaveConfigAsync(Config config)
        {
            await Connection.OpenAsync();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO config (Id, ThreatLockerUrl, ThreatLockerAuthorization, ManageURL, ManageAPIKeyPu, ManageAPIKeyPr, ManageCompanyName, ManageClientId, RequestCheckDelay)  VALUES (1, @ThreatLockerUrl, @ThreatLockerAuthorization, @ManageURL, @ManageAPIKeyPu, @ManageAPIKeyPr, @ManageCompanyName, @ManageClientId, @RequestCheckDelay)  ON DUPLICATE KEY UPDATE ThreatLockerURL = @ThreatLockerURL, ThreatLockerAuthorization = @ThreatLockerAuthorization, ManageURL = @ManageURL, ManageAPIKeyPu = @ManageAPIKeyPu, ManageAPIKeyPr = @ManageAPIKeyPr, ManageCompanyName = @ManageCompanyName, ManageClientId = @ManageClientId, RequestCheckDelay = @RequestCheckDelay;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ThreatLockerUrl",
                DbType = DbType.String,
                Value = config.ThreatLockerUrl
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ThreatLockerAuthorization",
                DbType = DbType.String,
                Value = config.ThreatlockerAuth
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageURL",
                DbType = DbType.String,
                Value = config.ManageUrl
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageAPIKeyPu",
                DbType = DbType.String,
                Value = config.ManagePubKey
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageAPIKeyPr",
                DbType = DbType.String,
                Value = config.ManagePriKey
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageCompanyName",
                DbType = DbType.String,
                Value = config.ManageCompanyName
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageClientId",
                DbType = DbType.String,
                Value = config.ManageClientId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@RequestCheckDelay",
                DbType = DbType.Int32,
                Value = config.RequestCheckDelay
            });
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task<ManageConfig> GetManageConfigAsync()
        {
            var manageConfig = new ManageConfig();
            await Connection.OpenAsync();

            using (var cmd = new MySqlCommand("SELECT boardId, typeId, subTypeId, itemId, priorityId, statusId, ticketSummary FROM manageconfig;", Connection))
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    manageConfig.BoardId = reader.GetInt32(0);
                    manageConfig.TypeId = reader.GetInt32(1);
                    manageConfig.SubTypeId = reader.GetInt32(2);
                    manageConfig.ItemId = reader.GetInt32(3);
                    manageConfig.PriorityId = reader.GetInt32(4);
                    manageConfig.StatusId = reader.GetInt32(5);
                    manageConfig.TicketSummary = reader.GetString(6);
                }
            Connection.Close();
            return manageConfig;
        }

        public async Task SaveManageConfigAsync(ManageConfig manageConfig)
        {
            await Connection.OpenAsync();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO manageconfig (Id, boardId, typeId, subTypeId, itemId, priorityId, statusId, ticketSummary) VALUES (1, @boardId, @typeId, @subTypeId, @itemId, @priorityId, @statusId, @ticketSummary) ON DUPLICATE KEY UPDATE boardId = @boardId, typeId = @typeId, subTypeId = @subTypeId, itemId = @itemId, priorityId = @priorityId, statusId = @statusId, ticketSummary = @ticketSummary;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@boardId",
                DbType = DbType.String,
                Value = manageConfig.BoardId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@typeId",
                DbType = DbType.String,
                Value = manageConfig.TypeId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@subTypeId",
                DbType = DbType.String,
                Value = manageConfig.SubTypeId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@itemId",
                DbType = DbType.String,
                Value = manageConfig.ItemId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@priorityId",
                DbType = DbType.String,
                Value = manageConfig.PriorityId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@statusId",
                DbType = DbType.String,
                Value = manageConfig.StatusId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ticketSummary",
                DbType = DbType.String,
                Value = manageConfig.TicketSummary
            });

            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task SaveManageCompany(ManageCompany manageCompany)
        {
            var manageConfig = new ManageConfig();

            await Connection.OpenAsync();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO managecompanies (Id, Name, Identifier)  VALUES (@Id, @Name, @Identifier) ON DUPLICATE KEY UPDATE Id = @Id, Name = @Name, Identifier = @Identifier;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = manageCompany.Id
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Name",
                DbType = DbType.String,
                Value = manageCompany.Name
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Identifier",
                DbType = DbType.String,
                Value = manageCompany.Identifier
            });
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task SaveManageCompanies(List<ManageCompany> manageCompanies)
        {
            foreach (var company in manageCompanies)
            {
                await SaveManageCompany(company);
            }
        }

        public async Task SaveThreatLockerOrganization(ThreatLockerOrganization threatLockerOrganization)
        {
            var manageConfig = new ManageConfig();

            await Connection.OpenAsync();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO threatlockerorganizations (OrganizationId, Name, ManageCompanyId)  VALUES (@OrganizationId, @Name, @ManageCompanyId) ON DUPLICATE KEY UPDATE OrganizationId = @OrganizationId, Name = @Name, ManageCompanyId = @ManageCompanyId;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OrganizationId",
                DbType = DbType.String,
                Value = threatLockerOrganization.OrganizationId
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Name",
                DbType = DbType.String,
                Value = threatLockerOrganization.Name
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageCompanyId",
                DbType = DbType.Int32,
                Value = threatLockerOrganization.ManageCompanyId
            });
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task<ThreatLockerOrganization> GetDefaultThreatLockerOrganization()
        {
            var threatLockerOrganization = new ThreatLockerOrganization();
            await Connection.OpenAsync();
            using (var cmd = new MySqlCommand("SELECT OrganizationId, Name, ManageCompanyId " +
                "FROM threatlockerorganizations WHERE OrganizationId = \"00000000-0000-0000-0000-000000000000\";", Connection))
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    threatLockerOrganization.OrganizationId = reader.GetString(0);
                    threatLockerOrganization.Name = reader.GetString(1);
                    threatLockerOrganization.ManageCompanyId = reader.GetInt32(2);
                };
            Connection.Close();
            return threatLockerOrganization;
        }

        public async Task SaveDefaultThreatLockerOrganization(ThreatLockerOrganization threatLockerOrganization)
        {
            var manageConfig = new ManageConfig();

            await Connection.OpenAsync();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"INSERT IGNORE INTO threatlockerorganizations (OrganizationId, Name, ManageCompanyId)  VALUES (@OrganizationId, @Name, @ManageCompanyId) ON DUPLICATE KEY UPDATE Name = @Name, ManageCompanyId = @ManageCompanyId;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@OrganizationId",
                DbType = DbType.String,
                Value = "00000000-0000-0000-0000-000000000000"
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Name",
                DbType = DbType.String,
                Value = "Catch-All Company"
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ManageCompanyId",
                DbType = DbType.Int32,
                Value = threatLockerOrganization.ManageCompanyId
            });
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task SaveThreatLockerOrganizations(List<ThreatLockerOrganization> threatLockerOrganizations)
        {
            foreach (var org in threatLockerOrganizations)
            {
                await Task.Run(() => SaveThreatLockerOrganization(org));
            }
        }

        public async Task<List<ThreatLockerOrganization>> GetThreatLockerOrganizationsAsync()
        {
            await Connection.OpenAsync();
            var threatLockerOrganizations = new List<ThreatLockerOrganization>();
            using (var cmd = new MySqlCommand("SELECT OrganizationId, Name, ManageCompanyId " +
                "FROM threatlockerorganizations", Connection))
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var threatLockerOrganization = new ThreatLockerOrganization()
                    {
                        OrganizationId = reader.GetString(0),
                        Name = reader.GetString(1),
                        ManageCompanyId = reader.GetInt32(2),
                    };
                    threatLockerOrganizations.Add(threatLockerOrganization);
                }
            Connection.Close();
            return threatLockerOrganizations;
        }

        public async Task UpdateLastSuccessSent(Config config)
        {
            await Connection.OpenAsync();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = @"UPDATE config SET LastSuccessRequestSent = @LastSuccessRequestSent WHERE id = 1";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LastSuccessRequestSent",
                DbType = DbType.DateTime,
                Value = config.LastSuccessRequestSent
            });
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

    }
}
