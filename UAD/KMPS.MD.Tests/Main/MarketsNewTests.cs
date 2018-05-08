using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Fakes;
using KM.Platform.Fakes;
using KMPlatform;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Web.UI;

namespace KMPS.MD.Tests.Main
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MarketsNewTests : BasePageTests
    {
        private MarketsNew _testEntity;
        private DropDownList _pubTypesList;
        private DropDownList _masterGroupsList;
        private string _xml = string.Empty;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new MarketsNew();
            InitializePage(_testEntity);

            _pubTypesList = (DropDownList)PrivatePage.GetField("ddlPubTypes");
            _pubTypesList.Items.Add("1");
            _pubTypesList.SelectedValue = "1";

            _masterGroupsList = (DropDownList)PrivatePage.GetField("ddlMasterGroups");
            _masterGroupsList.Items.Add("1");
            _masterGroupsList.SelectedValue = "1";
            
            ShimXmlDocument.AllInstances.LoadString = (doc, _) =>
            {
                doc.LoadXml("<Market>" +
                            "   <PubTypes>" +
                            "       <PubType ID=\"1\">" +
                            "           <Pub ID=\"1\"></Pub>" +
                            "           <Pub ID=\"2\"></Pub>" +
                            "           <Pub ID=\"3\"></Pub>" +
                            "       </PubType>" +
                            "       <PubType ID=\"2\">" +
                            "           <Pub ID=\"1\"></Pub>" +
                            "       </PubType>" +
                            "   </PubTypes>" +
                            "   <Dimensions>" +
                            "       <MasterGroup ID=\"1\">" +
                            "           <Entry ID=\"1\"></Entry>" +
                            "           <Entry ID=\"2\"></Entry>" +
                            "           <Entry ID=\"3\"></Entry>" +
                            "       </MasterGroup>" +
                            "       <MasterGroup ID=\"2\">" +
                            "           <Entry ID=\"1\"></Entry>" +
                            "       </MasterGroup>" +
                            "   </Dimensions>" +
                            "</Market>");
            };

            ShimXmlDocument.AllInstances.SaveString = (doc, _) => { _xml = doc.OuterXml; };
        }

        [Test]
        public void RemovePubFromXML_RemoveItems_XmlHasItemsRemoved()
        {
            // Arrange
            var itemsToRemove = new ListItemCollection
            {
                "1",
                "2"
            };

            // Act
            PrivatePage.Invoke("RemovePubFromXML", itemsToRemove);

            // Assert
            _xml.ShouldBe("<Market>" +
                         "<PubTypes>" +
                         "<PubType ID=\"1\">" +
                         "<Pub ID=\"3\"></Pub>" + //Removed IDs 1 and 2
                         "</PubType>" +
                         "<PubType ID=\"2\">" +
                         "<Pub ID=\"1\"></Pub>" +
                         "</PubType>" +
                         "</PubTypes>" +
                         "<Dimensions>" +
                         "<MasterGroup ID=\"1\">" +
                         "<Entry ID=\"1\"></Entry>" +
                         "<Entry ID=\"2\"></Entry>" +
                         "<Entry ID=\"3\"></Entry>" +
                         "</MasterGroup>" +
                         "<MasterGroup ID=\"2\">" +
                         "<Entry ID=\"1\"></Entry>" +
                         "</MasterGroup>" +
                         "</Dimensions>" +
                         "</Market>");
        }

        [Test]
        public void RemoveDimensionFromXML_RemoveItems_XmlHasItemsRemoved()
        {
            // Arrange
            var itemsToRemove = new ListItemCollection
            {
                "1",
                "2"
            };

            // Act
            PrivatePage.Invoke("RemoveDimensionFromXML", itemsToRemove);

            // Assert
            _xml.ShouldBe("<Market>" +
                         "<PubTypes>" +
                         "<PubType ID=\"1\">" +
                         "<Pub ID=\"1\"></Pub>" +
                         "<Pub ID=\"2\"></Pub>" +
                         "<Pub ID=\"3\"></Pub>" +
                         "</PubType>" +
                         "<PubType ID=\"2\">" +
                         "<Pub ID=\"1\"></Pub>" +
                         "</PubType>" +
                         "</PubTypes>" +
                         "<Dimensions>" +
                         "<MasterGroup ID=\"1\">" +
                         "<Entry ID=\"3\"></Entry>" + //Removed Entry 1 and 2
                         "</MasterGroup>" +
                         "<MasterGroup ID=\"2\">" +
                         "<Entry ID=\"1\"></Entry>" +
                         "</MasterGroup>" +
                         "</Dimensions>" +
                         "</Market>");
        }
    }
}
