using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Results
    {
        public string Name { get; set; }

        public string Topic { get; set; }


        public int Score { get; set; }
        
        public DateTime Date { get; set; }
    }
}
