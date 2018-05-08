using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.reports;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Reports
{
    [TestFixture]
    public class BlastStatusTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastStatus) => Property (Master) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastStatus_Class_Invalid_Property_MasterNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMaster = "MasterNotPresent";
            var blastStatus  = new BlastStatus();

            // Act , Assert
            Should.NotThrow(() => blastStatus.GetType().GetProperty(propertyNameMaster));
        }

        #endregion

        #endregion

        #endregion
    }
}