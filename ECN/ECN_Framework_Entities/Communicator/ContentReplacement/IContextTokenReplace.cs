using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_Entities.Communicator.ContentReplacement
{
    public interface IContextTokenReplace<ContextT>
    {
        System.Text.RegularExpressions.Regex Pattern { get; }

        event EventHandler<ECN_Framework_Entities.Communicator.ContentReplacement.ContentReplacementEventArgs<ContextT>> OnAfterReplace;

        string GetToken(System.Text.RegularExpressions.Match match);

        /// <summary>
        /// Supply an enumeration of ContextT given an enumeration of matches
        /// </summary>
        /// <param name="text"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        ContextT BuildContext(System.Text.RegularExpressions.Match match);

        /// <summary>
        /// Supply the replacement HTML formatted string given <code>token</code> and <code>match</code>.
        /// Replace HTML is called once for each match success of Pattern in <code>html</code>.
        /// </summary>
        /// <param name="html">HTML formatted text in which the match was found</param>
        /// <param name="token"><code>Value</code> from the first <code>Match Group</code>, or String.Empty</param>
        /// <param name="match">a match found by <see cref="Pattern"/> in <code>html</code></param>
        /// <returns>the substitute for <code>match</code> in <code>html</code></returns>
        string ReplaceHtml(string token, ContextT context);
        
        /// <summary>
        /// Supply the replacement for <code>token</code> given <code>match</code> and context.
        /// <code>ReplaceText</code> is called once for each match success of Pattern in <code>text</code>.
        /// </summary>
        /// <param name="text">text in which the match was found</param>
        /// <param name="token"><code>Value</code> from the first <code>Match Group</code>, or String.Empty</param>
        /// <param name="match">a match found by <see cref="Pattern"/> in <code>html</code></param>
        /// <param name="context">context supplied for BuildContext for the match<code>Value</code></param>
        /// <returns>the substitute for <code>match</code> in <code>text</code></returns>
        string ReplaceText(string token, ContextT context);
    }
}
