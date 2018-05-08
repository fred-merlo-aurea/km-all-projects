using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports.CrystalReport;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports.CrystalReport
{
    [TestFixture]
    public class ReportDiskMonitorTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_DiskMonitor_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstrptDiskMonitor = new rpt_DiskMonitor();
            var secondrptDiskMonitor = new rpt_DiskMonitor();
            var thirdrptDiskMonitor = new rpt_DiskMonitor();
            var fourthrptDiskMonitor = new rpt_DiskMonitor();
            var fifthrptDiskMonitor = new rpt_DiskMonitor();
            var sixthrptDiskMonitor = new rpt_DiskMonitor();

            // Act, Assert
            firstrptDiskMonitor.ShouldNotBeNull();
            secondrptDiskMonitor.ShouldNotBeNull();
            thirdrptDiskMonitor.ShouldNotBeNull();
            fourthrptDiskMonitor.ShouldNotBeNull();
            fifthrptDiskMonitor.ShouldNotBeNull();
            sixthrptDiskMonitor.ShouldNotBeNull();
            firstrptDiskMonitor.ShouldNotBeSameAs(secondrptDiskMonitor);
            thirdrptDiskMonitor.ShouldNotBeSameAs(firstrptDiskMonitor);
            fourthrptDiskMonitor.ShouldNotBeSameAs(firstrptDiskMonitor);
            fifthrptDiskMonitor.ShouldNotBeSameAs(firstrptDiskMonitor);
            sixthrptDiskMonitor.ShouldNotBeSameAs(firstrptDiskMonitor);
            sixthrptDiskMonitor.ShouldNotBeSameAs(fourthrptDiskMonitor);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_rpt_DiskMonitor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(action: () => new rpt_DiskMonitor());
        }
    }
}