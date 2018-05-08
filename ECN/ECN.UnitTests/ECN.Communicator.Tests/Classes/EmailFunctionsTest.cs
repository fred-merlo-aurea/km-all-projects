using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.Fakes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using System.Xml;
using System.Xml.Fakes;
using ecn.communicator.classes;
using ecn.communicator.classes.Fakes;
using ecn.common.classes.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Communicator;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Common.Functions.Fakes;
using EmailPreview.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using AccountFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using ClassesDataFunctions = ecn.common.classes.DataFunctions;
using PlatformUserFake = KMPlatform.BusinessLogic.Fakes.ShimUser;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Classes
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class EmailFunctionsTest
	{
		private const string hashKey = "5-6-7-8-9-10-11-12-13";
		private const string emailSlots = "%%slot1%%";
		private const string sampleHtmlBody = "sample html body";
		private const string sampleTextBody = "sample text body";
		private const string dummyTableLayoutOptions = "dummy layout options";
		private const string dummyTemplateSource = "dummy template source";
		private const string dummyText = "dummyText";
		private const string dummyContent = "dummyContent";
		private const string dummyRowData = "dummyRowData";
		private const string dummyDynamicTag = "dummyDynamicTag";
		private const string taggedHashKey = hashKey + "-" + dummyDynamicTag;
		private const string staticHashKey = "oneemail-PERSONALIZEDBLAST";
		private IDisposable _context;
		private Type _emailFunctionsType;
		private EmailFunctions _emailFunctionsObject;
		private Blast _blast;
		private string _virtualPath;
		private string _hostName;
		private string _bounceDomain;
		private DataTable _emailTable;
		private bool _resend;
		private bool _testBlast;
		private string _forwardEmail;
		private bool _doSocialMedia;
		private int _filterID;
		private int _smartSegmentID;
		private int _customerID;
		private int _groupID;
		private string _blastID;

		[SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public void SendBlastForEmails_WhenBlastTypeIsChampion_EmailListIsCreatedAndSampleIsSaved()
		{
			// Arrage
			Initialize();
			var emailListCreated = false;
			var sampleSaved = false;
			_doSocialMedia = true;
			_blast.BlastType = "Champion";
			var methodArgs = GetMethodParameters("SendBlastForEmails");
			ShimEmailFunctions.AllInstances.CreateLayoutAndEmailsMappingBlastStringStringStringDataTableBooleanBooleanString = (a, b, c, d, e, f, g, h, i) => new Hashtable();
			ShimEmailFunctions.CreateEmailListBlastStringBoolean = (x, y, z) => new DataTable();
			ShimEmailFunctions.CreateEmailListBlastStringBoolean = (x, y, z) =>
			{
				emailListCreated = true;
				return new DataTable();
			};
			ShimSample.Save_NoAccessCheckSampleUser = (x, y) =>
			{
				sampleSaved = true;
			};

			//Act
			CallMethod(_emailFunctionsType, "SendBlastForEmails", methodArgs, _emailFunctionsObject);

			//Assert
			emailListCreated.ShouldSatisfyAllConditions(
				() => emailListCreated.ShouldBeTrue(),
				() => sampleSaved.ShouldBeTrue());
		}

		[Test]
		public void SendBlastForEmails_WhenResendIsFalse_SendTotalForBlastIsUpdated()
		{
			// Arrage
			Initialize();
			var sendTotalUpdated = false;
			_doSocialMedia = true;
			_blast.BlastType = "Champion";
			var methodArgs = GetMethodParameters("SendBlastForEmails");
			ShimEmailFunctions.CreateEmailListBlastStringBoolean = (x, y, z) => new DataTable();
			ShimEmailFunctions.AllInstances.CreateLayoutAndEmailsMappingBlastStringStringStringDataTableBooleanBooleanString = (a, b, c, d, e, f, g, h, i) => new Hashtable();
			ShimEmailFunctions.AllInstances.UpdateSendTotalInt32Int32 = (x, y, z) =>
			{
				sendTotalUpdated = true;
			};

			//Act
			CallMethod(_emailFunctionsType, "SendBlastForEmails", methodArgs, _emailFunctionsObject);

			//Assert
			sendTotalUpdated.ShouldBeTrue();
		}

		[Test]
		public void SendBlastForEmails_WhenCalled_EmailsBlastIsSent()
		{
			// Arrage
			Initialize();
			var emailBlastSent = false;
			_doSocialMedia = true;
			_blast.BlastType = "Champion";
			var methodArgs = GetMethodParameters("SendBlastForEmails");
			ShimEmailFunctions.CreateEmailListBlastStringBoolean = (x, y, z) => new DataTable();
			ShimEmailFunctions.AllInstances.CreateLayoutAndEmailsMappingBlastStringStringStringDataTableBooleanBooleanString = (a, b, c, d, e, f, g, h, i) => new Hashtable();
			ShimEmailFunctions.AllInstances.BlastHashtable = (x, y) =>
			{
				emailBlastSent = true;
			};

			//Act
			CallMethod(_emailFunctionsType, "SendBlastForEmails", methodArgs, _emailFunctionsObject);

			//Assert
			emailBlastSent.ShouldBeTrue();
		}

		[TestCase(true, false)]
		[TestCase(false, true)]
		public void CreateEmailList_WhenTestBlastIsSet_GetByBlastIDIsCalledOrNot(bool testBlast, bool isCalledGetByBlastID)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList");
			_blast.BlastType = "Personalization";
			_testBlast = testBlast;
			var _isCalledGetByBlastID = false;
			ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
			{
				_isCalledGetByBlastID = true;
				return new CampaignItem();
			};

			//Act
			EmailFunctions.CreateEmailList(_blast, _bounceDomain, _testBlast);

			//Assert
			_isCalledGetByBlastID.ShouldBe(isCalledGetByBlastID);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CreateEmailList_WhenStoredProcedureEmailListCountReturnsRows_EmailListIsCreated(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList");
			_testBlast = testBlast;
			var dummyCellValue = "dummy Cell Value";
			ShimDbDataAdapter.AllInstances.FillDataSetString = (x, dataset, z) =>
			{
				dataset.Tables.Add(FillDataSetDummyTable("dummyColumn", dummyCellValue));
				return 0;
			};

			//Act
			var emailList = EmailFunctions.CreateEmailList(_blast, _bounceDomain, _testBlast);

			//Assert
			emailList.ShouldSatisfyAllConditions(
				() => emailList.ShouldNotBeNull(),
				() => emailList.Rows.ShouldNotBeNull(),
				() => emailList.Rows[0].ItemArray.ShouldNotBeNull(),
				() => emailList.Rows[0].ItemArray.ShouldNotBeEmpty(),
				() =>
				{
					var resultCellValue = emailList.Rows[0].ItemArray[0];
					resultCellValue.ShouldBe(dummyCellValue);
				});
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CreateEmailList_CommunicatorTableContainsSeedsForCustomer_EmailListContainsTheseSeeds(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList");
			_testBlast = testBlast;
			ShimDataFunctions.GetDataTableStringSqlCommand = (x, y) => GetDataTableDummyTable();

			//Act
			var emailList = EmailFunctions.CreateEmailList(_blast, _bounceDomain, _testBlast);

			//Assert
			var dummyTableRowCount = GetDataTableDummyTable().Rows.Count;
			var emailListRowCount = emailList.Rows.Count;
			var emailListContainsSeeds = emailListRowCount > dummyTableRowCount;
			emailListContainsSeeds.ShouldBeTrue();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CreateEmailList_XmlDocumentHaveEmailAddressNode_EmailListContainsTheseNodes(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList");
			_testBlast = testBlast;
			ShimXmlNode.AllInstances.SelectSingleNodeString = (x, y) => DummyXmlNode();

			//Act
			var emailList = EmailFunctions.CreateEmailList(_blast, _bounceDomain, _testBlast);

			//Assert
			var dummyTableRowCount = GetDataTableDummyTable().Rows.Count;
			var emailListRowCount = emailList.Rows.Count;
			var emailListContainsNodes = emailListRowCount > dummyTableRowCount ? true : false;
			emailListContainsNodes.ShouldBeTrue();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CreateEmailList_WhenhasEmailPreviewIsTrue_CreateCustomerEmailTestFromEngineIsCalled(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList");
			_testBlast = testBlast;
			var emailPreviewCreated = false;
			ShimDataFunctions.ExecuteScalarString = (sqlQuery) => 1;
			ShimPreview.AllInstances.CreateCustomerEmailTestFromEngineInt32Int32 = (x, y, z) =>
			{
				emailPreviewCreated = true;
				return "sample String";
			};
			ShimPreview.GetSpamSeedAddresses = () =>
			{
				var sampleStringArr = new string[]
				{
					"sampleString1",
					"sampleString2",

				};
				return sampleStringArr;
			};

			//Act
			var emailList = EmailFunctions.CreateEmailList(_blast, _bounceDomain, _testBlast);

			//Assert
			emailPreviewCreated.ShouldBeTrue();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void GetBlastRemainingCount_WhenCalled_ReturnsRemainingBlastCount(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList");
			CreateShims("GetBlastRemainingCount");
			_testBlast = testBlast;
			var _remainingCount = "1057";
			ShimDbDataAdapter.AllInstances.FillDataSetString = (x, dataset, z) =>
			{
				dataset.Tables.Add(FillDataSetDummyTable("dummyColumn", _remainingCount));
				return 0;
			};
			ShimDataFunctions.GetDataTableString = (x) => new DataTable();

			//Act
			var remainingCount = EmailFunctions.GetBlastRemainingCount(_filterID, _smartSegmentID, _customerID, _groupID, _bounceDomain, _blastID, _testBlast).ToString();

			//Assert
			remainingCount.ShouldBe(_remainingCount);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CreateLayoutAndEmailsMapping_WithStaticContent_ReturnsStaticMapping(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList", false);
			CreateShims("CreateLayoutAndEmailsMapping", false);
			_testBlast = testBlast;
			_blast.BlastType = "Personalization";
			_blast.EnableCacheBuster = true;
			var columns = new string[] { "dummyColumn", "PersonalizedContentID", "BlastID", dummyDynamicTag };
			var rows = new string[][] {
				new string[] { dummyRowData, "1", "1", dummyDynamicTag },
				new string[] { dummyRowData, "2", "2", dummyDynamicTag }
			};
			CreateEmailTable(columns, rows);
			var methodArgs = GetMethodParameters("CreateLayoutAndEmailsMapping");
			ShimContent.GetTagsListOfStringBoolean = (x, y) => new List<string>();

			// Act
			var result = (Hashtable)CallMethod(_emailFunctionsType, "CreateLayoutAndEmailsMapping", methodArgs, _emailFunctionsObject, false);

			// Assert
			var mapping = result?[staticHashKey] as EmailBlast;
			var mappingContainsEmailTable = IfSimilarToEmailTable(mapping.EmailsTable);
			var isStatiContent = !(mapping.TEXT.Contains(dummyText));
			mapping.ShouldSatisfyAllConditions(
				() => isStatiContent.ShouldBeTrue(),
				() => mappingContainsEmailTable.ShouldBeTrue(),
				() => mapping.TEXT.Contains(dummyText).ShouldBeFalse(),
				() => mapping.HTML.Contains(dummyTableLayoutOptions).ShouldBeTrue(),
				() => mapping.HTML.Contains(dummyTemplateSource).ShouldBeTrue());
		}

		[TestCase(true)]
		[TestCase(false)]
		public void CreateLayoutAndEmailsMapping_WithDynamicContent_ReturnsDynamicMapping(bool testBlast)
		{
			// Arrage
			Initialize();
			CreateShims("CreateEmailList", false);
			CreateShims("CreateLayoutAndEmailsMapping", false);
			_testBlast = testBlast;
			_blast.BlastType = "Personalization";
			_blast.EnableCacheBuster = true;
			var columns = new string[] { "dummyColumn", "PersonalizedContentID", "BlastID", dummyDynamicTag };
			var rows = new string[][] {
				new string[] { dummyRowData, "1", "1", dummyDynamicTag },
				new string[] { dummyRowData, "2", "2", dummyDynamicTag }
			};
			CreateEmailTable(columns, rows);
			var methodArgs = GetMethodParameters("CreateLayoutAndEmailsMapping");
			ShimContent.GetTagsListOfStringBoolean = (x, y) => new List<string>()
			{
				dummyDynamicTag
			};
			
			// Act
			var result = (Hashtable)CallMethod(_emailFunctionsType, "CreateLayoutAndEmailsMapping", methodArgs, _emailFunctionsObject, false);

			// Assert
			var mapping = (EmailBlast)result[taggedHashKey];
			var mappingContainsEmailTable = IfSimilarToEmailTable(mapping.EmailsTable);
			var isDynamicContent = mapping.TEXT.Contains(dummyText);
			mapping.ShouldSatisfyAllConditions(
				() => isDynamicContent.ShouldBeTrue(),
				() => mappingContainsEmailTable.ShouldBeTrue(),
				() => mapping.TEXT.Contains(dummyText).ShouldBeTrue(),
				() => mapping.HTML.Contains(dummyTableLayoutOptions).ShouldBeTrue(),
				() => mapping.HTML.Contains(dummyTemplateSource).ShouldBeTrue());
		}

		private void Initialize()
		{
			_emailFunctionsType = typeof(EmailFunctions);
			_emailFunctionsObject = CreateInstance(_emailFunctionsType);
			_blast = CreateInstanceWithValues(typeof(Blast), new
			{
				BlastType = "Champion",
				TestBlast = "N"
			});
			_virtualPath = string.Empty;
			_hostName = string.Empty;
			_bounceDomain = string.Empty;
			_emailTable = new DataTable();
			_resend = false;
			_testBlast = false;
			_forwardEmail = string.Empty;
			_doSocialMedia = false;
			_filterID = 1;
			_smartSegmentID = 1;
			_customerID = 1;
			_groupID = 1;
			_blastID = "1";
			CreateShims();
		}

		private object[] GetMethodParameters(string methodName)
		{
			if (methodName == "GetBlastRemainingCount")
			{
				return new object[]
				{
					_filterID,
					_smartSegmentID,
					_customerID,
					_groupID,
					_bounceDomain,
					_blastID,
					_testBlast
				};
			}
			if (methodName == "SendBlastForEmails")
			{
				return new object[]
				{
					_blast,
					_virtualPath,
					_hostName,
					_bounceDomain,
					_emailTable,
					_resend,
					_testBlast,
					_forwardEmail,
					_doSocialMedia
				};
			}
			if (methodName == "CreateLayoutAndEmailsMapping")
			{
				return new object[]
				{
					_blast,
					_virtualPath,
					_hostName,
					_bounceDomain,
					_emailTable,
					_resend,
					_testBlast,
					_forwardEmail,
				};
			}
			else
			{
				return null;
			}
		}

		private void CreateShims()
		{
			ShimLoggingFunctions.LogStatistics = () => true;
			ShimBlast.GetSampleBlastUserBySampleIDInt32 = (x) => 0;
			PlatformUserFake.GetByUserIDInt32Boolean = (x, y) => new User();
			AccountFakes.ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => CreateInstance(typeof(Customer));
			AccountFakes.ShimBaseChannel.GetByBaseChannelIDInt32 = (x) => CreateInstance(typeof(BaseChannel));
			ShimClient.AllInstances.SelectInt32Boolean = (x, y, z) => CreateInstance(typeof(Client));
			ShimClientGroup.AllInstances.SelectInt32Boolean = (x, y, z) => CreateInstance(typeof(ClientGroup));
			ShimSecurityGroup.AllInstances.SelectInt32Int32BooleanBoolean = (a, b, c, d, e) => CreateInstance(typeof(SecurityGroup));
			ShimSample.GetBySampleID_NoAccessCheckInt32User = (x, y) => CreateInstance(typeof(Sample));
			ShimBlastActivity.ChampionByProc_NoAccessCheckInt32Int32BooleanString = (a, b, c, d) => ChampionByProc_NoAccessCheckDummyTable();
			ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => BlastAbstractMockObject();
			ShimBlast.GetBySampleID_NoAccessCheckInt32Boolean = (x, y) => BlastAbstractMockList();
			ShimBlastActivity.GetABSampleCountInt32Int32 = (x, y) => AbSampleCountDummyTable();
			ShimChampionAudit.InsertChampionAuditUser = (x, y) => 0;
			ShimSample.Save_NoAccessCheckSampleUser = (x, y) => { };
			ShimEmailFunctions.AllInstances.DoSocialMediaInt32Int32Int32 = (a, b, c, d) => { };
			ShimEmailFunctions.AllInstances.UpdateSendTotalInt32Int32 = (x, y, z) => { };
			ShimDataFunctions.ExecuteString = (x) => 0;
		}

		private void CreateShims(string methodName, bool allShims = true)
		{
			if (methodName == "CreateEmailList")
			{
				if (allShims)
				{
					ShimEmailFunctions.AllInstances.CreateLayoutAndEmailsMappingBlastStringStringStringDataTableBooleanBooleanString = (a, b, c, d, e, f, g, h, i) => new Hashtable();
				}
				ConfigurationManager.AppSettings["ECNEngineAccessKey"] = "dummyString";
				PlatformUserFake.GetByAccessKeyStringBoolean = (x, y) => CreateInstance(typeof(User));
				ClassesDataFunctions.connStr = string.Empty;
				ShimDbDataAdapter.AllInstances.FillDataSetString = (x, dataset, z) =>
				{
					dataset.Tables.Add(FillDataSetDummyTable("dummyColumn", "dummyRowValue"));
					return 0;
				};
				ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItem();
				ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(CampaignItem));
				ShimCampaignItemBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
				{
					var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
					SetProperty(campaignItemBlastFilterObject, "FilterID", null);
					var campaignItemBlastObject = CreateInstanceWithValues(typeof(CampaignItemBlast), new
					{
						Filters = new List<CampaignItemBlastFilter>()
						{
							campaignItemBlastFilterObject
						}
					});
					return campaignItemBlastObject;
				};
				ShimCampaignItemTestBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) =>
				{

					var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
					SetProperty(campaignItemBlastFilterObject, "FilterID", null);
					var campaignItemBlastObject = CreateInstanceWithValues(typeof(CampaignItemTestBlast), new
					{
						Filters = new List<CampaignItemBlastFilter>()
						{
							campaignItemBlastFilterObject
						}
					});
					return campaignItemBlastObject;
				};
				ShimCampaignItemSuppression.GetByCampaignItemID_NoAccessCheckInt32Boolean = (x, y) =>
				{
					var campaignItemSuppressionList = new List<CampaignItemSuppression>();
					var campaignItemBlastFilterObject = CreateInstance(typeof(CampaignItemBlastFilter));
					var campaignItemSuppressionObject = CreateInstanceWithValues(typeof(CampaignItemSuppression), new
					{
						Filters = new List<CampaignItemBlastFilter>()
						{
							campaignItemBlastFilterObject
						}
					});
					campaignItemSuppressionList.Add(campaignItemSuppressionObject);
					return campaignItemSuppressionList;
				};
				ShimDataFunctions.GetDataTableStringSqlCommand = (x, y) => new DataTable();
				ShimBlast.GetDefaultContentForSlotandDynamicTagsInt32 = (x) => GetDataTableDummyTable();
				ShimXmlDocument.AllInstances.LoadString = (x, y) => { };
				ShimXmlNode.AllInstances.SelectSingleNodeString = (x, y) => DummyXmlNode();
				ShimDataFunctions.ExecuteScalarString = (sqlQuery) => "0";
			}
			if (methodName == "CreateLayoutAndEmailsMapping")
			{
				ShimContentFilter.HasDynamicContentInt32 = (x) => false;
				ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) => CreateInstanceWithValues(typeof(Layout), new
				{
					TemplateID = 1,
					TableOptions = dummyTableLayoutOptions
				});
				ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (x) => CreateInstanceWithValues(typeof(Template), new
				{
					TemplateText = emailSlots,
					TemplateSource = dummyTemplateSource,
					SlotsTotal = 1
				});
				ShimLayout.GetPreviewNoAccessCheckInt32EnumsContentTypeCodeBooleanInt32NullableOfInt32NullableOfInt32NullableOfInt32 = (a, contentType, b, c, d, e, f) =>
				{
					return contentType == Enums.ContentTypeCode.HTML ? sampleHtmlBody : sampleTextBody;
				};
				ShimDataFunctions.GetTextContentInt32 = (x) => dummyText;
				ShimDataFunctions.GetContentInt32 = (x) => dummyContent;
				ShimTemplateFunctions.LinkReWriterTextStringBlastStringStringStringString = (textbody, a, b, c, d, e) => textbody;
				ShimTemplateFunctions.LinkReWriterStringBlastStringStringStringString = (htmlbody, a, b, c, d, e) => htmlbody;
				ShimTemplateFunctions.addOpensImageStringInt32StringString = (htmlbody, a, b, c) => htmlbody;
				ShimEmailBlast.ConstructorInt32DataTableStringStringBooleanBooleanStringStringStringBooleanBlast = (emailBlast, a, emailTable, htmlbody, textbody, b, c, d, e, f, g, h) =>
				{
					emailBlast.EmailsTable = emailTable;
					emailBlast.TEXT = textbody;
					emailBlast.HTML = htmlbody;
				};
				ShimDataFunctions.GetDataTableString = (x) =>
				{
					var dummyDataTable = new DataTable()
					{
						Columns =
						 {
							"ContentText",
							"ContentSource"
						 }
					};
					dummyDataTable.Rows.Add("dummyContentText", "dymmyContentSource");
					return dummyDataTable;
				};
				ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (x, y) => CreateInstanceWithValues(typeof(Content), new
				{

				});
			}
		}

		private XmlNode DummyXmlNode()
		{
			var doc = new XmlDocument();
			doc.LoadXml("<dummyXml> </dummyXml>");
			var elem = doc.CreateNode(XmlNodeType.Element, "sampleXmlNode", null);
			var childNode = doc.CreateNode(XmlNodeType.Element, "EmailAddress", "testEmail.com");
			elem.AppendChild(childNode);
			return elem;
		}

		private DataTable GetDataTableDummyTable()
		{
			var dummyTable = new DataTable();
			dummyTable.Columns.Add("EmailID");
			dummyTable.Columns.Add("BlastID");
			dummyTable.Columns.Add("EmailAddress");
			dummyTable.Columns.Add("CustomerID");
			dummyTable.Columns.Add("FormatTypeCode");
			dummyTable.Columns.Add("subscribetypecode");
			dummyTable.Columns.Add("ConversionTrkCDE");
			dummyTable.Columns.Add("mailRoute");
			dummyTable.Columns.Add("groupID");
			dummyTable.Rows.Add("groupID");
			return dummyTable;
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private dynamic CreateInstanceWithValues(Type type, dynamic values)
		{
			return ReflectionHelper.CreateInstanceWithValues(type, values);
		}

		private DataTable ChampionByProc_NoAccessCheckDummyTable()
		{
			var championsTable = new DataTable();
			championsTable.Columns.Add("dummyColumn");
			championsTable.Rows.Add("1");
			return championsTable;
		}

		private DataTable AbSampleCountDummyTable()
		{
			var dummyTable = new DataTable();
			dummyTable.Columns.Add("dummyColumn1");
			dummyTable.Columns.Add("dummyColumn2");
			dummyTable.Columns.Add("dummyColumn3");
			dummyTable.Columns.Add("dummyColumn4");
			dummyTable.Rows.Add("1", "1", "1", "1");
			dummyTable.Rows.Add("1", "1", "1", "1");
			return dummyTable;
		}

		private BlastAbstract BlastAbstractMockObject()
		{
			var blastAbstractMockObject = new Mock<BlastAbstract>().Object;
			blastAbstractMockObject.BlastType = "sample";
			blastAbstractMockObject.GroupID = 1;
			blastAbstractMockObject.LayoutID = 1;
			return blastAbstractMockObject;
		}

		private DataTable FillDataSetDummyTable(string colName, string rowValue)
		{
			var dummyTable = new DataTable();
			dummyTable.Columns.Add("colName");
			dummyTable.Columns.Add("EmailID");
			dummyTable.Columns.Add("BlastID");
			dummyTable.Columns.Add("EmailAddress");
			dummyTable.Columns.Add("CustomerID");
			dummyTable.Columns.Add("FormatTypeCode");
			dummyTable.Columns.Add("subscribetypecode");
			dummyTable.Columns.Add("ConversionTrkCDE");
			dummyTable.Columns.Add("BounceAddress");
			dummyTable.Columns.Add("mailRoute");
			dummyTable.Columns.Add("groupID");
			dummyTable.Rows.Add(rowValue, "1", "1", "1", "1", "1", "1", "1", "1", "1", "1");
			return dummyTable;
		}

		private List<BlastAbstract> BlastAbstractMockList()
		{
			var blastAbstractMockObject = new Mock<BlastAbstract>().Object;
			blastAbstractMockObject.BlastType = "sample";
			blastAbstractMockObject.GroupID = 1;
			blastAbstractMockObject.LayoutID = 1;
			var blastAbstractMockObject2 = new Mock<BlastAbstract>().Object;
			blastAbstractMockObject2.BlastType = "champion";
			var blastAbstractMockList = new List<BlastAbstract>();
			blastAbstractMockList.Add(blastAbstractMockObject);
			blastAbstractMockList.Add(blastAbstractMockObject);
			blastAbstractMockList.Add(blastAbstractMockObject2);
			return blastAbstractMockList;
		}

		private object CallMethod(Type type, string methodName, object[] parametersValues, object instance = null, bool paramCount = true)
		{
			if (paramCount)
			{
				return ReflectionHelper.CallOverloadedMethod(type, methodName, parametersValues, 9, instance);
			}
			else
			{
				return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
			}
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}

		private dynamic GetField(dynamic obj, string fieldName)
		{
			return ReflectionHelper.GetField(obj, fieldName);
		}

		private void SetProperty(dynamic instance, string propertyName, dynamic value)
		{
			ReflectionHelper.SetProperty(instance, propertyName, value);
		}

		private void CreateEmailTable(string[] colNames, string[][] rows)
		{
			var dummyEmailTable = new DataTable();
			foreach (var col in colNames)
			{
				dummyEmailTable.Columns.Add(col);
			}
			foreach (var row in rows)
			{
				dummyEmailTable.Rows.Add(row);
			}
			_emailTable = dummyEmailTable;
		}

		private bool IfSimilarToEmailTable(DataTable mappingTable)
		{
			var emailTableArray = new object[_emailTable.Rows.Count];
			_emailTable.Rows.CopyTo(emailTableArray, 0);
			var emailTablefirstRow = (string)((DataRow)emailTableArray[0])[0];
			var emailTableLastRow = (string)((DataRow)emailTableArray[_emailTable.Rows.Count - 1])[0];
			var mappingTableArray = new object[mappingTable.Rows.Count];
			mappingTable.Rows.CopyTo(mappingTableArray, 0);
			var mappingTablefirstRow = (string)((DataRow)mappingTableArray[0])[0];
			var mappingTableLastRow = (string)((DataRow)mappingTableArray[_emailTable.Rows.Count - 1])[0];
			return mappingTablefirstRow == emailTablefirstRow && mappingTableLastRow == emailTableLastRow && mappingTableArray.Length == emailTableArray.Length ? true : false;
		}

		private dynamic GetProperty(dynamic instance, string propertyName)
		{
			return ReflectionHelper.GetPropertyValue(instance, propertyName);
		}
	}
}