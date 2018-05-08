using System.Web;

namespace KMSite
{
    public interface IKMAuthenticationManager
    {
        /// <summary>
        /// Creates an instance of <see cref="FormsAuthenticationTicket"/> and adds it to <paramref name="cookieCollection"/> collection
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientGroupId">Client Group Id</param>
        /// <param name="cookieCollection">Response cookie collection where new <see cref="FormsAuthenticationTicket"/> is being added </param>
        /// <returns>True if AuthenticationCookie has been added</returns>
        bool AddFormsAuthenticationCookie(int clientId, int clientGroupId, HttpCookieCollection cookieCollection);

        /// <summary>
        /// Creates an instance of <see cref="FormsAuthenticationTicket"/> and adds it to <paramref name="cookieCollection"/> collection
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientGroupId">Client Group Id</param>
        /// <param name="cookieCollection">Response cookie collection where new <see cref="FormsAuthenticationTicket"/> is being added </param>
        /// <param name="productId">Product id</param>
        /// <returns>True if AuthenticationCookie has been added</returns>
        bool AddFormsAuthenticationCookie(int clientId, int clientGroupId, int productId, HttpCookieCollection cookieCollection);

        /// <summary>
        /// Checks if client has authorized.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>true if <paramref name="clientId"/> is autorised id</returns>
        bool HasAuthorized(int clientId);
    }
}