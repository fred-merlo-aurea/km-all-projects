using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration.Fakes;
using aspNetMX.Fakes;
using NUnit.Framework;
using KMEntities;
using KMManagers.Fakes;
using KMDbManagers.Fakes;
using Shouldly;
using Enums = KMEnums;

namespace KMManagers.Tests
{
    public partial class KMEntitiesWrapperTest
    {
        [Test]
        public void Validate_ControlTypeDateTextbox_DoesNotMatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = ((int)Enums.TextboxDataTypes.Date).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)Enums.ControlType.TextBox
                }
            };
            var valueToMatch = "1";
            
            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.ShouldBe(valueToMatch));
            isValid.ShouldBeFalse();
        }

        [Test]
        public void Validate_ControlTypeDateTextbox_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = ((int)Enums.TextboxDataTypes.Date).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)Enums.ControlType.TextBox
                }
            };
            var valueToMatch = DateTime.UtcNow.ToString();

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNullOrWhiteSpace(),
                 () => result.ShouldBe(valueToMatch));
            isValid.ShouldBeTrue();
        }

        [Test]
        public void Validate_ControlTypeTextArea_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = "SampleContent"
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)Enums.ControlType.TextArea
                }
            };
            var valueToMatch = "SampleContent";

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.ShouldBe(valueToMatch));
            isValid.ShouldBeTrue();
        }

        [Test]
        public void Validate_ControlTypeCustom_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = ((int)Enums.TextboxDataTypes.Custom).ToString() // pattern is "8"
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)Enums.ControlType.TextBox
                }
            };
            var valueToMatch = "8";

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNullOrWhiteSpace(),
                 () =>result.ShouldBe(valueToMatch));
            isValid.ShouldBeTrue();
        }

        [Test]
        public void Validate_ControlTypeListCheckbox_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = bool.TrueString.ToLower()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)Enums.ControlType.ListBox
                }
            };
            var valueToMatch = bool.TrueString.ToLower();

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () =>result.ShouldBe(valueToMatch));
            isValid.ShouldBeTrue();
        }

        [Test]
        public void Validate_ControlTypeGridValidationTypeAtleastOnce_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = ((int)Enums.GridValidation.AtLeastOne).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.Grid
                }
            };
            var valueToMatch = "1_2_3_4,on";

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () =>result.ShouldBe("C|4"));
            isValid.ShouldBeTrue();
        }

        [Test]
        public void Validate_ControlTypeGridValidationTypeOneResponse_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = ((int)Enums.GridValidation.OneResponse).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.Grid
                }
            };
            var valueToMatch = "1_2_3_4,on";

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNullOrWhiteSpace(),
                 () =>result.ShouldBe("C|4"));
            isValid.ShouldBeTrue();
        }

        [Test]
        public void Validate_ControlTypeGridValidationTypeAtLeastOnePerLine_MatchesWithStoredValue()
        {
            // Arrange
            SetUpFakes();
            var result = string.Empty;
            var controlPropertyManager = new ControlPropertyManager();
            var control = new Control
            {
                FormControlProperties = new List<FormControlProperty>
                {
                    new FormControlProperty
                    {
                        ControlProperty_ID = 1,
                        Value = ((int)Enums.GridValidation.AtLeastOnePerLine).ToString()
                    }
                },
                ControlType = new ControlType
                {
                    MainType_ID = (int)KMEnums.ControlType.Grid
                }
            };
            var valueToMatch = "1_2_3_4,on";

            // Act
            var isValid = controlPropertyManager.Validate(control, valueToMatch, out result);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () =>result.ShouldBe("C|4"));
            isValid.ShouldBeTrue();
        }

        private void SetUpFakes()
        {
            SetWebConfigurationFakes();
            ShimMXValidate.Constructor = (a) =>  { };
            ShimMXValidate.StaticConstructor = () => { };
            ShimAPIRunnerBase.StaticConstructor = () => { };

            ShimControlPropertyDbManager.AllInstances.GetRequiredPropertyByControlControl = (cpm,c) => new ControlProperty
            {
                ControlProperty_Seq_ID = 1
            };
            ShimControlPropertyDbManager.AllInstances.GetPropertyByNameAndControlStringControl = (cpm, c, n) => new ControlProperty
            {
                ControlProperty_Seq_ID = 1
            };
            ShimDataTypePatternDbManager.AllInstances.GetPatternByTypeTextboxDataTypes = (t, c) => string.Empty;
        }

        private void SetWebConfigurationFakes()
        {
            var settings = new NameValueCollection();
            settings.Add(CssDirKey, string.Empty);
            settings.Add(ApiDomainKey, "http://www.km.com");
            settings.Add(ApiTimeoutKey, "2000");
            settings.Add(AspNetMxLevelKey, "5");
            settings.Add(BlockSubmitIfTimeoutKey, bool.FalseString.ToLower());
            ShimWebConfigurationManager.AppSettingsGet = () => settings;
        }
    }
}
