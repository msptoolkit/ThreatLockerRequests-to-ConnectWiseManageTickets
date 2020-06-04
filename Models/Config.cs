using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.Models
{
    public class Config
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string ThreatLockerUrl { get; set; }
        [Required]
        [MaxLength(250)]
        public string ThreatlockerAuth { get; set; }
        [Required]
        [MaxLength(250)]
        public string ManageUrl { get; set; }
        [Required]
        [MaxLength(250)]
        public string ManagePubKey { get; set; }
        [Required]
        [MaxLength(250)]
        public string ManagePriKey { get; set; }
        [Required]
        [MaxLength(250)]
        public string ManageCompanyName { get; set; }
        [Required]
        [MaxLength(250)]
        public string ManageClientId { get; set; }
        public int RequestCheckDelay { get; set; }
        public DateTime LastSuccessRequestSent { get; set; }

    }
}
