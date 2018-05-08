using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using ExCSS;
using KMEntities;
using KMEnums;
using KMModels.PostModels;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace KMModels.Tests.PostModels
{
    /// <summary>
    /// Unit test for <see cref="FormStylesPostModel"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class FormStylesPostModelTest
    {
        private const string CssFileName = "UnitTestFile";
        private const string KmForm = ".kmForm";
        private const string LiDivlabel = "li>div>label";
        private const string Color = "color";
        public const string FileName = "SampleStyle.css";
        public const string DefaultSplitter = "bin";
        private static Guid _cssFileUID = Guid.NewGuid();
        private const string DefaultColor = "rgba(0,0,0,0)";

        private FormStylesPostModel _formStylesPostModel;

        [SetUp]
        public void SetUp()
        {
            _formStylesPostModel = new FormStylesPostModel();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void FillData_EntityIsStyleSheet_UpdatedCssFileObject(bool includeCategory)
        {
            // Arrange
            var currentExecutablePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var filePath = currentExecutablePath.Split(new string[] { DefaultSplitter }, StringSplitOptions.None);
            var parser = new Parser();
            var resultCss = File.ReadAllText(string.Concat(filePath[0], FileName));
            var stylesheet = parser.Parse(string.Concat(resultCss, GetCategoryCss(includeCategory)));

            // Act
            _formStylesPostModel.FillData(stylesheet);

            // Assert
            _formStylesPostModel.ShouldSatisfyAllConditions(
                () => _formStylesPostModel.ShouldNotBeNull(),
                () => _formStylesPostModel.StylingType.ShouldBe(StylingType.External)
             );
        }

        [TestCase(1)]
        [TestCase(0)]
        public void FillData_EntityTypeIsForm_UpdatedCssFileObject(int stylingType)
        {
            // Arrange
            var entity = CreateFormObject(stylingType);
            // Act
            _formStylesPostModel.FillData(entity);

            // Assert
            if (stylingType == 1)
            {
                _formStylesPostModel.ShouldSatisfyAllConditions(
                    () => _formStylesPostModel.ShouldNotBeNull(),
                    () => _formStylesPostModel.File.ShouldNotBeNull(),
                    () => _formStylesPostModel.File.Name.ShouldBe(CssFileName),
                    () => _formStylesPostModel.File.UID.ShouldBe(_cssFileUID),
                    () => _formStylesPostModel.StylingType.ShouldBe(StylingType.Upload)
                 );
            }
            else
            {
                _formStylesPostModel.ShouldSatisfyAllConditions(
                    () => _formStylesPostModel.ShouldNotBeNull(),
                    () => _formStylesPostModel.StylingType.ShouldBe(StylingType.External)
                 );
            }
        }

        [TestCase(Alignment.Left, true)]
        [TestCase(Alignment.Center, true)]
        [TestCase(Alignment.Right, true)]
        [TestCase(Alignment.Left, false)]
        [TestCase(Alignment.Center, false)]
        [TestCase(Alignment.Right, false)]

        public void RewriteCss_ByCssObject_ReturnsUpdatedCssObject(Alignment alignmentType, bool border)
        {
            // Arrange
            var parser = new Parser();
            var currentExecutablePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var filePath = currentExecutablePath.Split(new string[] { DefaultSplitter }, StringSplitOptions.None);
            var resultCss = File.ReadAllText(string.Concat(filePath[0], FileName));
            resultCss = string.Concat(resultCss, GetCategoryCss(true));
            CreateCustomStyles(alignmentType, border);
            var customControl = CreateCustomControl();
            var controlsStyles = new Dictionary<StyledControlType, ControlStyles>();
            controlsStyles.Add(StyledControlType.CheckBox, customControl);
            controlsStyles.Add(StyledControlType.ListBox, customControl);
            controlsStyles.Add(StyledControlType.Grid, customControl);
            _formStylesPostModel.CustomStyles.ControlsStyles = controlsStyles;

            // Act
            var result = _formStylesPostModel.RewriteCss(resultCss);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            _formStylesPostModel.ShouldSatisfyAllConditions(
                () => _formStylesPostModel.ShouldNotBeNull(),
                () => _formStylesPostModel.StylingType.ShouldBe(StylingType.External)
             );
        }

        private void CreateCustomStyles(Alignment alignmentType, bool border)
        {
            _formStylesPostModel.CustomStyles = new CustomStyles
            {
                FormStyles = new FormStyles
                {
                    Alignment = alignmentType,
                    Border = border,
                    BorderColor = DefaultColor,
                    BackgroundColor = DefaultColor,
                    Color = DefaultColor,
                    Spacing = 1,
                },
                ButtonStyles = new ButtonStyles
                {
                    BackgroundColor = DefaultColor,
                    BorderColor = DefaultColor,
                    Color = DefaultColor,
                    Font = "normal 13px arial,sans-serif;",
                    FontBold = true,
                    FontSize = 12
                }
            };
        }

        private ControlStyles CreateCustomControl()
        {
            var customControl = new ControlStyles(true);
            customControl.BackgroundColor = _formStylesPostModel.CustomStyles.ButtonStyles.BackgroundColor;
            customControl.BorderColor = _formStylesPostModel.CustomStyles.ButtonStyles.BorderColor;
            customControl.TextColor = _formStylesPostModel.CustomStyles.ButtonStyles.Color;
            customControl.Font = _formStylesPostModel.CustomStyles.ButtonStyles.Font;
            customControl.FontBold = _formStylesPostModel.CustomStyles.ButtonStyles.FontBold;
            customControl.FontSize = _formStylesPostModel.CustomStyles.ButtonStyles.FontSize;
            customControl.Border = _formStylesPostModel.CustomStyles.FormStyles.Border;
            return customControl;
        }


        private Form CreateFormObject(int stylingType)
        {
            return new Form
            {
                CssFile = new KMEntities.CssFile
                {
                    Name = CssFileName,
                    CssFileUID = _cssFileUID,
                },
                StylingType = stylingType,
            };
        }

        private string GetCategoryCss(bool includeCategory)
        {
            if (includeCategory)
            {
                return @".kmForm .kmCheckbox .category
                {
                  display:block;
                  color:#000;
                  background-color:#FFF;
                  font-family:Arial;
                  font-weight:bold;
                  font-size:12px;
                  margin: 10px 0 5px 5px;
                  font: normal 13px arial,sans-serif;
                }";
            }
            else
            {
                return @".kmForm .kmCheckbox .category
                {
                  display:block;
                  color:#fff;
                  background-color:#000;
                  font-family:Vardana;
                  font-weight:bold;
                  font-size:9px;
                  margin: 23px 2 10px 12px;
                  font: normal;
                }";
            }
        }
    }
}
