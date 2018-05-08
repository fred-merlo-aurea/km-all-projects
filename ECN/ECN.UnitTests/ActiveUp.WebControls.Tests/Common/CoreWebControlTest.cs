using ActiveUp.WebControls.Common;
using ActiveUp.WebControls.Common.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveUp.WebControls.Tests.Common
{
    [TestClass]
    public class CoreWebControlTest
    {
        private const string ImageDummyDirectory1 = @"C:\temp1";
        private const string ImageDummyDirectory2 = @"C:\temp2";

        private const string ScriptDummyDirectory1 = @"C:\dummy1";
        private const string ScriptDummyDirectory2 = @"C:\dummy2";

        private const string DummyExternalScript1 = @"ActivePanel.js";
        private const string DummyExternalScript2 = @"PanelGroup.js";

        [TestMethod]
        public void ImagesDirectory_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            //Create the control, start tracking viewstate, then set a new Text value.
            var coreWebControl = new CoreWebControl();
            coreWebControl.ImagesDirectory = ImageDummyDirectory1;
            ((IControl) coreWebControl).TrackViewState();
            coreWebControl.ImagesDirectory = ImageDummyDirectory2;

            //Save the control's state
            var viewState = ((IControl) coreWebControl).SaveViewState();

            // Act:
            //Create a new control instance and load the state
            //back into it, overriding any existing values
            var control = new CoreWebControl();
            coreWebControl.ImagesDirectory = ImageDummyDirectory1;
            ((IControl) control).LoadViewState(viewState);

            // Assert:
            Assert.AreEqual(ImageDummyDirectory2, control.ImagesDirectory,
                "Value restored from viewstate does not match the original value we set");
        }

        [TestMethod]
        public void ScriptDirectory_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            //Create the control, start tracking viewstate, then set a new Text value.
            var coreWebControl = new CoreWebControl();
            coreWebControl.ScriptDirectory = ScriptDummyDirectory1;
            ((IControl)coreWebControl).TrackViewState();
            coreWebControl.ScriptDirectory = ScriptDummyDirectory2;

            //Save the control's state
            var viewState = ((IControl)coreWebControl).SaveViewState();

            // Act:
            //Create a new control instance and load the state
            //back into it, overriding any existing values
            var control = new CoreWebControl();
            coreWebControl.ScriptDirectory = ScriptDummyDirectory1;
            ((IControl)control).LoadViewState(viewState);

            // Assert:
            Assert.AreEqual(ScriptDummyDirectory2, control.ScriptDirectory,
                "Value restored from viewstate does not match the original value we set");
        }

        [TestMethod]
        public void ExternalScript_SetAndGetValue_ReturnsTheSetValue()
        {
            // Arrange:
            //Create the control, start tracking viewstate, then set a new Text value.
            var coreWebControl = new CoreWebControl();
            coreWebControl.ExternalScript = DummyExternalScript1;
            ((IControl)coreWebControl).TrackViewState();
            coreWebControl.ExternalScript = DummyExternalScript2;

            //Save the control's state
            var viewState = ((IControl)coreWebControl).SaveViewState();

            // Act:
            //Create a new control instance and load the state
            //back into it, overriding any existing values
            var control = new CoreWebControl();
            coreWebControl.ExternalScript = DummyExternalScript1;
            ((IControl)control).LoadViewState(viewState);

            // Assert:
            Assert.AreEqual(DummyExternalScript2, control.ExternalScript,
                "Value restored from viewstate does not match the original value we set");
        }
    }
}
