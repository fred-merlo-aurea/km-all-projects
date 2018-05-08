using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveUp.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ActiveUp.WebControls.Tests.Utils
{
    public abstract class HtmlDesignerTestBase
    {
        protected const string Zero = "0";
        protected const string One = "1";
        protected const string Two = "2";
        protected const string Style = "position: relative; display: block; width:100%; border: 0px;";
        protected const string Full = "100%";
        protected const string Left = "left";
        protected const string Right = "right";
        protected const string AbsMiddle = "absmiddle";
        protected const string Middle = "middle";
        protected const string Space = "&nbsp;";
        protected const string FontSize = "11px";
        protected const string Font = "verdana";

        protected StringWriter StringWriter;
        protected HtmlTextWriter Writer;
        protected Editor Editor;

        private IDisposable _context;

        public virtual void SetUp()
        {
            _context = ShimsContext.Create();
            StringWriter = new StringWriter();
            Writer = new HtmlTextWriter(StringWriter);
            Editor = new Editor();

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        protected virtual void SetupFakes()
        {
            ShimToolbar.AllInstances.ToolsGet =
                _ => new ToolCollection(new ControlCollection(new Control()));
        }

        protected virtual void Render(string icon, string fileName, string label)
        {
            RenderBeginTag(HtmlTextWriterTag.Table, HtmlTextWriterTag.Tr, HtmlTextWriterTag.Td);
            AddStyleAttribute(HtmlTextWriterStyle.FontFamily, Font);
            AddStyleAttribute(HtmlTextWriterStyle.FontSize, FontSize);
            RenderBeginTag(HtmlTextWriterTag.Span);
            Write(Space);
            AddAttributes(
                Attributes(HtmlTextWriterAttribute.Border, HtmlTextWriterAttribute.Align, HtmlTextWriterAttribute.Src),
                Zero,
                AbsMiddle,
                Image(icon, fileName));
            RenderBeginTag(HtmlTextWriterTag.Img);
            RenderEndTag();
            Write(Space);
            Write(label);
            Write(Space);
        }

        protected virtual string Image(string icon, string fileName)
        {
            return WebControls.Utils
             .ConvertToImageDir(
                 Editor.IconsDirectory == "/" ?
                     Editor.IconsDirectory = string.Empty :
                     Editor.IconsDirectory,
                 icon,
                 fileName,
                 Editor.Page,
                 Editor.GetType());
        }

        protected virtual void RenderStartupModeStyles()
        {
            AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, One);
            AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, BorderStyle.Solid.ToString());
            AddStyleAttribute(HtmlTextWriterStyle.BorderColor, WebControls.Utils.Color2Hex(Color.Black));
            AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, WebControls.Utils.Color2Hex(Color.White));
        }

        protected virtual void RenderEndTag()
        {
            Writer.RenderEndTag();
        }

        protected virtual void RenderBeginTag(params HtmlTextWriterTag[] tags)
        {
            foreach (var tag in tags)
            {
                Writer.RenderBeginTag(tag);
            }
        }

        protected virtual void Render(int times, Action action)
        {
            for (int index = 0; index < times; index++)
            {
                action.Invoke();
            }
        }

        protected virtual HtmlTextWriterAttribute[] Attributes(params HtmlTextWriterAttribute[] keys)
        {
            return keys;
        }

        protected virtual void AddAttributes(IReadOnlyList<HtmlTextWriterAttribute> keys, params string[] attributeValues)
        {
            for (var index = 0; index < keys.Count; index++)
            {
                var key = keys[index];
                Writer.AddAttribute(key, attributeValues[index]);
            }
        }

        protected virtual void AddAttribute(HtmlTextWriterAttribute key, string attributeValue)
        {
            Writer.AddAttribute(key, attributeValue);
        }

        protected virtual void AddStyleAttribute(HtmlTextWriterStyle key, string attributeValue)
        {
            Writer.AddStyleAttribute(key, attributeValue);
        }

        protected virtual void Write(string key)
        {
            Writer.Write(key);
        }
    }
}
