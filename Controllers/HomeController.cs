using ManageIntegration.DAL;
using ManageIntegration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using TLManageService.Models;

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

            List<ManageCompany> manageCompanies = ManageAccess.GetCompanies(model, null);
            List<ThreatLockerOrganization> threatLockerOrganizations = ThreatLockerAccess.GetOrganizations(model);

            await _appDb.SaveManageCompanies(manageCompanies);
            await _appDb.SaveThreatLockerOrganizations(threatLockerOrganizations);


            return View();
        }

        [HttpGet("manageconfig")]
        public async Task<IActionResult> ManageConfig()
        {
            Config config = await _appDb.GetConfigAsync();
            ManageConfig manageConfig = await _appDb.GetManageConfigAsync();
            List<ManageCompany> manageCompanies = await _appDb.GetManageCompaniesAsync();
            List<ManageBoard> manageBoards = ManageAccess.GetBoards(config);

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

            List<ManageCompany> manageComapies = ManageAccess.GetCompanies(config, null);
            ViewBag.ListOfBoards = manageBoards;

            await _appDb.SaveManageConfigAsync(model);

            return View(model);
        }

        [HttpGet("/")]
        public async Task<IActionResult> CompanyMappings()
        {
            CompanyMappingViewModel companyMappingViewModel = new CompanyMappingViewModel();

            Config config = await _appDb.GetConfigAsync();

            if(config.ManageClientId == "" || config.ManagePubKey == "")
            {
                companyMappingViewModel.DefaultOrganization = new ThreatLockerOrganization { ManageCompanyId = 1, Name = " ", OrganizationId =" " };
                companyMappingViewModel.ManageCompanies = new List<ManageCompany> { new ManageCompany { Name = " ", Id = 1 } };
                companyMappingViewModel.ThreatLockerOrganizations = new List<ThreatLockerOrganization> { new ThreatLockerOrganization { ManageCompanyId = 1, Name = " ", OrganizationId = " " }  };

                ViewBag.ManageCompanyList = new List<ManageCompany> { new ManageCompany { Name = " ", Id = 1 } };

                return View(companyMappingViewModel);
            }

            companyMappingViewModel.ThreatLockerOrganizations = await _appDb.GetThreatLockerOrganizationsAsync();
            if (companyMappingViewModel.ThreatLockerOrganizations == null)
            {
                companyMappingViewModel.ThreatLockerOrganizations = ThreatLockerAccess.GetOrganizations(config);
            }


            companyMappingViewModel.DefaultOrganization = await _appDb.GetDefaultThreatLockerOrganization();

            List<ManageCompany> manageCompanyList = new List<ManageCompany>();
            manageCompanyList = await _appDb.GetManageCompaniesAsync();

            if(manageCompanyList == null)
            {
                manageCompanyList = ManageAccess.GetCompanies(config, null);
            }

            ViewBag.ManageCompanyList = manageCompanyList;

            companyMappingViewModel.DefaultOrganization = await _appDb.GetDefaultThreatLockerOrganization();

            return View(companyMappingViewModel);
        }

        [HttpPost("")]
        public async Task<IActionResult> CompanyMappings(CompanyMappingViewModel model)
        {
            Config config = await _appDb.GetConfigAsync();

            ViewBag.ManageCompanyList = ManageAccess.GetCompanies(config, null);

            await _appDb.SaveThreatLockerOrganizations(model.ThreatLockerOrganizations);
            await _appDb.SaveDefaultThreatLockerOrganization(model.DefaultOrganization);

            return View(model);
        }

        [HttpGet("getmanagecompanies")]
        public async Task<IActionResult> GetManageCompanies()
        {
            List<ManageCompany> manageCompanies = new List<ManageCompany>();
            Config config = await _appDb.GetConfigAsync();
            if (ViewBag.ManageCompanies == null)
            {
                manageCompanies = await _appDb.GetManageCompaniesAsync();
            }
            else
            {
                manageCompanies = ViewBag.ManageCompanies;
            }

            ManageCompanyViewModel manageCompanyViewModel = new ManageCompanyViewModel();

            manageCompanyViewModel.ManageCompanies = manageCompanies;
            manageCompanyViewModel.ManageCompanyStatuses = ManageAccess.GetCompanyStatuses(config);
            manageCompanyViewModel.ManageCompanyTypes = ManageAccess.GetCompanyTypes(config);


            return View(manageCompanyViewModel);
        }

        [HttpPost("getmanagecompanies")]
        public async Task<IActionResult> GetManageCompanies(ManageCompanyViewModel model)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageCompany> manageCompanies = await _appDb.GetManageCompaniesAsync();
            List<int> manageCompanyStatuses = new List<int>();
            List<int> manageCompanyTypes = new List<int>();

            foreach (var key in Request.Form["ManageCompanyStatuses"])
            {
                manageCompanyStatuses.Add(int.Parse(key));
            }
            foreach (var key in Request.Form["ManageCompanyTypes"])
            {
                manageCompanyTypes.Add(int.Parse(key));
            }
            model.ManageCompanyStatusIds = manageCompanyStatuses;
            model.ManageCompanyTypeIds = manageCompanyTypes;
            model.ManageCompanyStatuses = ManageAccess.GetCompanyStatuses(config);
            model.ManageCompanyTypes = ManageAccess.GetCompanyTypes(config);

            string manageCompanyFilterUrl = ManageAccess.CreateCompanyFilterUrl(manageCompanyStatuses, manageCompanyTypes);

            ViewBag.ManageCompanies = ManageAccess.GetCompanies(config, manageCompanyFilterUrl);

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

        public async Task<JsonResult> GetFilteredManageCompaniesJson (string FilterUrl)
        {
            Config config = await _appDb.GetConfigAsync();
            List<ManageCompany> manageCompanies = new List<ManageCompany>();
            manageCompanies = ManageAccess.GetCompanies(config, null);


            return Json(new SelectList(manageCompanies, "BoardStatusId", "BoardStatusName"));
        }



        public async Task<List<ThreatLockerOrganization>> AutoMatchOrganizations(List<ThreatLockerOrganization> threatLockerOrganizations, List<ManageCompany> manageCompanies)
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

            await _appDb.SaveThreatLockerOrganizations(matchedOrgs);


            return matchedOrgs;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
