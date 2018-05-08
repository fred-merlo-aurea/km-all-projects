using System;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using AUT.ConfigureTestProjects.Analyzer;
using AUT.ConfigureTestProjects.Extensions;
using ecn.accounts.customersmanager;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Main.Customers
{
    [TestFixture]
    public class LicenseDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : MethodCallTest

        #region General Method Call : Class (licensedetail) => Method (getCLID) (Return Type :  int) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_getCLID_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
            var licensedetail  = new licensedetail();

            // Act, Assert
            Should.NotThrow(() => licensedetail.getCLID());
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_getCLID_Method_With_No_Parameters_2_Calls_Compare_Test()
        {
            // Arrange
            var licensedetail  = new licensedetail();

            // Act
            Func<int> getClId1 = () => licensedetail.getCLID();
            Func<int> getClId2 = () => licensedetail.getCLID();
            var result1 = getClId1();
            var result2 = getClId2();
            var target1 = getClId1.Target;
            var target2 = getClId2.Target;

            // Assert
            getClId1.ShouldNotBeNull();
            getClId2.ShouldNotBeNull();
            getClId1.ShouldNotBe(getClId2);
            target1.ShouldBe(target2);
            Should.NotThrow(() => getClId1.Invoke());
            Should.NotThrow(() => getClId2.Invoke());
            result1.ShouldBe(result2);
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_getCLID_Method_With_No_Parameters_Call_With_Reflection_Reflection_Return_Data_Test()
        {
            // Arrange
            Object[] parametersOfgetClid = {};
            System.Exception exception, exception1, exception2;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "getCLID";

            if(licensedetail != null)
            {
                // Act
                var getClIdMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var getClIdMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = getClIdMethodInfo1.ReturnType;
                var returnType2 = getClIdMethodInfo2.ReturnType;
                var result1 = getClIdMethodInfo1.GetResultMethodInfo<licensedetail, int>(licensedetail, out exception1, parametersOfgetClid);
                var result2 = getClIdMethodInfo2.GetResultMethodInfo<licensedetail, int>(licensedetail, out exception2, parametersOfgetClid);

                // Assert
                parametersOfgetClid.ShouldNotBeNull();
                licensedetail.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                getClIdMethodInfo1.ShouldNotBeNull();
                getClIdMethodInfo2.ShouldNotBeNull();
                getClIdMethodInfo1.ShouldBe(getClIdMethodInfo2);
                if(exception1 == null)
                {
                    result1.ShouldBe(result2);
                    Should.NotThrow(() => getClIdMethodInfo1.Invoke(licensedetail, parametersOfgetClid));
                    Should.NotThrow(() => getClIdMethodInfo2.Invoke(licensedetail, parametersOfgetClid));
                }
                else
                {
                    result1.ShouldBeNull();
                    result2.ShouldBeNull();
                    exception1.ShouldNotBeNull();
                    exception2.ShouldNotBeNull();
                    Should.Throw(() => getClIdMethodInfo1.Invoke(licensedetail, parametersOfgetClid), exceptionType: exception1.GetType());
                    Should.Throw(() => getClIdMethodInfo2.Invoke(licensedetail, parametersOfgetClid), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_getCLID_Method_With_No_Parameters_Call_With_Reflection_Test()
        {
            // Arrange
            System.Exception exception, exception1, exception2;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "getCLID";

            if(licensedetail != null)
            {
                // Act
                var getClIdMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var getClIdMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = getClIdMethodInfo1.ReturnType;
                var returnType2 = getClIdMethodInfo2.ReturnType;
                var result1 = getClIdMethodInfo1.GetResultMethodInfo<licensedetail, int>(licensedetail, out exception1);
                var result2 = getClIdMethodInfo2.GetResultMethodInfo<licensedetail, int>(licensedetail, out exception2);

                // Assert
                licensedetail.ShouldNotBeNull();
                getClIdMethodInfo1.ShouldNotBeNull();
                getClIdMethodInfo2.ShouldNotBeNull();
                getClIdMethodInfo1.ShouldBe(getClIdMethodInfo2);
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                if(exception1 == null)
                {
                    result1.ShouldBe(result2);
                    Should.NotThrow(() => getClIdMethodInfo1.Invoke(licensedetail, null));
                    Should.NotThrow(() => getClIdMethodInfo2.Invoke(licensedetail, null));
                }
                else
                {
                    result1.ShouldBeNull();
                    result2.ShouldBeNull();
                    Should.Throw(() => getClIdMethodInfo1.Invoke(licensedetail, null), exceptionType: exception1.GetType());
                    Should.Throw(() => getClIdMethodInfo2.Invoke(licensedetail, null), exceptionType: exception2.GetType());
                }
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_getCLID_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            Object[] parametersOutRanged = {null, null};
            System.Exception exception;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "getCLID";

            if(licensedetail != null)
            {
                // Act
                var getClIdMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var getClIdMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = getClIdMethodInfo1.ReturnType;
                var returnType2 = getClIdMethodInfo2.ReturnType;

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licensedetail.ShouldNotBeNull();
                getClIdMethodInfo1.ShouldNotBeNull();
                getClIdMethodInfo2.ShouldNotBeNull();
                getClIdMethodInfo1.ShouldBe(getClIdMethodInfo2);
                Should.Throw<System.Exception>(() => getClIdMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<System.Exception>(() => getClIdMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getClIdMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => getClIdMethodInfo2.Invoke(licensedetail, parametersOutRanged));
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (licensedetail) => Method (LoadChannelDDfromBaseChannel) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_LoadChannelDDfromBaseChannel_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfLoadChannelDDfromBaseChannel = {sender, e};
            System.Exception exception, invokeException;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "LoadChannelDDfromBaseChannel";

            // Act
            var loadChannelDDfromBaseChannelMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
            var loadChannelDDfromBaseChannelMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
            var returnType1 = loadChannelDDfromBaseChannelMethodInfo1.ReturnType;
            var returnType2 = loadChannelDDfromBaseChannelMethodInfo2.ReturnType;

            // Assert
            parametersOfLoadChannelDDfromBaseChannel.ShouldNotBeNull();
            licensedetail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            loadChannelDDfromBaseChannelMethodInfo1.ShouldNotBeNull();
            loadChannelDDfromBaseChannelMethodInfo2.ShouldNotBeNull();
            loadChannelDDfromBaseChannelMethodInfo1.ShouldBe(loadChannelDDfromBaseChannelMethodInfo2);
            if(loadChannelDDfromBaseChannelMethodInfo1.DoesInvokeThrow(licensedetail, out invokeException, parametersOfLoadChannelDDfromBaseChannel))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOfLoadChannelDDfromBaseChannel), exceptionType: invokeException.GetType());
                Should.Throw(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOfLoadChannelDDfromBaseChannel), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOfLoadChannelDDfromBaseChannel));
                Should.NotThrow(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOfLoadChannelDDfromBaseChannel));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_LoadChannelDDfromBaseChannel_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "LoadChannelDDfromBaseChannel";

            if(licensedetail != null)
            {
                // Act
                var loadChannelDDfromBaseChannelMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var loadChannelDDfromBaseChannelMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = loadChannelDDfromBaseChannelMethodInfo1.ReturnType;
                var returnType2 = loadChannelDDfromBaseChannelMethodInfo2.ReturnType;
                loadChannelDDfromBaseChannelMethodInfo1.InvokeMethodInfo(licensedetail, out exception1, parametersOutRanged);
                loadChannelDDfromBaseChannelMethodInfo2.InvokeMethodInfo(licensedetail, out exception2, parametersOutRanged);
                loadChannelDDfromBaseChannelMethodInfo1.InvokeMethodInfo(licensedetail, out exception3, parametersInDifferentNumber);
                loadChannelDDfromBaseChannelMethodInfo2.InvokeMethodInfo(licensedetail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licensedetail.ShouldNotBeNull();
                loadChannelDDfromBaseChannelMethodInfo1.ShouldNotBeNull();
                loadChannelDDfromBaseChannelMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadChannelDDfromBaseChannelMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => loadChannelDDfromBaseChannelMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (licensedetail) => Method (LoadCustomersDDfromChannels) (Return Type :  void) Test



        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_LoadCustomersDDfromChannels_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfLoadCustomersDDfromChannels = {sender, e};
            System.Exception exception, invokeException;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "LoadCustomersDDfromChannels";

            // Act
            var loadCustomersDDfromChannelsMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
            var loadCustomersDDfromChannelsMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
            var returnType1 = loadCustomersDDfromChannelsMethodInfo1.ReturnType;
            var returnType2 = loadCustomersDDfromChannelsMethodInfo2.ReturnType;

            // Assert
            parametersOfLoadCustomersDDfromChannels.ShouldNotBeNull();
            licensedetail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            loadCustomersDDfromChannelsMethodInfo1.ShouldNotBeNull();
            loadCustomersDDfromChannelsMethodInfo2.ShouldNotBeNull();
            loadCustomersDDfromChannelsMethodInfo1.ShouldBe(loadCustomersDDfromChannelsMethodInfo2);
            if(loadCustomersDDfromChannelsMethodInfo1.DoesInvokeThrow(licensedetail, out invokeException, parametersOfLoadCustomersDDfromChannels))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOfLoadCustomersDDfromChannels), exceptionType: invokeException.GetType());
                Should.Throw(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOfLoadCustomersDDfromChannels), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOfLoadCustomersDDfromChannels));
                Should.NotThrow(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOfLoadCustomersDDfromChannels));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_LoadCustomersDDfromChannels_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "LoadCustomersDDfromChannels";

            if(licensedetail != null)
            {
                // Act
                var loadCustomersDDfromChannelsMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var loadCustomersDDfromChannelsMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = loadCustomersDDfromChannelsMethodInfo1.ReturnType;
                var returnType2 = loadCustomersDDfromChannelsMethodInfo2.ReturnType;
                loadCustomersDDfromChannelsMethodInfo1.InvokeMethodInfo(licensedetail, out exception1, parametersOutRanged);
                loadCustomersDDfromChannelsMethodInfo2.InvokeMethodInfo(licensedetail, out exception2, parametersOutRanged);
                loadCustomersDDfromChannelsMethodInfo1.InvokeMethodInfo(licensedetail, out exception3, parametersInDifferentNumber);
                loadCustomersDDfromChannelsMethodInfo2.InvokeMethodInfo(licensedetail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licensedetail.ShouldNotBeNull();
                loadCustomersDDfromChannelsMethodInfo1.ShouldNotBeNull();
                loadCustomersDDfromChannelsMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => loadCustomersDDfromChannelsMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => loadCustomersDDfromChannelsMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (licensedetail) => Method (CreateLicense) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_CreateLicense_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfCreateLicense = {sender, e};
            System.Exception exception, invokeException;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "CreateLicense";

            // Act
            var createLicenseMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
            var createLicenseMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
            var returnType1 = createLicenseMethodInfo1.ReturnType;
            var returnType2 = createLicenseMethodInfo2.ReturnType;

            // Assert
            parametersOfCreateLicense.ShouldNotBeNull();
            licensedetail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createLicenseMethodInfo1.ShouldNotBeNull();
            createLicenseMethodInfo2.ShouldNotBeNull();
            createLicenseMethodInfo1.ShouldBe(createLicenseMethodInfo2);
            if(createLicenseMethodInfo1.DoesInvokeThrow(licensedetail, out invokeException, parametersOfCreateLicense))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOfCreateLicense), exceptionType: invokeException.GetType());
                Should.Throw(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOfCreateLicense), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOfCreateLicense));
                Should.NotThrow(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOfCreateLicense));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_CreateLicense_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "CreateLicense";

            if(licensedetail != null)
            {
                // Act
                var createLicenseMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var createLicenseMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = createLicenseMethodInfo1.ReturnType;
                var returnType2 = createLicenseMethodInfo2.ReturnType;
                createLicenseMethodInfo1.InvokeMethodInfo(licensedetail, out exception1, parametersOutRanged);
                createLicenseMethodInfo2.InvokeMethodInfo(licensedetail, out exception2, parametersOutRanged);
                createLicenseMethodInfo1.InvokeMethodInfo(licensedetail, out exception3, parametersInDifferentNumber);
                createLicenseMethodInfo2.InvokeMethodInfo(licensedetail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licensedetail.ShouldNotBeNull();
                createLicenseMethodInfo1.ShouldNotBeNull();
                createLicenseMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => createLicenseMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => createLicenseMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #region General Method Call : Class (licensedetail) => Method (UpdateLicense) (Return Type :  void) Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_UpdateLicense_Method_2_Parameters_Simple_Call_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            var licensedetail  = new licensedetail();

            // Act, Assert
            Should.Throw<System.Exception>(() => licensedetail.UpdateLicense(sender, e));
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_UpdateLicense_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOfUpdateLicense = {sender, e};
            System.Exception exception, invokeException;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "UpdateLicense";

            // Act
            var updateLicenseMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
            var updateLicenseMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
            var returnType1 = updateLicenseMethodInfo1.ReturnType;
            var returnType2 = updateLicenseMethodInfo2.ReturnType;

            // Assert
            parametersOfUpdateLicense.ShouldNotBeNull();
            licensedetail.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            updateLicenseMethodInfo1.ShouldNotBeNull();
            updateLicenseMethodInfo2.ShouldNotBeNull();
            updateLicenseMethodInfo1.ShouldBe(updateLicenseMethodInfo2);
            if(updateLicenseMethodInfo1.DoesInvokeThrow(licensedetail, out invokeException, parametersOfUpdateLicense))
            {
                invokeException.ShouldNotBeNull();
                Should.Throw(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOfUpdateLicense), exceptionType: invokeException.GetType());
                Should.Throw(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOfUpdateLicense), exceptionType: invokeException.GetType());
            }
            else
            {
                invokeException.ShouldBeNull();
                Should.NotThrow(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOfUpdateLicense));
                Should.NotThrow(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOfUpdateLicense));
            }
        }


        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void licensedetail_UpdateLicense_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            Object[] parametersOutRanged = {sender, e, null};
            Object[] parametersInDifferentNumber = {sender};
            System.Exception exception, exception1, exception2, exception3, exception4;
            var licensedetail  = CreateAnalyzer.CreateOrReturnStaticInstance<licensedetail>(Fixture, out exception);
            var methodName = "UpdateLicense";

            if(licensedetail != null)
            {
                // Act
                var updateLicenseMethodInfo1 = licensedetail.GetType().GetMethod(methodName);
                var updateLicenseMethodInfo2 = licensedetail.GetType().GetMethod(methodName);
                var returnType1 = updateLicenseMethodInfo1.ReturnType;
                var returnType2 = updateLicenseMethodInfo2.ReturnType;
                updateLicenseMethodInfo1.InvokeMethodInfo(licensedetail, out exception1, parametersOutRanged);
                updateLicenseMethodInfo2.InvokeMethodInfo(licensedetail, out exception2, parametersOutRanged);
                updateLicenseMethodInfo1.InvokeMethodInfo(licensedetail, out exception3, parametersInDifferentNumber);
                updateLicenseMethodInfo2.InvokeMethodInfo(licensedetail, out exception4, parametersInDifferentNumber);

                // Assert
                parametersOutRanged.ShouldNotBeNull();
                parametersInDifferentNumber.ShouldNotBeNull();
                returnType1.ShouldNotBeNull();
                returnType2.ShouldNotBeNull();
                returnType1.ShouldBe(returnType2);
                licensedetail.ShouldNotBeNull();
                updateLicenseMethodInfo1.ShouldNotBeNull();
                updateLicenseMethodInfo2.ShouldNotBeNull();
                if(exception1 != null)
                {
                    Should.Throw(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                if(exception1 != null)
                {
                    Should.Throw(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged), exceptionType: exception1.GetType());
                    Should.Throw(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged), exceptionType: exception2.GetType());
                }
                else
                {
                    Should.NotThrow(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                    Should.NotThrow(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                }

                Should.Throw<System.Exception>(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<System.Exception>(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersOutRanged));
                Should.Throw<TargetParameterCountException>(() => updateLicenseMethodInfo1.Invoke(licensedetail, parametersInDifferentNumber));
                Should.Throw<TargetParameterCountException>(() => updateLicenseMethodInfo2.Invoke(licensedetail, parametersInDifferentNumber));
            }
            else
            {
                // Act, Assert
                licensedetail.ShouldBeNull();
                exception.ShouldNotBeNull();
            }
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (licensedetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_licensedetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new licensedetail());
        }

        #endregion

        #region General Constructor : Class (licensedetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_licensedetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstlicensedetail = new licensedetail();
            var secondlicensedetail = new licensedetail();
            var thirdlicensedetail = new licensedetail();
            var fourthlicensedetail = new licensedetail();
            var fifthlicensedetail = new licensedetail();
            var sixthlicensedetail = new licensedetail();

            // Act, Assert
            firstlicensedetail.ShouldNotBeNull();
            secondlicensedetail.ShouldNotBeNull();
            thirdlicensedetail.ShouldNotBeNull();
            fourthlicensedetail.ShouldNotBeNull();
            fifthlicensedetail.ShouldNotBeNull();
            sixthlicensedetail.ShouldNotBeNull();
            firstlicensedetail.ShouldNotBeSameAs(secondlicensedetail);
            thirdlicensedetail.ShouldNotBeSameAs(firstlicensedetail);
            fourthlicensedetail.ShouldNotBeSameAs(firstlicensedetail);
            fifthlicensedetail.ShouldNotBeSameAs(firstlicensedetail);
            sixthlicensedetail.ShouldNotBeSameAs(firstlicensedetail);
            sixthlicensedetail.ShouldNotBeSameAs(fourthlicensedetail);
        }

        #endregion

        #endregion

        #endregion
    }
}