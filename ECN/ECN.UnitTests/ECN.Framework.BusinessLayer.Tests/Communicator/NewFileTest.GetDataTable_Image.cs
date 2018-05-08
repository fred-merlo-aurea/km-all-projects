using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using System.Linq;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.Entity;
using NUnit.Framework;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using KM.Platform.Fakes;
using Shouldly;
using ECNEntities = ECN_Framework_Entities.Communicator;
using KMPlatformFakes = KMPlatform.BusinessLogic.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class NewFileTest
    {
        [Test]
        public void GetDataTable_Image_NullDirectory_ReturnEmptyDataTable()
        {
            // Arrange
            ShimDirectory.GetFilesStringString = (str, search) => new string[0];

            // Act	
            var actualResult = NewFile.GetDataTable_Image(DummyString, DummyString, DummyString, null, null, DummyString);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows.ShouldNotBeNull(),
                () => actualResult.Rows.Count.ShouldBe(0)
            );
        }

        [Test]
        public void GetDataTable_Image_OnException_ReturnEmptyDataTable()
        {
            // Arrange
            ShimDirectory.GetFilesStringString = (str, search) => new[] { DummyString, DummyString };
            ShimFileInfo.ConstructorString = (obj, str) => { };
            ShimFileInfo.AllInstances.NameGet = (obj) => $"{DummyString}.jpg";

            // Act	
            var actualResult = NewFile.GetDataTable_Image(DummyString, DummyString, DummyString, null, null, DummyString);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows.ShouldNotBeNull(),
                () => actualResult.Rows.Count.ShouldBe(0)
            );
        }

        [Test]
        [TestCase("jpg")]
        [TestCase("gif")]
        [TestCase("png")]
        public void GetDataTable_Image_ImgListViewRBIsNull_ReturnDataTable(string extension)
        {
            // Arrange
            ShimDirectory.GetFilesStringString = (str, search) => new[] { $"{DummyString}.{extension}", $"{DummyString}.{extension}" };
            ShimFileInfo.ConstructorString = (obj, str) => { };
            ShimFileInfo.AllInstances.NameGet = (obj) => $"{DummyString}.{extension}";
            ShimFileSystemInfo.AllInstances.LastWriteTimeGet = (obj) => DateTime.Now;
            ShimHttpContext.CurrentGet =
                () => new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));

            // Act	
            var actualResult = NewFile.GetDataTable_Image(DummyString, DummyString, DummyString, null, null, DummyString);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows.ShouldNotBeNull(),
                () => actualResult.Rows.Count.ShouldBe(2),
                () => actualResult.Rows[0][1].ShouldNotBeNull(),
                () => actualResult.Rows[0][1].ToString().ShouldContain(DummyString)
            );
        }

        [Test]
        [TestCase("jpg", 2000)]
        [TestCase("gif", 1024)]
        [TestCase("png", 1000)]
        public void GetDataTable_Image_ImgListViewRBNotNull_ReturnDataTable(string extension, long fileLength)
        {
            // Arrange
            ShimDirectory.GetFilesStringString = (str, search) => new[] { $"{DummyString}.{extension}", $"{DummyString}.{extension}" };
            ShimFileInfo.ConstructorString = (obj, str) => { };
            ShimFileInfo.AllInstances.NameGet = (obj) => $"{DummyString}.{extension}";
            ShimFileInfo.AllInstances.LengthGet = (obj) => fileLength;
            ShimFileSystemInfo.AllInstances.LastWriteTimeGet = (obj) => DateTime.Now;
            ShimHttpContext.CurrentGet =
                () => new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
            var imgListViewRB = new RadioButtonList
            {
                Items = { new ListItem(DummyString, "DETAILS") },
                SelectedIndex =  0
            };

            // Act	
            var actualResult = NewFile.GetDataTable_Image(
                DummyString,
                DummyString,
                DummyString,
                null,
                imgListViewRB,
                DummyString);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Rows.ShouldNotBeNull(),
                () => actualResult.Rows.Count.ShouldBe(2),
                () => actualResult.Rows[0][0].ShouldNotBeNull(),
                () => actualResult.Rows[0][0].ToString().ShouldContain(DummyString)
            );
        }
    }
}
