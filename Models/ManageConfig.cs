using ManageIntegration.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class ManageConfig
    {
        public ManageBoard ManageBoard { get; set; }
        public int BoardId { get; set; }
        public ManageBoardType ManageBoardType { get; set; }
        public int TypeId { get; set; }
        public ManageBoardSubType ManageBoardSubType { get; set; }
        public int SubTypeId { get; set; }
        public ManageBoardItem ManageBoardItem { get; set; }
        public int ItemId { get; set; }
        public ManageBoardPriority ManageBoardPriority { get; set; }
        public int PriorityId { get; set; }
        public ManageBoardStatus ManageBoardStatus { get; set; }
        public int StatusId { get; set; }
        public string TicketSummary { get; set; }
        public List<ManageBoard> ManageBoards { get; set; }
        public List<ManageBoardType> ManageBoardTypes { get; set; }
        public List<ManageBoardSubType> ManageBoardSubTypes { get; set; }
        public List<ManageBoardItem> ManageBoardItems { get; set; }
        public List<ManageBoardPriority> ManageBoardPriorities { get; set; }
        public List<ManageBoardStatus> ManageBoardStatuses { get; set; }
    }
}