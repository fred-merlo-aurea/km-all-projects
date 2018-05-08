using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMSite
{
    public class InvalidLinkException : Exception
    {
        public InvalidLinkException() 
        { 
        
        }

        public InvalidLinkException(string message) : base(message) 
        { 
        
        }
    }
}
