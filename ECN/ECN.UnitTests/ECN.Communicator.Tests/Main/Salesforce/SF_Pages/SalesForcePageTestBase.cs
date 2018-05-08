using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECN.Tests.Helpers;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    public class SalesForcePageTestBase : PageHelper
    {
        protected PrivateObject _testObject;

        protected void ExecuteOnCheck(CheckBox sender, string methodName)
        {
            _testObject.Invoke(methodName, new object[] { sender, null });
        }

        protected void SetParentRowColor(CheckBox sender, Color color)
        {
            (sender.Parent.Parent as GridViewRow).BackColor = color;
        }

        protected void SetCount(string CountPropertyName, int count)
        {
            _testObject.SetFieldOrProperty(CountPropertyName, count);
        }

        protected T GetProperty<T>(string propertyName)
        {
            return (T)_testObject.GetFieldOrProperty(propertyName);
        }

        protected string GetLabelText(string propertyName)
        {
            return GetProperty<Label>(propertyName).Text;
        }

        protected Color GetLabelColor(string propertyName)
        {
            return GetProperty<Label>(propertyName).BackColor;
        }

        protected bool RadioButtonVisible(string propertyName)
        {
            return GetProperty<RadioButtonList>(propertyName).Visible;
        }

        protected IEnumerable<CheckBox> GetAllCheckBoxes(GridView grid, string checkboxId, string checkboxHeaderId)
        {
            foreach (GridViewRow row in grid.Rows)
            {
                var checkbox = row.FindControl(checkboxId) as CheckBox;
                if (checkbox != null)
                {
                    yield return checkbox;
                }
            }
            if (grid.HeaderRow != null)
            {
                var headerCheckbox = grid.HeaderRow.FindControl(checkboxHeaderId) as CheckBox;
                if (headerCheckbox != null)
                {
                    yield return headerCheckbox;
                }
            }
        }

        protected void SetColor(CheckBox sender, string color)
        {
            sender.Attributes["Color"] = color;
        }

        protected Email CreateEmail(int id, string email)
        {
            return new Email()
            {
                EmailID = id,
                Address = "Address",
                Address2 = "Address2",
                Mobile = "Mobile",
                City = "City",
                EmailAddress = email,
                FirstName = "FirstName",
                LastName = "LastName",
                Voice = "Voice",
                State = "State",
                Zip = "Zip",
                Country = "Country"
            };
        }

        protected void SetupAddresses(bool isEcnValid, bool isSfValid)
        {
            var ecn = new AddressValidator.AddressLocation { IsValid = isEcnValid };
            var sf = new AddressValidator.AddressLocation { IsValid = isSfValid };
            _testObject.SetFieldOrProperty("Address_ECN", ecn);
            _testObject.SetFieldOrProperty("Address_SF", sf);
        }

        protected void SetupCompareButton(string command, int id)
        {
            const string imgbtnCompareProperty = "imgbtnCompare";
            var imgbtnCompare = GetProperty<ImageButton>(imgbtnCompareProperty); ;
            imgbtnCompare.CommandName = command;
            imgbtnCompare.CommandArgument = id.ToString();
        }
    }
}
