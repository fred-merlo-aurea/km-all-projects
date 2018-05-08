using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.API.Models.EmailGroup
{
    public interface IHasEmailAddress
    {
        string EmailAddress { get; }
    }
}
