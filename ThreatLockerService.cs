using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManageIntegration;
using ManageIntegration.DAL;
using ManageIntegration.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TLManageService
{
    public class ThreatLockerService : BackgroundService
    {
        private readonly ILogger<ThreatLockerService> _logger;
        private readonly AppDb _appDb;

        public ThreatLockerService(ILogger<ThreatLockerService> logger, AppDb appDb)
        {
            _logger = logger;
            _appDb = appDb;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Config config = await _appDb.GetConfigAsync();


                if (string.IsNullOrEmpty(config.ThreatlockerAuth) || string.IsNullOrEmpty(config.ManagePubKey))
                {
                    return;
                }

                ManageConfig manageConfig = await _appDb.GetManageConfigAsync();
                ManageTicket manageTicket = new ManageTicket();
                manageTicket.Company = new ManageCompany() { Name = "", Id = 0 };
                List<ThreatLockerOrganization> threatLockerOrganizations = await _appDb.GetThreatLockerOrganizationsAsync();
                _logger.LogInformation($"Checking for requests.");
                List<ThreatLockerRequest> threatLockerRequests = ThreatLockerAccess.GetRequests(config);

                if (threatLockerRequests != null)
                {
                    _logger.LogInformation($"{threatLockerRequests.Count} requests found.");
                    foreach (var request in threatLockerRequests)
                    {

                        _logger.LogInformation($"Matching Companies");
                        foreach (var org in threatLockerOrganizations)
                        {

                            if (org.OrganizationId == request.OrganizationId)
                            {
                                manageTicket.Company = new ManageCompany { Id = org.ManageCompanyId };
                                _logger.LogInformation($"{manageTicket.Company.Name} matched {org.Name}");
                            }
                            
                        }
                        
                        if (manageTicket.Company.Id <= 0)
                        {
                            var defaultThreatLockerOrganization = await _appDb.GetDefaultThreatLockerOrganization();
                            manageTicket.Company.Id = defaultThreatLockerOrganization.ManageCompanyId;
                        }

                        var threatLockerAction = ThreatLockerAccess.ProcessJson(request);

                        string approvalLink = config.ThreatLockerUrl;
                        if (threatLockerAction.ActionType == "execute")
                        {
                            approvalLink += "/applicationcontrolapproval.aspx?popup=true&approvalrequestid=" + request.ApprovalRequestId;
                        }
                        else
                        {
                            approvalLink += "/storagecontrolapproval.aspx?popup=true&approvalrequestid=" + request.ApprovalRequestId;
                        }
                        threatLockerAction.ApprovalLink = approvalLink;

                        StringBuilder initialDescription = new StringBuilder($"{threatLockerAction.Username} has requested access to {threatLockerAction.FullPath}\n");
                        initialDescription.Append($"Organization: {request.OrganizationName}\n");
                        initialDescription.Append($"Hostname: {threatLockerAction.Username.Split('\\')[0]}\n");
                        initialDescription.Append($"Hash: {threatLockerAction.Hash}");
                        foreach (var cert in threatLockerAction.Certs)
                        {
                            initialDescription.Append($"Cert: {cert.Subject} SHA: {cert.Sha}\n");
                        }

                        StringBuilder initialInternalAnalysis = new StringBuilder($"{approvalLink}");

                        manageTicket.Summary = manageConfig.TicketSummary;
                        manageTicket.InitialDescription = initialDescription.ToString();
                        manageTicket.InitialInternalAnalysis = initialInternalAnalysis.ToString();
                        manageTicket.Board = new ManageBoard { Id = manageConfig.BoardId };
                        manageTicket.Type = new ManageBoardType { BoardTypeId = manageConfig.TypeId };
                        manageTicket.SubType = new ManageBoardSubType { BoardSubTypeId = manageConfig.SubTypeId };
                        manageTicket.Item = new ManageBoardItem { BoardItemId = manageConfig.ItemId };
                        manageTicket.Priority = new ManageBoardPriority { BoardPriorityId = manageConfig.PriorityId };
                        manageTicket.Status = new ManageBoardStatus { BoardStatusId = manageConfig.StatusId };

                        ManageAccess.PostTicket(config, manageTicket);
                        config.LastSuccessRequestSent = DateTime.UtcNow;
                        await _appDb.UpdateLastSuccessSent(config);

                        _logger.LogInformation($"Ticket Created");
                    }

                }
                await Task.Delay(config.RequestCheckDelay * 1000, stoppingToken);
            }
        }
    }
}
