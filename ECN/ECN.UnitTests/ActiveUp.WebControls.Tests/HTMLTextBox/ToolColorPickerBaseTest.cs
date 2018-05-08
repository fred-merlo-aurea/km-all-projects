using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.UI;
using ActiveUp.WebControls.Tests.Helper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    /// <summary>
    /// Unit tests for <see cref="ToolColorPickerBase.AddColorPicker"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class ToolColorPickerBaseTest
    {
        private const string TableDrowDownContent = "<table class=\"HTB_clsDropDownContent\">";
        private const string TableDrowDownContentColumn = "<td><span class=\"HTB_clsFont\">Standard Colors</span>";
        private const string NestedTable = "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">";
        private const string Row = "<tr>";
        private const string Column = "<td>";
        private const string TableColor = "<table class=\"HTB_clsColorCont\"";
        private const string Onclick = " onclick=\"HTB_SetSelectedColor('";
        private const string OnMouseOver = ";HTB_OnColorOff(this);\" onmouseover=\"HTB_OnColorOver(this);\" ";
        private const string OnMouseOut = "onmouseout=\"HTB_OnColorOff(this);\" ";
        private const string Style = "width=\"18\" height=\"18\" cellpadding=\"0\" cellspacing=\"0\">";
        private const string ColumnEnd = "</td>";
        private const string RowEnd = "</tr>";
        private const string TableEnd = "</table>";
        private const string TableColorColumn = "<td align=\"center\">";
        private const string TableDrowDownItem = "<table class=\"HTB_clsDropDownItem\" ";
        private const string AddColorPicker = "AddColorPicker";
        private const string Parent = "_parent";

        private ToolColorPickerBase _toolColorPicker;
        private Editor _editor;
        private StringBuilder _expectedHtml;

        [SetUp]
        public void Setup()
        {
            _toolColorPicker = new ToolColorPickerBase();
            _toolColorPicker.SetField(Parent, GetControl());
            _editor = (Editor)_toolColorPicker.Parent.Parent.Parent;

            _expectedHtml = new StringBuilder();
        }

        [Test]
        public void AddColorPicker_WhenCalled_VerifyHtml()
        {
            // Arrange
            AppendHeader(_expectedHtml);
            AppendRow("black");
            AppendRow("white");
            AppendRow("#008000");
            AppendRow("#800000");
            AppendRow("#808000");
            AppendRow("#000080");
            AppendRow("#800080");
            AppendRow("#808080");

            _expectedHtml.Append(RowEnd);
            _expectedHtml.Append(Row);

            AppendRow("#FFFF00");
            AppendRow("#00FF00");
            AppendRow("#00FFFF");
            AppendRow("#FF00FF");
            AppendRow("#C0C0C0");
            AppendRow("#FF0000");
            AppendRow("#0000FF");
            AppendRow("#008080");
            AppendFooter();

            // Act
            _toolColorPicker.GetType().CallMethod(AddColorPicker, new object[0], _toolColorPicker);

            // Assert
            _toolColorPicker.Items.ShouldSatisfyAllConditions(
                () => _toolColorPicker.Items.Count.ShouldBe(1),
                () => Normalize.ShouldBe(_expectedHtml.ToString()));
        }

        private string Normalize => _toolColorPicker.Items[0].Text
            .Replace("\t", string.Empty)
            .Replace(" <", "<")
            .Replace(" onmouseover", "onmouseover")
            .Replace("onmouseover", " onmouseover")
            .Replace("\n", string.Empty);

        private void AppendFooter()
        {
            var showPopup =
                $"_CustomColors'); ATB_showPopup('{_toolColorPicker.ClientID}_CustomColors');{_toolColorPicker.ClientColorSelected}{OnMouseOver}{OnMouseOut}width=\"100%\">";
            _expectedHtml.Append(RowEnd)
                .Append(TableEnd)
                .Append("<hr size=\"2\" width=\"100%\">")

              .Append(
                    $"<table class=\"HTB_clsColorCont\" onclick=\"HTB_SetPopupPosition('{_editor.ClientID}','{_toolColorPicker.ClientID}{showPopup}")
                .Append("<tr>")
                .Append("<td align=\"center\"><span class=\"HTB_clsFont\">More Colors...</span></td>")
                .Append(RowEnd)
                .Append(TableEnd)
                .Append(ColumnEnd)
                .Append(RowEnd)
                .Append(TableEnd);
        }

        private void AppendRow(string color)
        {
            _expectedHtml.Append(Column)
                .Append($"{TableColor}{Onclick}{_toolColorPicker.ClientID}\',\'{color}\');{_toolColorPicker.ClientColorSelected}{OnMouseOver}{OnMouseOut}{Style}")
                .Append(Row)
                .Append(TableColorColumn)
                .Append($"{TableDrowDownItem}style=\"background-color: {color};\" width=\"12\" height=\"12\">")
                .Append(Row)
                .Append("<td></td>")
                .Append(RowEnd)
                .Append(TableEnd)
                .Append(ColumnEnd)
                .Append(RowEnd)
                .Append(TableEnd)
                .Append(ColumnEnd);
        }

        private static void AppendHeader(StringBuilder expected)
        {
            expected.Append(TableDrowDownContent).Append(Row)
                .Append(TableDrowDownContentColumn)
                .Append(NestedTable).Append(Row);
        }

        private Control GetControl()
        {
            Control control = new Control();
            Control parent = new Control();
            parent.SetField(Parent, new Editor());
            control.SetField(Parent, parent);
            return control;
        }
    }
}
