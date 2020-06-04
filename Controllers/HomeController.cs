using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManageIntegration.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ManageIntegration.DAL;
using RestSharp;

namespace ManageIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDb _appDb;

        public HomeController(ILogger<HomeController> logger, AppDb appDb)
        {
            _logger = logger;
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("config")]
        public async Task<IActionResult> Config()
        {
            Config config = await _appDb.GetConfigAsync();

            return View(config);
        }

        [HttpPost("config")]
        public async Task<IActionResult> Config(Config model)
        {
            await _appDb.SaveConfigAsync(model);

            return View();
        }

        [HttpGet("manageconfig")]
        public async Task<IActionResult> ManageConfig()
        {
            Config config = await _appDb.GetConfigAsync();
            ManageConfig manageConfig = await _appDb.GetManageConfigAsync();
            List<ManageBoard> manageBoards = ManageAccess.GetBoards(config);

            ViewBag.ListOfBoards = manageBoards;

            manageConfig.ManageBoard = ManageAccess.GetBoard(config, manageConfig.BoardId);
            manageConfig.ManageBoards = manageBoards;
            manageConfig.ManageBoardTypes = ManageAccess.GetBoardTypes(config, manageConfig.BoardId);
            manageConfig.ManageBoardType = ManageAccess.GetBoardType(config, manageConfig.BoardId, manageConfig.TypeId);
            manageConfig.ManageBoardSubTypes = ManageAccess.GetBoardSubTypes(config, manageConfig.BoardId);
            manageConfig.ManageBoardSubType = ManageAccess.GetBoardSubType(config, manageConfig.BoardId, manageConfig.SubTypeId);
            manageConfig.ManageBoardItems = ManageAccess.GetBoardItems(config, manageConfig.BoardId);
            manageConfig.ManageBoardItem = ManageAccess.GetBoardItem(config, manageConfig.BoardId, manageConfig.ItemId);
            manageConfig.ManageBoardPriorities = ManageAccess.GetBoardPriorities(config);
            manageConfig.ManageBoardPriority = ManageAccess.GetBoardPriority(config, manageConfig.PriorityId);
            manageConfig.ManageBoardStatuses = ManageAccess.GetBoardStatuses(config, manageConfig.BoardId);
            manageConfig.ManageBoardStatus = ManageAccess.GetBoardStatus(config, manageConfig.BoardId, manageConfig.StatusId);


            return View(manageConfig);
        }

        [HttpPost("manageconfig")]
        public async Task<IActionResult> ManageConfig(ManageConfig model)
        {
            Config config = await _appDb.GetConfigAsync();
            ManageConfig manageConfig = await _appDb.GetManageConfigAsync();
            List<ManageBoard> manageBoards = ManageAccess.GetBoards(config);

            int BoardId = int.Parse(HttpContext.Request.Form["BoardId"]);
            int TypeId = int.Parse(HttpContext.Request.Form["BoardTypeId"]);
            int SubTypeId = int.Parse(HttpContext.Request.Form["BoardSubTypeId"]);
            int ItemId = int.Parse(HttpContext.Request.Form["BoardItemId"]);
            int PriorityId = int.Parse(HttpContext.Request.Form["BoardPriorityId"]);
            int StatusId = int.Parse(HttpContext.Request.Form["BoardStatusId"]);
            string TicketSummary = (HttpContext.Request.Form["TicketSummary"]);

            model.BoardId = BoardId;
            model.TypeId = TypeId;
            model.SubTypeId = SubTypeId;
            model.ItemId = ItemId;
            model.PriorityId = PriorityId;
            model.StatusId = StatusId;
            model.TicketSummary = TicketSummary;
            model.ManageBoard = ManageAccess.GetBoard(config, manageConfig.BoardId);
            model.ManageBoards = manageBoards;
            model.ManageBoardTypes = ManageAccess.GetBoardTypes(config, manageConfig.BoardId);
            model.ManageBoardType = ManageAccess.GetBoardType(config, manageConfig.BoardId, manageConfig.TypeId);
            model.ManageBoardSubTypes = ManageAccess.GetBoardSubTypes(config, manageConfig.BoardId);
            model.ManageBoardSubType = ManageAccess.GetBoardSubType(config, manageConfig.BoardId, manageConfig.SubTypeId);
            model.ManageBoardItems = ManageAccess.GetBoardItems(config, manageConfig.BoardId);
            model.ManageBoardItem = ManageAccess.GetBoardItem(config, manageConfig.BoardId, manageConfig.ItemId);
            model.ManageBoardPriorities = ManageAccess.GetBoardPriorities(config);
            model.ManageBoardPriority = ManageAccess.GetBoardPriority(config, manageConfig.PriorityId);
            model.ManageBoardStatuses = ManageAccess.GetBoardStatuses(config, manageConfig.BoardId);
            model.ManageBoardStatus = ManageAccess.GetBoardStatus(config, manageConfig.BoardId, manageConfig.StatusId);

            List<ManageCompany> manageComapies = ManageAccess.GetCompanies(config);
            model.ManageCompanies = manageComapies;
            ViewBag.ListOfBoards = manageBoards;

            await _appDb.SaveManageConfigAsync(model);

            return View(model);
        }

        [HttpGet("companymappings")]
        public async Task<IActionResult> CompanyMappings()
        {
            CompanyMappingViewModel companyMappingViewModel = new CompanyMappingViewModel();

            Config config = await _appDb.GetConfigAsync();
     
            companyMappingViewModel.threatLockerOrganizations = await _appDb.GetThreatLockerOrganizationsAsync();
            if (companyMappingViewModel.threatLockerOrganizations == null)
            {
                companyMappingViewModel.threatLockerOrganizations = ThreatLockerAccess.GetOrganizations(config);
            }


                ViewBag.ManageCompanyList = ManageAccess.GetCompanies(config);

            return View(companyMappingViewModel);
        }

        [HttpPost("companymappings")]
        public async Task<IActionResult> CompanyMappings(CompanyMappingViewModel model)
        {
            Config config = await _appDb.GetConfigAsync();

            ViewBag.ManageCompanyList = ManageAccess.GetCompanies(config);

            await _appDb.SaveThreatLockerOrganizations(model.threatLockerOrganizations);

            return View(model);
        }

        public async Task<JsonResult> GetBoardTypesJson(int BoardId)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageBoardType> manageBoardTypes = new List<ManageBoardType>();
            manageBoardTypes = ManageAccess.GetBoardTypes(config, BoardId);

            manageBoardTypes.Insert(0, new ManageBoardType { BoardTypeId = 0, BoardTypeName = "Select" });

            return Json(new SelectList(manageBoardTypes, "BoardTypeId", "BoardTypeName"));
        }

        public async Task<JsonResult> GetBoardSubTypesJson(int BoardId)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageBoardSubType> manageBoardSubTypes = new List<ManageBoardSubType>();
            manageBoardSubTypes = ManageAccess.GetBoardSubTypes(config, BoardId);

            manageBoardSubTypes.Insert(0, new ManageBoardSubType { BoardSubTypeId = 0, BoardSubTypeName = "Select" });

            return Json(new SelectList(manageBoardSubTypes, "BoardSubTypeId", "BoardSubTypeName"));
        }

        public async Task<JsonResult> GetBoardItemsJson(int BoardId)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageBoardItem> manageBoardItems = new List<ManageBoardItem>();
            manageBoardItems = ManageAccess.GetBoardItems(config, BoardId);

            manageBoardItems.Insert(0, new ManageBoardItem { BoardItemId = 0, BoardItemName = "Select" });

            return Json(new SelectList(manageBoardItems, "BoardItemId", "BoardItemName"));
        }

        public async Task<JsonResult> GetBoardPrioritiesJson(int BoardId)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageBoardPriority> manageBoardPriorities = new List<ManageBoardPriority>();
            manageBoardPriorities = ManageAccess.GetBoardPriorities(config);

            manageBoardPriorities.Insert(0, new ManageBoardPriority { BoardPriorityId = 0, BoardPriorityName = "Select" });

            return Json(new SelectList(manageBoardPriorities, "BoardPriorityId", "BoardPriorityName"));
        }

        public async Task<JsonResult> GetBoardStatusesJson(int BoardId)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageBoardStatus> manageBoardStatuses = new List<ManageBoardStatus>();
            manageBoardStatuses = ManageAccess.GetBoardStatuses(config, BoardId);

            manageBoardStatuses.Insert(0, new ManageBoardStatus { BoardStatusId = 0, BoardStatusName = "Select" });

            return Json(new SelectList(manageBoardStatuses, "BoardStatusId", "BoardStatusName"));
        }

        public List<ThreatLockerOrganization> AutoMatchOrganizations(List<ThreatLockerOrganization> threatLockerOrganizations, List<ManageCompany> manageCompanies)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            List<ThreatLockerOrganization> matchedOrgs = new List<ThreatLockerOrganization>();

            foreach (var orgs in threatLockerOrganizations)
            {
                foreach (var comp in manageCompanies)
                {
                    if (comparer.Compare(orgs.Name, comp.Name) == 0)
                    {
                        orgs.ManageCompanyId = comp.Id;
                        matchedOrgs.Add(orgs);
                    }
                }
            }

            _appDb.SaveThreatLockerOrganizations(matchedOrgs);


            return matchedOrgs;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
