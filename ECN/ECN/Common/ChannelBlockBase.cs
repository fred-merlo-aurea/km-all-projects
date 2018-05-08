using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN.Common
{
    public abstract class ChannelBlockBase
    {
        public string LoginBlock(string username, string errormessage, string processpage)
        {
            var builder = new StringBuilder();

            builder.Append("<table border=0 cellspacing=1 cellpadding=1>");
            builder.Append($"<form action='{processpage}' method=post><tr>");
            builder.Append("<td class=tableContent align=center>username</td>");
            builder.Append("</tr><tr>");
            builder.Append($"<td><input name=user value='{username}' size=15 maxlength=100 class=formfield type=text></td>");
            builder.Append("</tr><tr>");
            builder.Append("<td class=tableContent align=center>password</td>");
            builder.Append("</tr><tr>");
            builder.Append("<td><input name=password type=Password size=15 maxlength=50 class=formfield></td>");
            builder.Append("</tr><tr>");
            builder.Append("<td class=tableContent align=middle><input type=checkbox name=persist> remember me</td>");
            builder.Append("</tr><tr>");
            builder.Append("<td align=middle><input type=submit name=cmdLogin value='Login' class=formbutton><br>");
            builder.Append($"<div id=ErrorMessage><br><b><font color=darkRed>{errormessage}</font></b></div></td>");
            builder.Append("</tr></form>");
            builder.Append("</table>");

            return builder.ToString();
        }
    }
}
