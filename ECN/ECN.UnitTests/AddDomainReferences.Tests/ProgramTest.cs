using System;
using System.IO;
using System.IO.Fakes;
using System.Reflection;
using System.Text;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ECN.TestHelpers;
using AddDomainReferences.Fakes;

namespace AddDomainReferences.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        private IDisposable _shimObject;
        private const string _MethodProcessFile = "ProcessFile";

        private void FillByteArray(byte[] byteArray)
        {
            using (var memoryStream = new MemoryStream())
            {
                var streamWriter = new StreamWriter(memoryStream);
                var shimStreamWriter = new ShimStreamWriter();
                var outFileFieldName = "outFile";
                var outFileField = typeof(Program).GetField(
                    outFileFieldName,
                    BindingFlags.Public | BindingFlags.Static);
                outFileField.SetValue(null, streamWriter);
                memoryStream.Read(byteArray, 0, memoryStream.Capacity);
            }
        }

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void ProcessFile_OnEmptyFile_ReachEnd()
        {
            // Arrange
            var fileName = "TestFileName";
            ShimStreamReader.ConstructorString = (obj, str) => { };
            ShimStreamReader.AllInstances.ReadLine = (obj) => null;
            var byteArr = new byte[0];
            FillByteArray(byteArr);

            // Act	
            typeof(Program).CallMethod(_MethodProcessFile, new object[] { fileName }, null);
            var actualResult = Encoding.UTF8.GetString(byteArr);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void ProcessFile_GetDomainAndGetIp_ReachEnd()
        {
            // Arrange
            var fileName = "TestFileName";
            var addedToDB = false;
            ShimStreamReader.ConstructorString = (obj, str) => { };
            var lineNumbers = 0;
            ShimStreamReader.AllInstances.ReadLine = (obj) =>
            {
                lineNumbers++;
                if (lineNumbers == 3)
                {
                    return " TO: test@test.com";
                }
                else if (lineNumbers == 5)
                {
                    return "250 Email Sent Successfully to server [10.1.1.1] ";
                }
                else if (lineNumbers == 502)
                {
                    return null;
                }
                return string.Empty;
            };
            var byteArr = new byte[0];
            FillByteArray(byteArr);
            ShimProgram.AddToDBString = (str) => 
            {
                addedToDB = true;
            };

            // Act	
            typeof(Program).CallMethod(_MethodProcessFile, new object[] { fileName }, null);
            var actualResult = Encoding.UTF8.GetString(byteArr);

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
            addedToDB.ShouldBeTrue();
        }
    }
}
