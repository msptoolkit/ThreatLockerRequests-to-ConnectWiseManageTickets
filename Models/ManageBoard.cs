using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class ManageBoard
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ManageBoardType
    {
        [JsonProperty("Id")]
        public int BoardTypeId { get; set; }
        [JsonProperty("Name")]
        public string BoardTypeName { get; set; }
    }
    public class ManageBoardSubType
    {
        [JsonProperty("Id")]
        public int BoardSubTypeId { get; set; }
        [JsonProperty("Name")]
        public string BoardSubTypeName { get; set; }
    }
    public class ManageBoardPriority
    {
        [JsonProperty("Id")]
        public int BoardPriorityId { get; set; }
        [JsonProperty("Name")]
        public string BoardPriorityName { get; set; }
    }
    public class ManageBoardItem
    {
        [JsonProperty("Id")]
        public int BoardItemId { get; set; }
        [JsonProperty("Name")]
        public string BoardItemName { get; set; }
    }
    public class ManageBoardStatus
    {
        [JsonProperty("Id")]
        public int BoardStatusId { get; set; }
        [JsonProperty("Name")]
        public string BoardStatusName { get; set; }
    }
}