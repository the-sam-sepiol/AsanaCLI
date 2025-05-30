﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double CompletionPercent { get; set; }
        public List<ToDo> ToDos { get; set; } = new();

        public override string ToString()
        {
            return $"{Name} - {Description} (Completion: {CompletionPercent * 100}%)";
        }
    }
}
