using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class ProductWrapper : BaseWrapper
    {
        public ProductWrapper()
        {
            pub = new FrameworkUAD.Entity.Product();
            pubTypes = new List<FrameworkUAD.Entity.ProductTypes>();
            frequency = new List<FrameworkUAD.Entity.Frequency>();
            clients = new List<KMPlatform.Entity.Client>();
            availableGroups = new List<ECN_Framework_Entities.Communicator.Group>();
            selectedGroups = new List<ECN_Framework_Entities.Communicator.Group>();
            selectedGroupsList = null;
            availableBrands = new List<FrameworkUAD.Entity.Brand>();
            selectedBrands = new List<FrameworkUAD.Entity.Brand>();
            selectedBrandsList = null;
            clientID = 0;
        }

        public ProductWrapper(List<FrameworkUAD.Entity.Product> pubsList)
        {
            pubs = pubsList;
        }

        public IEnumerable<FrameworkUAD.Entity.Product> pubs { get; set; }
        public FrameworkUAD.Entity.Product pub { get; set; }
        public List<FrameworkUAD.Entity.ProductTypes> pubTypes { get; set; }
        public List<FrameworkUAD.Entity.Frequency> frequency { get; set; }
        public List<KMPlatform.Entity.Client> clients { get; set; }
        public List<ECN_Framework_Entities.Communicator.Group> availableGroups { get; set; }
        public List<ECN_Framework_Entities.Communicator.Group> selectedGroups { get; set; }
        public IEnumerable<string> selectedGroupsList { get; set; }
        public List<FrameworkUAD.Entity.Brand> availableBrands { get; set; }
        public List<FrameworkUAD.Entity.Brand> selectedBrands { get; set; }
        public IEnumerable<string> selectedBrandsList { get; set; }
        public int clientID { get; set; }
    }
}