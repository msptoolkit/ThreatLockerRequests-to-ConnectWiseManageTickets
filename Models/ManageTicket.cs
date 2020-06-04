using ManageIntegration.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class ManageTicket
    {
        [JsonProperty("company")]
        public ManageCompany Company { get; set; }
        [JsonProperty("board")]
        public ManageBoard Board { get; set; }
        [JsonProperty("type")]
        public ManageBoardType Type { get; set; }
        [JsonProperty("subtype")]
        public ManageBoardSubType SubType { get; set; }
        [JsonProperty("item")]
        public ManageBoardItem Item { get; set; }
        [JsonProperty("priority")]
        public ManageBoardPriority Priority { get; set; }
        [JsonProperty("status")]
        public ManageBoardStatus Status { get; set; }
        public string Summary { get; set; } 
        public string InitialDescription { get; set; }
        public string InitialInternalAnalysis { get; set; }
    }
}