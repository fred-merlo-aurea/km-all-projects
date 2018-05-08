using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KM.Common.UserControls
{
    public class SelectBuilder
    {
        private const string IgnoreColumnName = "Ignore";

        private HtmlSelect _selectbox;

        public SelectBuilder(string id)
        {
            _selectbox = new HtmlSelect
            {
                ID = id
            };
            _selectbox.Style.Add("font-family", "Verdana, Arial, Helvetica, sans-serif");
            _selectbox.Style.Add("font-size", "11px");
            _selectbox.Style.Add("background-color", "#FCF8E9");
        }

        public SelectBuilder Bind(ArrayList columnHeaderSelect)
        {
            _selectbox.DataSource = columnHeaderSelect;
            _selectbox.DataBind();

            return this;
        }

        public SelectBuilder SelectItems(string nameToSelect)
        {
            try
            {
                SelectItemByName(IgnoreColumnName, true);
                foreach (ListItem li in _selectbox.Items)
                {
                    if (li.Text.Equals(nameToSelect, StringComparison.OrdinalIgnoreCase) 
                        || li.Text.Equals( $"user_{nameToSelect}", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectItemByName(IgnoreColumnName, false);
                        li.Selected = true;
                        break;
                    }
                }
            }
            catch
            {
                //ToDo: add logging
                //Was ignored in original code. Decided to leave as is
            }
            return this;
        }

        public HtmlSelect Build()
        {
            return _selectbox;
        }

        private void SelectItemByName(string name, bool selected)
        {
            var item = _selectbox?.Items?.FindByText(name);
            if (item != null)
            {
                item.Selected = selected;
            }
        }
    }
}
