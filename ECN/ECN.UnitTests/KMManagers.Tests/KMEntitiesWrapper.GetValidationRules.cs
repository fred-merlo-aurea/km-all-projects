using System.Collections.Generic;
using Shouldly;
using NUnit.Framework;
using KMDbManagers.Fakes;
using KMEntities;

namespace KMManagers.Tests
{
    [TestFixture]
    public partial class KMEntitiesWrapperTest
    {
        private const int DefaultControlPropertyValue = 1;
        private const int StrLenControlPropertyValue = 2;
        private const int ValuesCountControlPropertyValue = 3;

        [Test]
        public void GetValidationRules_ControlTypeGrid_ReturnsValidationRules()
        {
            // Arrange
            SetUpFakes();
            var rule = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = DefaultControlPropertyValue,
                        Value = ((int)KMEnums.GridValidation.AtLeastOnePerLine).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.Grid
                }
            };

            var patternManager = new DataTypePatternManager();

            // Act
            rule = (string)controlPropertyManager.GetValidationRules(control, patternManager);

            // Assert
            rule.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetValidationRules_ControlTextBox_ReturnsValidationRules()
        {
            // Arrange
            SetUpFakes();
            ShimControlProperty();
            var rule = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var patternManager = new DataTypePatternManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = DefaultControlPropertyValue,
                        Value = ((int)KMEnums.TextboxDataTypes.Date).ToString()
                    },
                    new FormControlProperty
                    {
                        ControlProperty_ID = StrLenControlPropertyValue,
                        Value = ((int)KMEnums.TextboxDataTypes.Date).ToString() + ";" + ((int)KMEnums.TextboxDataTypes.Number).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.TextBox
                }
            };
            
            // Act
            rule = (string)controlPropertyManager.GetValidationRules(control, patternManager);

            // Assert
            rule.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetValidationRules_ControlCustomTextBox_WithCorrespondingPattern_ReturnsValidationRules()
        {
            // Arrange
            SetUpFakes();
            ShimControlProperty();
            var rule = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var patternManager = new DataTypePatternManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = DefaultControlPropertyValue,
                        Value = ((int)KMEnums.TextboxDataTypes.Custom).ToString()
                    },
                    new FormControlProperty
                    {
                        ControlProperty_ID = ValuesCountControlPropertyValue,
                        Value = ((int)KMEnums.TextboxDataTypes.Custom).ToString() + ";" + ((int)KMEnums.TextboxDataTypes.Text).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.TextBox
                }
            };

            // Act
            rule = (string)controlPropertyManager.GetValidationRules(control, patternManager);

            // Assert
            rule.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetValidationRules_ControlListBox_ReturnsValidationRules()
        {
            // Arrange
            SetUpFakes();
            ShimControlProperty();
            var rule = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var patternManager = new DataTypePatternManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = DefaultControlPropertyValue,
                        Value = ((int)KMEnums.ControlType.ListBox).ToString()
                    },
                    new FormControlProperty
                    {
                        ControlProperty_ID = ValuesCountControlPropertyValue,
                        Value = ((int)KMEnums.ControlType.ListBox).ToString() + ";" + ((int)KMEnums.TextboxDataTypes.Text).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.ListBox
                }
            };

            // Act
            rule = (string)controlPropertyManager.GetValidationRules(control, patternManager);

            // Assert
            rule.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void GetValidationRules_ControlCaptcha_ReturnsValidationRules()
        {
            // Arrange
            SetUpFakes();
            var rule = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var patternManager = new DataTypePatternManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = DefaultControlPropertyValue,
                        Value = ((int)KMEnums.ControlType.Captcha).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.Captcha
                }
            };

            // Act
            rule = (string)controlPropertyManager.GetValidationRules(control, patternManager);

            // Assert
            rule.ShouldNotBeNullOrEmpty();
        }

        private static void ShimControlProperty()
        {
            ShimControlPropertyDbManager.AllInstances.GetPropertyByNameAndControlStringControl =
                (cpm, c, n) =>
                {
                    if (c == HTMLGenerator.StrLen_Property)
                    {
                        return new ControlProperty
                        {
                            ControlProperty_Seq_ID = StrLenControlPropertyValue
                        };
                    }
                    else if (c == HTMLGenerator.ValuesCount_Property)
                    {
                        return new ControlProperty
                        {
                            ControlProperty_Seq_ID = ValuesCountControlPropertyValue
                        };
                    }
                    else
                    {
                        return new ControlProperty
                        {
                            ControlProperty_Seq_ID = DefaultControlPropertyValue
                        };
                    }

                };
        }
    }
}
