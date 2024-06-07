﻿using System.ComponentModel.DataAnnotations;

namespace InfoMed.Models
{
    public class Speakers
    {
        [Key]
        public int IdSpeaker { get; set; }
        public int IdEvent { get; set; }
        public int IdEventVersion { get; set; }
        public string SpeakerName { get; set; }
        public string AboutSpeaker { get; set; }
        public string SpeakerImage { get; set; }
        public int OrderNumber { get; set; }
        public bool Status { get; set; }
    }
}