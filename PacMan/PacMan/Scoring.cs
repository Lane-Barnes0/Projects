using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Scoring
    {
        /// <summary>
        /// Have to have a default constructor for the XmlSerializer.Deserialize method
        /// </summary>
        public Scoring() { }

        /// <summary>
        /// Overloaded constructor used to create an object for long term storage
        /// </summary>
        /// <param name="scores"></param>
       
        public Scoring(List<int> scores)
        {  
            this.Score = new List<int>(); 
        }

        public List<int> Score { get; set; }
       
    
    }
}

