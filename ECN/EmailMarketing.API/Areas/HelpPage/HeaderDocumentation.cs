using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HeaderDocumentationLibrary = System.Collections.Generic.Dictionary<string, EmailMarketing.API.Areas.HelpPage.HeaderDocumentation>;

namespace EmailMarketing.API.Areas.HelpPage
{
    public class HeaderDocumentation
    {
        #region static members

        /// <summary>
        /// Provides access to the (singleton) repository of
        /// documentation and globally applicable HTTP Headers.
        /// </summary>
        /// <returns>a dictionary of header information organized by Name</returns>
        public static HeaderDocumentationLibrary GetLibrary()
        {
            return library;
        }

        public static IEnumerable<HeaderDocumentation> GetHeadersFor(HeaderApplicabilityFlags headerContext)
        {
            IEnumerable<HeaderDocumentation> headers = from x in GetLibrary() where (x.Value.For & headerContext) == headerContext select x.Value;
            if (headers == null)
            {
                headers = new List<HeaderDocumentation>();
            }
            return headers;
        }

        public static HeaderDocumentation GetHeader(string headerName)
        {
            var library = GetLibrary();
            if(false == library.ContainsKey(headerName))
            {
                return null;
            }
            return library[headerName];
        }


        /// <summary>
        /// stores global HTTP Header documentation
        /// </summary>
        static readonly HeaderDocumentationLibrary library = new HeaderDocumentationLibrary()
        {
            { 
                "APIAccessKey", new HeaderDocumentation()
                {
                    Name = "APIAccessKey",
                    Description = "User access token for the Email Marketing API.",
                    For = HeaderApplicabilityFlags.Request
                }
            },
            { 
                "Location", new HeaderDocumentation()
                {
                    Name = "Location",
                    Description = "Unique identifier (URI) for accessing the context resource"
                }
            }
        };

        #endregion static members

        public string Name { get; set; }
        public string Description { get; set; }

        [Flags] public enum HeaderApplicabilityFlags { Request, Response }        
        public HeaderApplicabilityFlags For;
    }
}