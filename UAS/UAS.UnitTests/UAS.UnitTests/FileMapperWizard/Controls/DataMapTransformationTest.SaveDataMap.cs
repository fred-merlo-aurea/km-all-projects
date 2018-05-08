using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Xceed.Wpf.Toolkit;

namespace UAS.UnitTests.FileMapperWizard.Controls
{
    public partial class DataMapTransformationTest
    {
        [Test]
        public void SaveDataMap_WithlstPubCodeEmpty_SavePubMapAndDataMap()
        {
            // Arrange 
            SetSaveDataMapFakes();

            // Act
            var result = (bool)_privateTestObj.Invoke(SaveDataMapMethodName);

            // Assert
            result.ShouldBeTrue();
            _isDataMapSaved.ShouldBeTrue();
            _isPubMapSaved.ShouldBeTrue();
            _savedPubMap.ShouldNotBeNull();
            _savedDataMap.ShouldNotBeNull();
            _savedDataMap.MatchType.ShouldBe(SampleType);
            _savedDataMap.DesiredData.ShouldBe(SampleDesiredData);
            _savedDataMap.SourceData.ShouldBe(SampleSourceData);
            _savedDataMap.PubID.ShouldBe(-1);
            _savedDataMap.IsActive.ShouldBeTrue();
            _savedPubMap.IsActive.ShouldBeTrue();
            _savedPubMap.PubID.ShouldBe(-1);
        }

        [Test]
        public void SaveDataMap_WithlstPubCodeHasValue_SavesTransformDataMapandPubMap()
        {
            // Arrange 
            SetSaveDataMapFakes();
            var spSingleDataMap = (StackPanel)_privateTestObj.GetFieldOrProperty(SpSingleDataMapField);
            foreach (var item in spSingleDataMap.Children)
            {
                var privateDataMap = new PrivateObject(item);
                var lstPubCode = (CheckComboBox)privateDataMap.GetFieldOrProperty(ListPubCodeField);
                lstPubCode.ItemsSource = new Dictionary<int, string> {  [1] = "TestPubCode"  };
                lstPubCode.SelectedMemberPath = "Key";
                lstPubCode.DisplayMemberPath = "Value";
                lstPubCode.SelectedItems.Add(lstPubCode.Items[0]);
            }

            // Act
            var result = (bool)_privateTestObj.Invoke(SaveDataMapMethodName);

            // Assert
            result.ShouldBeTrue();
            _isDataMapSaved.ShouldBeTrue();
            _isPubMapSaved.ShouldBeTrue();
            _savedPubMap.ShouldNotBeNull();
            _savedDataMap.ShouldNotBeNull();
            _savedDataMap.MatchType.ShouldBe(SampleType);
            _savedDataMap.DesiredData.ShouldBe(SampleDesiredData);
            _savedDataMap.SourceData.ShouldBe(SampleSourceData);
            _savedDataMap.PubID.ShouldBe(1);
            _savedDataMap.IsActive.ShouldBeTrue();
            _savedPubMap.IsActive.ShouldBeTrue();
            _savedPubMap.PubID.ShouldBe(1);
        }

        [Test]
        public void SaveDataMap_WithOverriedPubCodeCheckAndMessageBoxResultNo_ReturnsWithOutSave ()
        {
            // Arrange 
            var isMessageBoxShown = false;
            var message = string.Empty;
            SetSaveDataMapFakes();
            var spSingleDataMap = (StackPanel)_privateTestObj.GetFieldOrProperty(SpSingleDataMapField);
            foreach (DataMap item in spSingleDataMap.Children)
            {
                var privateDataMap = new PrivateObject(item);
                var myList = (Dictionary<int,string>)privateDataMap.GetFieldOrProperty(MyListField);
                myList.Remove(1);
            }
            ShimMessageBox.ShowStringStringMessageBoxButton = (msg, s, b) => 
            {
                isMessageBoxShown = true;
                message = msg;
                return MessageBoxResult.No;
            };

            // Act
            var result = (bool)_privateTestObj.Invoke(SaveDataMapMethodName);

            // Assert
            result.ShouldBeFalse();
            isMessageBoxShown.ShouldBeTrue();    
            message.ShouldNotBeNullOrWhiteSpace();
            message.ShouldContain("Pub Code was not selected.");
        }

        [Test]
        public void SaveDataMap_WithOverriedPubCodeCheckAndMessageBoxTrue_ReturnsWithMessage()
        {
            // Arrange 
            var spSingleDataMap = (StackPanel)_privateTestObj.GetFieldOrProperty(SpSingleDataMapField);
            foreach (DataMap item in spSingleDataMap.Children)
            {
                var privateDataMap = new PrivateObject(item);
                var myList = (Dictionary<int, string>)privateDataMap.GetFieldOrProperty(MyListField);
                myList.Remove(1);
            }
            ShimMessageBox.ShowStringStringMessageBoxButton = (m, s, b) => MessageBoxResult.Yes;

            // Act
            var result = (bool)_privateTestObj.Invoke(SaveDataMapMethodName);

            // Assert
            result.ShouldBeFalse();
            _messageBoxMessage.ShouldNotBeNullOrEmpty();
            _messageBoxMessage.ShouldContain("Please select a match type before saving.");
        }

        [Test]
        public void SaveDataMap_WithNoStackPanelChildren_ReturnsWithMessage()
        {
            // Arrange 
            SetSaveDataMapFakes();
            var spSingleDataMap = (StackPanel)_privateTestObj.GetFieldOrProperty(SpSingleDataMapField);
            spSingleDataMap.Children.RemoveAt(0);

            // Act
            var result = (bool)_privateTestObj.Invoke(SaveDataMapMethodName);

            // Assert
            result.ShouldBeFalse();
            _messageBoxMessage.ShouldNotBeNullOrEmpty();
            _messageBoxMessage.ShouldContain("Nothing to save. Please add data mapping.");
        }

        [Test]
        public void SaveDataMap_WithMatchTypeEmpty_ReturnsWithMessage()
        {
            // Act
            var result = (bool)_privateTestObj.Invoke(SaveDataMapMethodName);

            // Assert
            result.ShouldBeFalse();
            _messageBoxMessage.ShouldNotBeNullOrEmpty();
            _messageBoxMessage.ShouldContain("Please select a match type before saving.");
        }

        private void SetSaveDataMapFakes()
        {
            ShimDataMap.AllInstances.ButtonTagGet = (b) => "0";
            ShimDataMap.AllInstances.MatchTypeGet = (b) => SampleType;
            ShimDataMap.AllInstances.SourceDataGet = (b) => SampleSourceData;
            ShimDataMap.AllInstances.DesireDataGet = (b) => SampleDesiredData;   
        }
    }
}
