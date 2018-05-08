using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ActiveUp.WebControls.Tests.Helper.TestsHelper;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ActiveUp.WebControls.Tests.ActiveCalendar
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CalendarTest
    {
        private const string DummyCalendarId = "DummyCalendar";
        private const string DummyImagePath = "dummy.png";
        private const string DummyImageDirectory = "c://temp";

        [Test]
        public void ShowNextPreviousMonthYear_PreviousMonth_AddTdAndImgAttribute()
        {
            // Arrange:
            var calendar = new Mock<Calendar>();
            calendar.Object.ID = DummyCalendarId;
            calendar.Object.ClientIDMode = ClientIDMode.Static;
            calendar.Object.PrevMonthImage = DummyImagePath;
            calendar.SetupGet(x => x.ImagesDirectory).Returns(DummyImageDirectory);

            var output = new HtmlTextWriter(new StringWriter());
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            var argumentForRefParameter = new object[] { CalendarNextPreviousType.PreviousMonth, output };
            var privateObject = new PrivateObject(calendar.Object, new PrivateType(typeof(Calendar)));
           
            // Act:
            privateObject.Invoke("ShowNextPreviousMonthYear", argumentForRefParameter);
            output.RenderEndTag();

            // Assert:
            var outputTextWriter = argumentForRefParameter[1] as HtmlTextWriter;
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<div>"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<td"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<img"));
        }

        [Test]
        public void ShowNextPreviousMonthYear_PreviousYear_AddTdAndImgAttribute()
        {
            // Arrange:
            var calendar = new Mock<Calendar>();
            calendar.Object.ID = DummyCalendarId;
            calendar.Object.ClientIDMode = ClientIDMode.Static;
            calendar.Object.PrevYearImage = DummyImagePath;
            calendar.SetupGet(x => x.ImagesDirectory).Returns(DummyImageDirectory);

            var output = new HtmlTextWriter(new StringWriter());
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            var argumentForRefParameter = new object[] { CalendarNextPreviousType.PreviousYear, output };
            var privateObject = new PrivateObject(calendar.Object, new PrivateType(typeof(Calendar)));

            // Act:
            privateObject.Invoke("ShowNextPreviousMonthYear", argumentForRefParameter);
            output.RenderEndTag();

            // Assert:
            var outputTextWriter = argumentForRefParameter[1] as HtmlTextWriter;
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<div>"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<td"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<img"));
        }

        [Test]
        public void ShowNextPreviousMonthYear_NextMonth_AddTdAttributeAndNotImg()
        {
            // Arrange:
            var calendar = new Mock<Calendar>();
            calendar.Object.ID = DummyCalendarId;
            calendar.Object.ClientIDMode = ClientIDMode.Static;
            calendar.SetupGet(x => x.ImagesDirectory).Returns(DummyImageDirectory);

            var output = new HtmlTextWriter(new StringWriter());
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            var argumentForRefParameter = new object[] { CalendarNextPreviousType.NextMonth, output };
            var privateObject = new PrivateObject(calendar.Object, new PrivateType(typeof(Calendar)));

            // Act:
            privateObject.Invoke("ShowNextPreviousMonthYear", argumentForRefParameter);
            output.RenderEndTag();

            // Assert:
            var outputTextWriter = argumentForRefParameter[1] as HtmlTextWriter;
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<div>"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<td"));
            Assert.IsFalse(outputTextWriter.InnerWriter.ToString().Contains("<img"));
        }

        [Test]
        public void ShowNextPreviousMonthYear_NextYear_AddTdAndImgAttribute()
        {
            // Arrange:
            var calendar = new Mock<Calendar>();
            calendar.Object.ID = DummyCalendarId;
            calendar.Object.ClientIDMode = ClientIDMode.Static;
            calendar.Object.NextYearImage = DummyImagePath;
            calendar.SetupGet(x => x.ImagesDirectory).Returns(DummyImageDirectory);

            var output = new HtmlTextWriter(new StringWriter());
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            var argumentForRefParameter = new object[] { CalendarNextPreviousType.NextYear, output };
            var privateObject = new PrivateObject(calendar.Object, new PrivateType(typeof(Calendar)));

            // Act:
            privateObject.Invoke("ShowNextPreviousMonthYear", argumentForRefParameter);
            output.RenderEndTag();

            // Assert:
            var outputTextWriter = argumentForRefParameter[1] as HtmlTextWriter;
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<div>"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<td"));
            Assert.IsTrue(outputTextWriter.InnerWriter.ToString().Contains("<img"));
        }

		[Test]
		public void ImagesDirectory_DefaultValue()
		{
			// Arrange
			var testObject = new Calendar();

			// Act, Assert
			AssertNotFX1(string.Empty, testObject.ImagesDirectory);
			AssertFX1(Define.IMAGES_DIRECTORY, testObject.ImagesDirectory);
		}

		[Test]
		public void ImagesDirectory_SetAndGetValue()
		{
			// Arrange
			var testObject = new Calendar();

			// Act
			testObject.ImagesDirectory = DummyImageDirectory;

			// Assert
			testObject.ImagesDirectory.ShouldBe(DummyImageDirectory);
		}

        [TestCase(true)]
        [TestCase(false)]
        public void AutoDetectSsl_SetValue_ReturnsSetValue(bool value)
        {
            // Arrange
            using (var testObject = new Calendar())
            {
                // Act
                testObject.AutoDetectSsl = value;

                // Assert
                testObject.AutoDetectSsl.ShouldBe(value);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EnableSsl_SetValue_ReturnsSetValue(bool value)
        {
            // Arrange
            using (var testObject = new Calendar())
            {
                // Act
                testObject.EnableSsl = value;

                // Assert
                testObject.EnableSsl.ShouldBe(value);
            }
        }
    }
}
