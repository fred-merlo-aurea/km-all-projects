using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

namespace ActiveUp.WebControls
{
    class BarcodeControlDesigner : ControlDesigner
    {
        private DesignerVerbCollection designerVerbs;
        private string _path = null;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (designerVerbs == null)
                {
                    designerVerbs = new DesignerVerbCollection();
                    designerVerbs.Add(new DesignerVerb("Property Builder...", new EventHandler(this.OnPropertyBuilder)));
                }

                return designerVerbs;
            }
        }

        private void OnPropertyBuilder(object sender, EventArgs e)
        {
            BarcodeComponentEditor compEditor = new BarcodeComponentEditor();
            compEditor.EditComponent(Component);
        }

        public override void Initialize(IComponent component)
        {
            if (!(component is ActiveUp.WebControls.Barcode))
            {
                throw new ArgumentException("Component must be a Barcode control.", "component");
            }
            base.Initialize(component);
        }

        /// <summary>
        /// Gets the design time HTML code.
        /// </summary>	
        /// <returns>A string containing the HTML to render.</returns>
        public override string GetDesignTimeHtml()
        {
            Barcode barcode = (Barcode)base.Component;

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

            barcode.RenderControl(writer);

            return stringWriter.ToString();

            /*if (barcode.Height == 0)
            {
                throw new Exception("Barcode height cannot be 0.");
            }

            if (_path == null)
            {
                _path = Path.GetTempFileName();
            }
            else
            {
                try
                {
                    File.Delete(_path);
                }
                catch
                {
                    _path = Path.GetTempFileName();
                }
            }

            GenerateBarcode(_path);
            return "<img src='file://" + _path.Replace('\\','/') + "'>";*/
            
        }

        private void GenerateBarcode(string path)
        {
            IBarcodeEncoder enc = new Code128Encoder();
            enc.Text = "123456";

            BarcodeRenderMode mode = BarcodeRenderMode.None;
            mode |= BarcodeRenderMode.Numbered;

            enc.Sizer.Mode = mode;
            enc.Sizer.DPI = 0;

            IBarcodeGenerator gen = enc.Generator;
            Bitmap bmp = gen.GenerateBarcode(new Size(68,40));
            bmp.Save(_path,System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
