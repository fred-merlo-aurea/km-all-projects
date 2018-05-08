using System;
using System.Reflection;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.collector.main.Content;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Collector.Tests.Main.Content
{
    [TestFixture]
    public  class ContentEditorTest : AbstractGenericTest
    {
        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_CreateContent_Method_No_Parameters_2_Calls_Test()
        {
            // Arrange
            var contentEditor  = new ContentEditor();

            // Act
            Func<int> createContent = () => contentEditor.CreateContent();

            // Assert
            Should.Throw<Exception>(actual: () => createContent.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_CreateContent_Method_No_Parameters_Simple_Call_Test()
        {
            // Arrange
            var contentEditor  = new ContentEditor();

            // Act, Assert
            Should.Throw<Exception>(actual: () => contentEditor.CreateContent());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_CreateContent_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var contentEditor  = new ContentEditor();
            var methodName = "CreateContent";

            // Act
            var createContentMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var createContentMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = createContentMethodInfo1.ReturnType;
            var returnType2 = createContentMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            createContentMethodInfo1.ShouldNotBeNull();
            createContentMethodInfo2.ShouldNotBeNull();
            createContentMethodInfo1.ShouldBe(createContentMethodInfo2);
            Should.Throw<Exception>(actual: () => createContentMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => createContentMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => createContentMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => createContentMethodInfo2.Invoke(contentEditor, parametersOutRanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_CreateSurveyBlast_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            var contentEditor  = new ContentEditor();

            // Act
            Action createSurveyBlast = () => contentEditor.CreateSurveyBlast(sender, e);

            // Assert
            Should.Throw<Exception>(actual: () => createSurveyBlast.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_CreateSurveyBlast_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            object[] parametersOutRanged = {sender, e, null};
            object[] parametersInDifferentNumber = {sender};
            var contentEditor  = new ContentEditor();
            var methodName = "CreateSurveyBlast";

            // Act
            var createSurveyBlastMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var createSurveyBlastMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = createSurveyBlastMethodInfo1.ReturnType;
            var returnType2 = createSurveyBlastMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            createSurveyBlastMethodInfo1.ShouldNotBeNull();
            createSurveyBlastMethodInfo2.ShouldNotBeNull();
            createSurveyBlastMethodInfo1.ShouldBe(createSurveyBlastMethodInfo2);
            Should.Throw<Exception>(actual: () => createSurveyBlastMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => createSurveyBlastMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => createSurveyBlastMethodInfo1.Invoke(contentEditor, parametersInDifferentNumber));
            Should.Throw<Exception>(actual: () => createSurveyBlastMethodInfo2.Invoke(contentEditor, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => createSurveyBlastMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => createSurveyBlastMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => createSurveyBlastMethodInfo1.Invoke(contentEditor, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => createSurveyBlastMethodInfo2.Invoke(contentEditor, parametersInDifferentNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_CreateSurveyBlast_Method_With_2_Parameters_Call_With_Reflection_No_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            object[] parametersOfCreateSurveyBlast = {sender, e};
            var contentEditor  = new ContentEditor();
            var methodName = "CreateSurveyBlast";

            // Act
            var createSurveyBlastMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var createSurveyBlastMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = createSurveyBlastMethodInfo1.ReturnType;
            var returnType2 = createSurveyBlastMethodInfo2.ReturnType;

            // Assert
            parametersOfCreateSurveyBlast.ShouldNotBeNull();
            contentEditor.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            createSurveyBlastMethodInfo1.ShouldNotBeNull();
            createSurveyBlastMethodInfo2.ShouldNotBeNull();
            createSurveyBlastMethodInfo1.ShouldBe(createSurveyBlastMethodInfo2);
            Should.Throw<Exception>(actual: () => createSurveyBlastMethodInfo1.Invoke(contentEditor, parametersOfCreateSurveyBlast));
            Should.Throw<Exception>(actual: () => createSurveyBlastMethodInfo2.Invoke(contentEditor, parametersOfCreateSurveyBlast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_GetTextFromHTML_Method_2_Parameters_2_Calls_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            var contentEditor  = new ContentEditor();

            // Act
            Action getTextFromHtml = () => contentEditor.GetTextFromHTML(sender, e);

            // Assert
            Should.Throw<Exception>(actual: () => getTextFromHtml.Invoke());
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_GetTextFromHTML_Method_With_2_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            var sender = Fixture.Create<object>();
            var e = Fixture.Create<EventArgs>();
            object[] parametersOutRanged = {sender, e, null};
            object[] parametersInDifferentNumber = {sender};
            var contentEditor  = new ContentEditor();
            var methodName = "GetTextFromHTML";

            // Act
            var getTextFromHtmlMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var getTextFromHtmlMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = getTextFromHtmlMethodInfo1.ReturnType;
            var returnType2 = getTextFromHtmlMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            parametersInDifferentNumber.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            getTextFromHtmlMethodInfo1.ShouldNotBeNull();
            getTextFromHtmlMethodInfo2.ShouldNotBeNull();
            getTextFromHtmlMethodInfo1.ShouldBe(getTextFromHtmlMethodInfo2);
            Should.Throw<Exception>(actual: () => getTextFromHtmlMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => getTextFromHtmlMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => getTextFromHtmlMethodInfo1.Invoke(contentEditor, parametersInDifferentNumber));
            Should.Throw<Exception>(actual: () => getTextFromHtmlMethodInfo2.Invoke(contentEditor, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => getTextFromHtmlMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => getTextFromHtmlMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => getTextFromHtmlMethodInfo1.Invoke(contentEditor, parametersInDifferentNumber));
            Should.Throw<TargetParameterCountException>(actual: () => getTextFromHtmlMethodInfo2.Invoke(contentEditor, parametersInDifferentNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_LoadFoldersDR_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var contentEditor  = new ContentEditor();
            var methodName = "LoadFoldersDR";

            // Act
            var loadFoldersDrMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var loadFoldersDrMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = loadFoldersDrMethodInfo1.ReturnType;
            var returnType2 = loadFoldersDrMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            loadFoldersDrMethodInfo1.ShouldNotBeNull();
            loadFoldersDrMethodInfo2.ShouldNotBeNull();
            loadFoldersDrMethodInfo1.ShouldBe(loadFoldersDrMethodInfo2);
            Should.Throw<Exception>(actual: () => loadFoldersDrMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => loadFoldersDrMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => loadFoldersDrMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => loadFoldersDrMethodInfo2.Invoke(contentEditor, parametersOutRanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_LoadUsersDR_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var contentEditor  = new ContentEditor();
            var methodName = "LoadUsersDR";

            // Act
            var loadUsersDrMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var loadUsersDrMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = loadUsersDrMethodInfo1.ReturnType;
            var returnType2 = loadUsersDrMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            loadUsersDrMethodInfo1.ShouldNotBeNull();
            loadUsersDrMethodInfo2.ShouldNotBeNull();
            loadUsersDrMethodInfo1.ShouldBe(loadUsersDrMethodInfo2);
            Should.Throw<Exception>(actual: () => loadUsersDrMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => loadUsersDrMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => loadUsersDrMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => loadUsersDrMethodInfo2.Invoke(contentEditor, parametersOutRanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_Reset_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var contentEditor  = new ContentEditor();
            var methodName = "Reset";

            // Act
            var resetMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var resetMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = resetMethodInfo1.ReturnType;
            var returnType2 = resetMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            resetMethodInfo1.ShouldNotBeNull();
            resetMethodInfo2.ShouldNotBeNull();
            resetMethodInfo1.ShouldBe(resetMethodInfo2);
            Should.Throw<Exception>(actual: () => resetMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => resetMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => resetMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => resetMethodInfo2.Invoke(contentEditor, parametersOutRanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT MethodCallTest")]
        public void ContentEditor_setShowPane_Method_With_No_Parameters_Call_With_Reflection_Exception_Thrown_Test()
        {
            // Arrange
            object[] parametersOutRanged = {null, null};
            var contentEditor  = new ContentEditor();
            var methodName = "setShowPane";

            // Act
            var setShowPaneMethodInfo1 = contentEditor.GetType().GetMethod(methodName);
            var setShowPaneMethodInfo2 = contentEditor.GetType().GetMethod(methodName);
            var returnType1 = setShowPaneMethodInfo1.ReturnType;
            var returnType2 = setShowPaneMethodInfo2.ReturnType;

            // Assert
            parametersOutRanged.ShouldNotBeNull();
            returnType1.ShouldNotBeNull();
            returnType2.ShouldNotBeNull();
            returnType1.ShouldBe(returnType2);
            contentEditor.ShouldNotBeNull();
            setShowPaneMethodInfo1.ShouldNotBeNull();
            setShowPaneMethodInfo2.ShouldNotBeNull();
            setShowPaneMethodInfo1.ShouldBe(setShowPaneMethodInfo2);
            Should.Throw<Exception>(actual: () => setShowPaneMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<Exception>(actual: () => setShowPaneMethodInfo2.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => setShowPaneMethodInfo1.Invoke(contentEditor, parametersOutRanged));
            Should.Throw<TargetParameterCountException>(actual: () => setShowPaneMethodInfo2.Invoke(contentEditor, parametersOutRanged));
        }
    }
}