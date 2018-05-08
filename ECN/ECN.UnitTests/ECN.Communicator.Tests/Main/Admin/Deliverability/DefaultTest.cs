using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using AjaxControlToolkit.Fakes;
using ecn.communicator.admin.deliverability;
using ECN.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Admin.Deliverability
{
    [TestFixture]
    public class DefaultTest
    {
        private const int CustomerID = 2;
        private const string DeliverabilityCustomerKey = "DELIVERABILITY_CUST";
        private const string DeliverabilityIPKey = "DELIVERABILITY_IP";
        private const string CustomerName = "Customer Name";
        private const string SourceIP = "10.0.0.1";
        private const string HostName = "Host Name";

        private const string CustomerIDPropertyName = "CustomerID";
        private const string CustomerNamePropertyName = "CustomerName";
        private const string SourceIPPropertyName = "SourceIP";
        private const string HostNamePropertyName = "HostName";
        private const string BlastIDPropertyName = "BlastID";

        private const string BlastsGridViewName = "gvBlasts";
        private const string BlastsIPGridViewName = "gvBlastsIP";
        private const string SubBlastsGridViewName = "gvSubBlasts";
        private const string SubBlastsIPGridViewName = "gvSubBlastsIP";
        private const string PopupModalName = "mdlPopup";
        private const string PopupIPModalName = "mdlPopupIP";
        private const string IPSubRptHeaderLabelName = "lblIPSubRptHeader";
        private const string ViewDropDownName = "dropdownView";
        private const string ViewIPDropDownName = "dropdownViewIP";

        private _default _deliverability;
        private IDisposable _shimsContext;

        private KeyValuePair<string, int>[] CommonProperties
            => new KeyValuePair<string, int>[]
            {
                new KeyValuePair<string, int>("TotalSent", 3),
                new KeyValuePair<string, int>("SoftBounces", 3),
                new KeyValuePair<string, int>("SBPerc", 100),
                new KeyValuePair<string, int>("HardBounces", 3),
                new KeyValuePair<string, int>("HBPerc", 100),
                new KeyValuePair<string, int>("MailBlock", 3),
                new KeyValuePair<string, int>("MBPerc", 100),
                new KeyValuePair<string, int>("Complaint", 3),
                new KeyValuePair<string, int>("ComPerc", 100),
                new KeyValuePair<string, int>("OptOut", 3),
                new KeyValuePair<string, int>("OptOPerc", 100),
                new KeyValuePair<string, int>("MasterSupp", 3),
                new KeyValuePair<string, int>("MSPerc", 100)
            };

        private string[] NonCalculatedProperties
            => new string[]
            {
                "TotalSent",
                "SoftBounces",
                "HardBounces",
                "MailBlock",
                "Complaint",
                "OptOut",
                "MasterSupp"
            };

        [SetUp]
        public void SetUp()
        {
            _shimsContext = ShimsContext.Create();
            ShimGridView.AllInstances.DataBind = (grid) => { };
            ShimModalPopupExtender.AllInstances.Show = (popup) => { };
            HttpContext.Current = MockHelpers.FakeHttpContext();
            _deliverability = new _default();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void LnkReportCommand_ValidCustomerData_SetsDataSourceWithCustomerData()
        {
            // Arrange
            var queryResults = new DataTable();
            SetupLinkReportColumns(queryResults);
            SetupLinkReportRows(queryResults);

            HttpContext.Current.Session.Add(DeliverabilityCustomerKey, queryResults);
            ReflectionHelper.SetValue(_deliverability, SubBlastsGridViewName, new GridView());
            ReflectionHelper.SetValue(_deliverability, PopupModalName, new ModalPopupExtender());

            // Act
            var parameters = new object[] { null, new CommandEventArgs(string.Empty, CustomerID.ToString()) };
            ReflectionHelper.ExecuteMethod(_deliverability, "lnkReport_Command", parameters);

            // Assert
            var datasource = AssertOnGridViewAndGetDataSource(SubBlastsGridViewName);

            var additionalProperties =
                new KeyValuePair<string, object>[]
                {
                    new KeyValuePair<string, object>(CustomerIDPropertyName, CustomerID),
                    new KeyValuePair<string, object>(CustomerNamePropertyName, CustomerName),
                    new KeyValuePair<string, object>(BlastIDPropertyName, 1)
                };

            var properties = AssertOnDataSourceProperties(datasource, additionalProperties);
            AssertCommonProperties(datasource, properties);
            AssertAdditionalProperties(datasource, properties, additionalProperties);
        }

        [Test]
        public void LnkIPReportCommand_ValidCustomerData_SetsDataSourceWithCustomerData()
        {
            // Arrange
            var queryResults = new DataTable();
            SetupLinkIPReportColumns(queryResults);
            SetupLinkIPReportRows(queryResults);

            HttpContext.Current.Session.Add(DeliverabilityIPKey, queryResults);
            ReflectionHelper.SetValue(_deliverability, SubBlastsIPGridViewName, new GridView());
            ReflectionHelper.SetValue(_deliverability, IPSubRptHeaderLabelName, new Label());
            ReflectionHelper.SetValue(_deliverability, PopupIPModalName, new ModalPopupExtender());

            // Act
            var parameters = new object[] { null, new CommandEventArgs(string.Empty, SourceIP) };
            ReflectionHelper.ExecuteMethod(_deliverability, "lnkIPReport_Command", parameters);

            // Assert
            var datasource = AssertOnGridViewAndGetDataSource(SubBlastsIPGridViewName);

            var additionalProperties =
                new KeyValuePair<string, object>[]
                {
                    new KeyValuePair<string, object>(SourceIPPropertyName, SourceIP),
                    new KeyValuePair<string, object>(BlastIDPropertyName, 1),
                    new KeyValuePair<string, object>(CustomerNamePropertyName, CustomerName)
                };

            var properties = AssertOnDataSourceProperties(datasource, additionalProperties);
            AssertAdditionalProperties(datasource, properties, additionalProperties);
            AssertCommonProperties(datasource, properties);
        }

        [Test]
        public void GetDeliverabilityByBlast_ValidCustomerData_SetsDataSourceWithCustomerData()
        {
            // Arrange
            var queryResults = new DataTable();
            SetupDeliverabitlityByBlasatColumns(queryResults);
            SetupDeliverabilityByBlastRows(queryResults);

            HttpContext.Current.Session.Add(DeliverabilityCustomerKey, queryResults);
            ReflectionHelper.SetValue(_deliverability, BlastsGridViewName, new GridView() { ID = BlastsGridViewName });
            ReflectionHelper.SetValue(_deliverability, ViewDropDownName, new DropDownList());

            // Act
            var parameters = new object[] { true, string.Empty, string.Empty };
            ReflectionHelper.ExecuteMethod(_deliverability, "getDeliverability_byBlast", parameters);

            // Assert
            var datasource = AssertOnGridViewAndGetDataSource(BlastsGridViewName);

            var additionalProperties =
                new KeyValuePair<string, object>[]
                {
                    new KeyValuePair<string, object>(CustomerIDPropertyName, CustomerID),
                    new KeyValuePair<string, object>(CustomerNamePropertyName, CustomerName),
                };

            var properties = AssertOnDataSourceProperties(datasource, additionalProperties);
            AssertCommonProperties(datasource, properties);
            AssertAdditionalProperties(datasource, properties, additionalProperties);
        }

        [Test]
        public void GetDeliverabilityByIP_ValidCustomerData_SetsDataSourceWithCustomerData()
        {
            // Arrange
            var queryResults = new DataTable();
            SetupDeliverabilityByIPColumns(queryResults);
            SetupDeliverabilityByIPRows(queryResults);

            HttpContext.Current.Session.Add(DeliverabilityIPKey, queryResults);
            ReflectionHelper.SetValue(_deliverability, BlastsIPGridViewName, new GridView() { ID = BlastsIPGridViewName });
            ReflectionHelper.SetValue(_deliverability, ViewIPDropDownName, new DropDownList());

            // Act
            var parameters = new object[] { true, string.Empty, string.Empty };
            ReflectionHelper.ExecuteMethod(_deliverability, "getDeliverability_byIP", parameters);

            // Assert
            var datasource = AssertOnGridViewAndGetDataSource(BlastsIPGridViewName);

            var additionalProperties =
                new KeyValuePair<string, object>[]
                {
                    new KeyValuePair<string, object>(SourceIPPropertyName, SourceIP),
                    new KeyValuePair<string, object>(HostNamePropertyName, HostName)
                };

            var properties = AssertOnDataSourceProperties(datasource, additionalProperties);
            AssertAdditionalProperties(datasource, properties, additionalProperties);
            AssertCommonProperties(datasource, properties);
        }

        private PropertyInfo[] AssertOnDataSourceProperties(IList datasource, KeyValuePair<string, object>[] additionalProperties)
        {
            var expectedProperties = CommonProperties.Select(x => x.Key).ToList();
            expectedProperties.AddRange(additionalProperties.Select(x => x.Key));

            var properties = datasource[0].GetType().GetAllProperties(false);
            properties.ShouldSatisfyAllConditions(
                () => properties.ShouldNotBeNull(),
                () => properties.Select(x => x.Name).ShouldBe(expectedProperties, true),
                () => properties.Length.ShouldBe(expectedProperties.Count));

            return properties;
        }

        private IList AssertOnGridViewAndGetDataSource(string gridViewName)
        {
            var gridView = ReflectionHelper
                                .GetFieldInfoFromInstanceByName(_deliverability, gridViewName)
                                .GetValue(_deliverability) as GridView;
            gridView.ShouldNotBeNull();

            var datasource = gridView.DataSource as IList;
            datasource.ShouldSatisfyAllConditions(
                () => datasource.ShouldNotBeNull(),
                () => datasource.Count.ShouldBe(1));

            return datasource;
        }

        private void AssertAdditionalProperties(IList datasource, PropertyInfo[] properties, IEnumerable<KeyValuePair<string, object>> additionalProperties)
        {
            foreach (var property in additionalProperties)
            {
                var propertyInfo = properties.FirstOrDefault(x => x.Name == property.Key);
                propertyInfo.GetValue(datasource[0]).ShouldBe(property.Value);
            }
        }

        private void AssertCommonProperties(IList datasource, PropertyInfo[] properties)
        {
            foreach (var property in CommonProperties)
            {
                var propertyInfo = properties.FirstOrDefault(x => x.Name == property.Key);
                propertyInfo.GetValue(datasource[0]).ShouldBe(property.Value);
            }
        }

        private void SetupDeliverabilityByIPRows(DataTable datatable)
        {
            for (var i = 0; i < 3; i++)
            {
                var row = datatable.NewRow();
                row[SourceIPPropertyName] = SourceIP;
                row[HostNamePropertyName] = HostName;
                SetupCommonRows(row, 1);

                datatable.Rows.Add(row);
            }
        }

        private void SetupDeliverabilityByIPColumns(DataTable datatable)
        {
            datatable.Columns.Add(SourceIPPropertyName, typeof(string));
            datatable.Columns.Add(HostNamePropertyName, typeof(string));
            SetupCommonColumns(datatable);
        }

        private void SetupDeliverabilityByBlastRows(DataTable datatable)
        {
            for (var i = 0; i < 3; i++)
            {
                var row = datatable.NewRow();
                row[CustomerIDPropertyName] = CustomerID;
                row[CustomerNamePropertyName] = CustomerName;
                SetupCommonRows(row, 1);

                datatable.Rows.Add(row);
            }
        }

        private void SetupDeliverabitlityByBlasatColumns(DataTable datatable)
        {
            datatable.Columns.Add(CustomerIDPropertyName, typeof(int));
            datatable.Columns.Add(CustomerNamePropertyName, typeof(string));
            SetupCommonColumns(datatable);
        }

        private void SetupLinkReportColumns(DataTable datatable)
        {
            datatable.Columns.Add(CustomerIDPropertyName, typeof(int));
            datatable.Columns.Add(CustomerNamePropertyName, typeof(string));
            datatable.Columns.Add(BlastIDPropertyName, typeof(int));
            SetupCommonColumns(datatable);
        }

        private void SetupCommonColumns(DataTable datatable)
        {
            foreach (var property in NonCalculatedProperties)
            {
                datatable.Columns.Add(property, typeof(int));
            }
        }

        private void SetupLinkIPReportRows(DataTable datatable)
        {
            for (var i = 0; i < 3; i++)
            {
                var row = datatable.NewRow();
                row[SourceIPPropertyName] = SourceIP;
                row[BlastIDPropertyName] = 1;
                row[CustomerNamePropertyName] = CustomerName;
                SetupCommonRows(row, 1);

                datatable.Rows.Add(row);
            }
        }

        private void SetupLinkIPReportColumns(DataTable datatable)
        {
            datatable.Columns.Add(SourceIPPropertyName, typeof(string));
            datatable.Columns.Add(BlastIDPropertyName, typeof(int));
            datatable.Columns.Add(CustomerNamePropertyName, typeof(string));
            SetupCommonColumns(datatable);
        }

        private void SetupLinkReportRows(DataTable datatable)
        {
            for (var i = 0; i < 3; i++)
            {
                var row = datatable.NewRow();
                row[CustomerIDPropertyName] = CustomerID;
                row[CustomerNamePropertyName] = CustomerName;
                row[BlastIDPropertyName] = 1;
                SetupCommonRows(row, 1);

                datatable.Rows.Add(row);
            }
        }

        private void SetupCommonRows(DataRow row, int val)
        {
            foreach(var property in NonCalculatedProperties)
            {
                row[property] = val;
            }
        }
    }
}
