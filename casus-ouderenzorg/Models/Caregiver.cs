﻿using System.ComponentModel.DataAnnotations;

namespace casus_ouderenzorg.Models
{
    public class Caregiver
    {
        [Key]
        public int CaregiverID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Transport> Transports { get; set; } = new List<Transport>();
    }

}
