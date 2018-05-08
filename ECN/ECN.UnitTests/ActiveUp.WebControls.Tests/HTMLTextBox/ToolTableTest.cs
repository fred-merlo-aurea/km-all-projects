using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using ActiveUp.WebControls.Tests.Helper;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    /// <summary>
    /// Unit Tests for <see cref="ToolTable.AddTableEditor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolTableTest
    {
        private const string AddTableEditor = "AddTableEditor";
        private const string OnClick = "					<td align=\"center\" onclick=\"HTB_CreateTableQuick('";
        private const string OnMouseOver = ";HTB_TableBuilderClear();\" onmouseover=\"HTB_TableBuilderOver(this, ";
        private const string OnMouseOut = "\" onmouseout=\"HTB_TableBuilderClear();\">\n";
        private const string Row = "				<tr>\n";
        private const string RowClose = "				</tr>\n";
        private const string NestedRow = "							<tr>\n";
        private const string NestedRowClose = "							</tr>\n";
        private const string CellTable = "						<table id=\"cell";
        private const string CellTableClose = "						</table>\n";
        private const string HtbClass = " class=\"HTB_clsColor\" width=\"18\" height=\"18\">\n";
        private const string EmptyColumn = "								<td></td>\n";
        private const string ColumnClose = "					</td>\n";

        private Editor _editor;
        private ToolTable _toolTable;

        [SetUp]
        public void Setup()
        {
            _toolTable = new ToolTable();
        }

        [Test]
        public void AddTableEditor_IfEditorNotNull_AddEditorTableHtmlToItems()
        {
            // Arrange
            _editor = new Editor();

            // Act
            _toolTable.GetType().CallMethod(AddTableEditor, new object[] { _editor }, _toolTable);

            // Assert
            _toolTable.Items.ShouldSatisfyAllConditions(
                () => _toolTable.Items.Count.ShouldBe(1),
                () => _toolTable.Items[0].Text.ShouldBe(EditorTableHtml.ToString()),
                () => _toolTable.Items[0].Value.ShouldBeEmpty());
        }

        [Test]
        public void AddTableEditor_IfEditorNull_ThrowNullReferenceException()
        {
            // Arrange
            _editor = null;

            // Act
            Action action = () => _toolTable.GetType().CallMethod(AddTableEditor, new object[] { _editor }, _toolTable);

            // Assert
            action
                .ShouldThrow<TargetInvocationException>()
                .InnerException
                .ShouldBeOfType<NullReferenceException>();
        }

        private StringBuilder EditorTableHtml => new StringBuilder()
            .Append(Header)
            .Append(GetRow(1))
            .Append(GetRow(2))
            .Append(GetRow(3))
            .Append(GetRow(4))
            .Append(Footer);


        private StringBuilder Header => new StringBuilder()
            .Append("<table \"style='background-color: #E0E0E0;'\">\n")
            .Append("<tr>\n")
            .Append("		<td align=\"center\">\n")
            .Append("			<table width=\"100%\" cellspacing=\"0\" cellpadding=\"1\">\n");

        private StringBuilder GetRow(int number) => new StringBuilder()
            .Append(Row)
            .Append($"{OnClick}{_editor.ClientID}', 1, {number}){OnMouseOver}0, {number - 1}){OnMouseOut}")
            .Append($"{CellTable}0{number - 1}\"{HtbClass}").Append(NestedRow).Append(EmptyColumn).Append(NestedRowClose).Append(CellTableClose)
            .Append(ColumnClose)
            .Append($"{OnClick}{_editor.ClientID}', 2, {number}){OnMouseOver}1, {number - 1}){OnMouseOut}")
            .Append($"{CellTable}1{number - 1}\"{HtbClass}").Append(NestedRow).Append(EmptyColumn).Append(NestedRowClose).Append(CellTableClose)
            .Append(ColumnClose)
            .Append($"{OnClick}{_editor.ClientID}', 3, {number}){OnMouseOver}2, {number - 1}){OnMouseOut}")
            .Append($"{CellTable}2{number - 1}\"{HtbClass}").Append(NestedRow).Append(EmptyColumn).Append(NestedRowClose).Append(CellTableClose)
            .Append(ColumnClose)
            .Append($"{OnClick}{_editor.ClientID}', 4, {number}){OnMouseOver}3, {number - 1}){OnMouseOut}")
            .Append($"{CellTable}3{number - 1}\"{HtbClass}").Append(NestedRow).Append(EmptyColumn).Append(NestedRowClose).Append(CellTableClose)
            .Append(ColumnClose)
            .Append($"{OnClick}{_editor.ClientID}', 5, {number}){OnMouseOver}4, {number - 1}){OnMouseOut}")
            .Append($"{CellTable}4{number - 1}\"{HtbClass}").Append(NestedRow).Append(EmptyColumn).Append(NestedRowClose).Append(CellTableClose)
            .Append(ColumnClose)
            .Append(RowClose);

        private StringBuilder Footer
        {
            get
            {
                var tableOnClick = "			<table onclick=\"HTB_SetPopupPosition(";
                var onMouseOut = " onmouseout=\"HTB_OnColorOff(this);\" class=\"HTB_clsColorCont\" width=\"100%\">\n";
                var onMouseOver = $";HTB_OnColorOff(this);\" onmouseover=\"HTB_OnColorOver(this);\"{onMouseOut}";
                var openTableEditor = "\' + \'_TableEditor\'); HTB_OpenTableEditor(\'";

                return new StringBuilder()
                    .Append("			</table>\n")
                    .Append("			<span class=\"HTB_clsFont\" id=\"tableInfo\">Cancel</span>\n")
                    .Append("			<hr size=\"2\" width=\"100%\">\n")
                    .Append($"{tableOnClick}\'{_editor.ClientID}\',\'{_toolTable.ClientID}{openTableEditor}{_toolTable.ClientID}\' + \'_TableEditor\',\'{_editor.ClientID}\'){onMouseOver}")
                    .Append("				<tr>\n")
                    .Append("					<td align=\"center\"><span class=\"HTB_clsFont\">Table Editor...</span></td>\n")
                    .Append("				</tr>\n")
                    .Append("			</table>\n")
                    .Append("		</td>\n")
                    .Append("	</tr>\n")
                    .Append("</table>\n");
            }
        }
    }
}
