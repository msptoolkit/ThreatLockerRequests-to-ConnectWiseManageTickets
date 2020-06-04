using ManageIntegration;
using ManageIntegration.DAL;
using ManageIntegration.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ThreatLockerService : IHostedService, IDisposable
{
    private readonly ILogger<ThreatLockerService> _logger;
    private Timer _timer;
    private readonly AppDb _appDb;

    public ThreatLockerService(ILogger<ThreatLockerService> logger, AppDb appDb)
    {
        _logger = logger;
        _appDb = appDb;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");
        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        Config config = await _appDb.GetConfigAsync();

        if (string.IsNullOrEmpty(config.ThreatlockerAuth) || string.IsNullOrEmpty(config.ManagePubKey))
        {
            return;
        }

        ManageConfig manageConfig = await _appDb.GetManageConfigAsync();
        ManageTicket manageTicket = new ManageTicket();
        List<ThreatLockerOrganization> threatLockerOrganizations = await _appDb.GetThreatLockerOrganizationsAsync();
        List<ThreatLockerRequest> threatLockerRequests = ThreatLockerAccess.GetRequests(config);

        foreach(var request in threatLockerRequests)
        {
            foreach(var org in threatLockerOrganizations)
            {
                if(org.OrganizationId == request.OrganizationId)
                {
                    manageTicket.Company = new ManageCompany { Id = org.ManageCompanyId };
                }
                //default company
            }

            var threatLockerAction = ThreatLockerAccess.ProcessJson(request);

            string approvalLink = config.ThreatLockerUrl;
            if(threatLockerAction.ActionType == "execute")
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
            foreach(var cert in threatLockerAction.Certs)
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
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);


        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}