﻿using OwlReadingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DocumentType DocumentType { get; set; }
        public String PlaceOfIssue { get; set; }
        public List<DocumentImageViewModel> Locations { get; set; }
    }
}
