using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using KMPS.MD.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ControlsValidatorsTests
    {
        private const string DefaultKey = "Default";
        private const string LowerCaseKey = "LOWERCASE";
        private const string ProperCaseKey = "PROPERCASE";
        private const string UpperCaseKey = "UPPERCASE";

        private const string LowerCaseDescription = "lower case";
        private const string ProperCaseDescription = "Proper Case";
        private const string UpperCaseDescription = "UPPER CASE";

        private Dictionary<string, string> _downloadfields;
        private ListBox _selectedFields;
        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Dispose();
        }

        [Test]
        public void LoadEditCase_WhenDownloadFieldsIsNull_ThrowsException()
        {
            // Arrange
            _downloadfields = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ControlsValidators.LoadEditCase(_downloadfields, _selectedFields));
        }

        [Test]
        public void LoadEditCase_WhenSelectedFieldsIsNull_ThrowsException()
        {
            // Arrange
            _selectedFields = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ControlsValidators.LoadEditCase(_downloadfields, _selectedFields));
        }

        [Test]
        public void LoadEditCase_WhenItemIsNotNull_ShouldInsertItemsToListBox()
        {
            // Arrange
            _selectedFields = CreateSelectedFieldsList();
            _downloadfields = CreateDownloadFieldsList();

            var resultSelectFields = CreateSelectedFieldsList();
            var resultDownloadFields = CreateDownloadFieldsList();
            var result = CreateEditCaseResultListBox(resultDownloadFields, resultSelectFields);

            // Act
            ControlsValidators.LoadEditCase(_downloadfields, _selectedFields);

            // Assert
            _selectedFields.ShouldNotBeNull();
            _selectedFields.ShouldSatisfyAllConditions(
                () => _selectedFields.Items.Count.ShouldBe(result.Items.Count),
                () => _selectedFields.Items[0].Text.ShouldBe(result.Items[0].Text),
                () => _selectedFields.Items[0].Value.ShouldBe(result.Items[0].Value),
                () => _selectedFields.Items[1].Text.ShouldBe(result.Items[1].Text),
                () => _selectedFields.Items[1].Value.ShouldBe(result.Items[1].Value),
                () => _selectedFields.Items[2].Text.ShouldBe(result.Items[2].Text),
                () => _selectedFields.Items[2].Value.ShouldBe(result.Items[2].Value),
                () => _selectedFields.Items[3].Text.ShouldBe(result.Items[3].Text),
                () => _selectedFields.Items[3].Value.ShouldBe(result.Items[3].Value));
        }

        [Test]
        public void ReorderSelectedList_WhenListBoxIsNull_ThrowsException()
        {
            // Arrange
            _selectedFields = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ControlsValidators.ReorderSelectedList(_selectedFields));
        }

        [Test]
        public void ReorderSelectedList_WhenListBoxIsNotNull_ShouldReorderListBox()
        {
            // Arrange
            _selectedFields = CreateSelectedFieldsList();
            var resultSelectedFields = CreateSelectedFieldsList();
            var result = CreateReorderResult(resultSelectedFields);

            // Act
            ControlsValidators.ReorderSelectedList(_selectedFields);

            // Assert
            _selectedFields.ShouldNotBeNull();
            _selectedFields.ShouldSatisfyAllConditions(
                () => _selectedFields.Items.Count.ShouldBe(result.Items.Count),
                () => _selectedFields.Items[0].Text.ShouldBe(result.Items[0].Text),
                () => _selectedFields.Items[0].Value.ShouldBe(result.Items[0].Value),
                () => _selectedFields.Items[1].Text.ShouldBe(result.Items[1].Text),
                () => _selectedFields.Items[1].Value.ShouldBe(result.Items[1].Value),
                () => _selectedFields.Items[2].Text.ShouldBe(result.Items[2].Text),
                () => _selectedFields.Items[2].Value.ShouldBe(result.Items[2].Value),
                () => _selectedFields.Items[3].Text.ShouldBe(result.Items[3].Text),
                () => _selectedFields.Items[3].Value.ShouldBe(result.Items[3].Value));
        }

        private static ListBox CreateSelectedFieldsList()
        {
            return new ListBox
            {
                Items =
                {
                    new ListItem { Text = @"One", Value = @"1|2", Selected = true },
                    new ListItem { Text = @"Two", Value = @"2|3", Selected = false },
                    new ListItem { Text = @"Three", Value = @"3|4", Selected = false },
                    new ListItem { Text = @"Four", Value = @"4|5", Selected = false }
                }
            };
        }

        private static Dictionary<string, string> CreateDownloadFieldsList()
        {
            return new Dictionary<string, string>
            {
                ["1|2"] = LowerCaseKey,
                ["2|3"] = ProperCaseKey,
                ["3|4"] = UpperCaseKey,
                ["4|5"] = DefaultKey
            };
        }

        private static ListBox CreateEditCaseResultListBox(IDictionary<string, string> downloadfields, ListBox selectedFields)
        {
            foreach (var field in downloadfields)
            {
                var item = selectedFields.Items.FindByValue(field.Key);
                if (item != null)
                {
                    var curIndex = selectedFields.Items.IndexOf(item);
                    selectedFields.Items.Remove(item);

                    string text;
                    switch (field.Value.ToUpper())
                    {
                        case ProperCaseKey:
                            text = ProperCaseDescription;
                            break;
                        case UpperCaseKey:
                            text = UpperCaseDescription;
                            break;
                        case LowerCaseKey:
                            text = LowerCaseDescription;
                            break;
                        default:
                            text = DefaultKey;
                            break;
                    }

                    var itemText = $"{item.Text.Split('(')[0].ToUpper()}({text})";
                    var itemValue = $"{item.Value.Split('|')[0]}|{item.Value.Split('|')[1]}|{field.Value}";

                    selectedFields.Items.Insert(curIndex, new ListItem(itemText, itemValue));
                }
            }

            return selectedFields;
        }

        private static ListBox CreateReorderResult(ListBox lstDestFields)
        {
            var startindex = lstDestFields.Items.Count - 1;

            for (var i = startindex; i > -1; i--)
            {
                if (lstDestFields.Items[i].Selected && i < startindex && !lstDestFields.Items[i + 1].Selected)
                {
                    var bottom = lstDestFields.Items[i];
                    lstDestFields.Items.Remove(bottom);
                    lstDestFields.Items.Insert(i + 1, bottom);
                    lstDestFields.Items[i + 1].Selected = true;
                }
            }

            return lstDestFields;
        }
    }
}
