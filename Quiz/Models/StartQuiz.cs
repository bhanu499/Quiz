using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class StartQuiz
    {
        [Required]
        public string Name { get; set; }

        public string Topic { get; set; }
    }
}
