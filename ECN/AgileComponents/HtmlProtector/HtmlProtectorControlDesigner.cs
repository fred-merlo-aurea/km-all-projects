// HtmlProtector
// Copyright (c) 2002 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Text;

namespace ActiveUp.WebControls
{
    /// <summary>
    /// This class is used to render the html protector at design time.
    /// </summary>
    /// <remarks>You should not use this class in your project.</remarks>
    class HtmlProtectorControlDesigner : ControlDesigner
    {
        /// <summary>
        /// Gets the design time HTML code.
        /// </summary>
        /// <returns>A string containing the HTML to render.</returns>
        public override string GetDesignTimeHtml()
        {
            HtmlProtector protector = (HtmlProtector)base.Component;

            StringBuilder stringBuilder = new StringBuilder();

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

            writer.Write("[HtmlProtector \"{0}\"]",protector.ClientID);

            return stringWriter.ToString();
        }

        /// <summary>
        /// Gets the design time HTML code when empty (never used in HtmlProtector).
        /// </summary>
        /// <returns>A string containing the HTML to render.</returns>
        protected override string GetEmptyDesignTimeHtml()
        {
            string text;
            text = "this should be never displayed.";
            return CreatePlaceHolderDesignTimeHtml(text);
        }
    }
}
