﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrameworkServices.Fakes;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ReportLibrary.Reports;
using Shouldly;
using UAS.UnitTests.ReportLibrary.Common;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using FrameworkUASService = FrameworkUAS.Service;

namespace UAS.UnitTests.ReportLibrary
{
    /// <summary>
    /// Unit test for <see cref="GeoDomesticBreakDown"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class GeoDomesticBreakDownTest
    {
        private const string GeoTable = "GeoTable";
        private const string Domestic = "Domestic";
        private const string Total = "Total";
        private const string FieldsCopies = "= IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0)";
        private const string QualifiedPercentage = "= CDbl( CDbl(IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0))/Exec(\"Qualified " +
    "Percent\",CDbl(Sum(Fields.Copies))))";
        private const string NonQualifiedPercent = "= CDbl( CDbl(IIf(Sum(Fields.Copies) > 0, Sum(Fields.Copies), 0))/Exec(\"NonQualifi" +
        "ed Percent\",CDbl(Sum(Fields.Copies))))";
        private const int ItemsCount = 48;
        private GeoDomesticBreakDown _geoDomesticBreakDown;
        private PrivateObject _privateObject;
        protected IDisposable _shimObject;
        private bool LogCriticalError = false;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void InitializeComponent_LoadDefaultValue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = false;
            CreateClassObject();
            var result = _geoDomesticBreakDown;

            // Act
            var geoTable = (Telerik.Reporting.Table)_privateObject.GetField(GeoTable);

            // Assert
            result.ShouldNotBeNull();
            geoTable.ShouldSatisfyAllConditions(
                () => geoTable.ShouldNotBeNull(),
                () => geoTable.DataSource.ShouldBeNull(),
                () => geoTable.Items.Count.ShouldBe(ItemsCount),
                () => geoTable.ColumnGroups.Count.ShouldBe(5),
                () => geoTable.RowGroups.Count.ShouldBe(5),
                () => geoTable.Items.Count.ShouldBe(ItemsCount),
                () => geoTable.Visible.ShouldBeTrue()
            );
            result.ShouldSatisfyAllConditions(
                () => result.Items.Count.ShouldBe(3),
                () => result.StyleSheet.Count.ShouldBe(5),
                () => result.ReportParameters.Count.ShouldBe(7),
                () => result.Action.ShouldBeNull(),
                () => result.Bindings.Count.ShouldBe(0),
                () => result.PageSettings.ShouldNotBeNull(),
                () => result.Report.ShouldNotBeNull()
            );
            AssertMethodResult(result);

        }

        [Test]
        public void InitializeComponent_ReportUtilitiesDebugIsTrue_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            ReportUtilities.Debug = true;

            // Act
            CreateClassObject();

            // Assert
            AssertMethodResult(_geoDomesticBreakDown);
            Get<Telerik.Reporting.SqlDataSource>(Domestic).ShouldNotBeNull();
        }

        [Test]
        public void GeoDomesticBreakDown_LoadPageControlThrowException_LogCriticalErrorForApplication()
        {
            // Arrange
            CreateClassObject();
            var parameters = new object[] { this, EventArgs.Empty };
            LogCriticalErrorForApplication();
            CreateAuthorizedUser();

            // Act
            _privateObject.Invoke(CommonHelper.GetParameters, parameters);

            // Assert
            LogCriticalError.ShouldBeTrue();

        }

        private void CreateAuthorizedUser()
        {
            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
            {
                return new UserAuthorization
                {
                    User = new User
                    {
                        CurrentClient = new Client
                        {
                            ClientConnections = new KMPlatform.Object.ClientConnections()
                        }
                    }
                };
            };
        }

        private void LogCriticalErrorForApplication()
        {
            ShimServiceClient.UAS_ApplicationLogClient = () =>
            {
                return new ShimServiceClient<IApplicationLog>
                {
                    ProxyGet = () =>
                    {
                        return new StubIApplicationLog()
                        {
                            LogCriticalErrorGuidStringStringEnumsApplicationsStringInt32 =
                            (accessKey, ex, sourceMethod, application, note, clientId) =>
                            {
                                LogCriticalError = true;
                                return new FrameworkUASService.Response<int>
                                {
                                    Message = string.Empty,
                                    ProcessCode = string.Empty,
                                    Result = 1,
                                    Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error
                                };
                            }
                        };
                    }
                };
            };
        }

        private object GetTextbox(string tbName)
        {
            var textBox = (Telerik.Reporting.TextBox)_privateObject.GetFieldOrProperty(tbName);
            var txt = textBox.Value;
            return txt;
        }
        private T Get<T>(string propName)
        {
            var val = (T)_privateObject.GetFieldOrProperty(propName);
            return val;
        }

        private void CreateClassObject()
        {
            _geoDomesticBreakDown = new GeoDomesticBreakDown();
            _privateObject = new PrivateObject(_geoDomesticBreakDown);
        }

        private void AssertMethodResult(GeoDomesticBreakDown result)
        {
            result.ShouldSatisfyAllConditions(
                () => GetTextbox(CommonHelper.TextBox1).ShouldBe("= Fields.CategoryCodeTypeName"),
                () => GetTextbox(CommonHelper.TextBox2).ShouldBe("= Fields.[State & Zip Code]"),
                () => GetTextbox(CommonHelper.TextBox3).ShouldBe("State & Zip Code"),
                () => GetTextbox(CommonHelper.TextBox4).ShouldBe("Region"),
                () => GetTextbox(CommonHelper.TextBox5).ShouldBe("Grand Total:"),
                () => GetTextbox(CommonHelper.TextBox6).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox7).ShouldBe("Total"),
                () => GetTextbox(CommonHelper.TextBox8).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox9).ShouldBe("= Fields.Region"),
                () => GetTextbox(CommonHelper.TextBox10).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox11).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox12).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox13).ShouldBe("Total"),
                () => GetTextbox(CommonHelper.TextBox14).ShouldBe(NonQualifiedPercent),
                () => GetTextbox(CommonHelper.TextBox15).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox16).ShouldBe("= Fields.[Category Type] + \" Percent\""),
                () => GetTextbox(CommonHelper.TextBox17).ShouldBe(QualifiedPercentage),
                () => GetTextbox(CommonHelper.TextBox18).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox19).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox20).ShouldBe("= Fields.[Category Type] +  \" Percent\""),
                () => GetTextbox(CommonHelper.TextBox21).ShouldBe(NonQualifiedPercent),
                () => GetTextbox(CommonHelper.TextBox22).ShouldBe(QualifiedPercentage),
                () => GetTextbox(CommonHelper.TextBox23).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox24).ShouldBe("=Now()"),
                () => GetTextbox(CommonHelper.TextBox25).ShouldBe("As of Date: "),
                () => GetTextbox(CommonHelper.TextBox26).ShouldBe("=Parameters.IssueName"),
                () => GetTextbox(CommonHelper.TextBox27).ShouldBe("Issue:"),
                () => GetTextbox(CommonHelper.TextBox28).ShouldBe("= Parameters.ProductName.Value"),
                () => GetTextbox(CommonHelper.TextBox29).ShouldBe("For Product: "),
                () => GetTextbox(CommonHelper.TextBox30).ShouldBe("= Fields.Demo7"),
                () => GetTextbox(CommonHelper.TextBox31).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox33).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox46).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox53).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox57).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox60).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox64).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox67).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox69).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox70).ShouldBe("=Fields.CountryRecap"),
                () => GetTextbox(CommonHelper.TextBox71).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox72).ShouldBe("=Sum(Fields.Copies)"),
                () => GetTextbox(CommonHelper.TextBox73).ShouldBe(FieldsCopies),
                () => GetTextbox(CommonHelper.TextBox74).ShouldBe(QualifiedPercentage),
                () => GetTextbox(CommonHelper.TextBox75).ShouldBe(NonQualifiedPercent),
                () => GetTextbox(CommonHelper.TextBox76).ShouldBe("=Sum(Fields.Copies)"),
                () => GetTextbox(CommonHelper.TextBox84).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox85).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox86).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox87).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox88).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox89).ShouldBe(string.Empty),
                () => GetTextbox(CommonHelper.TextBox90).ShouldBe(string.Empty)
            );
        }
    }
}