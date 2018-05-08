using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.BlastCreator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        private ECN_Framework_Entities.Communicator.CampaignItem _campaignItem;
        private ECN_Framework_Entities.Communicator.BlastAbstract _blastObject;
        private string _statusCode;
        private string _blastType;
        private string _suppressionGroups;
        private readonly DateTime _now = DateTime.Now;

        private const int SampleId = 10;
        private const int BlastScheduleId = 20;
        private const int OverrideAmount = 30;
        private const int LayoutId = 40;
        private const int GroupId = 50;
        private const int FinalOverrideAmount = OverrideAmount / 2;
        private const bool AddOptOutsToMs = true;
        private const bool OverrideIsAmount = true;
        private const bool IgnoreSuppression = false;
        private const string TestBlastType = "N";
        private const string SampleEmail1 = "sample1@email.com";
        private const string SampleEmail2 = "sample2@email.com";
        private const string EmailSubject1 = "Email Subject 1";
        private const string EmailSubject2 = "Email Subject 2";
        private const string FromName1 = "John";
        private const string FromName2 = "Mary";
        private const string ReplyTo = "Reply To";
        private const string DynamicFromName = "Dynamic From Name";
        private const string DynamicFromEmail = "Dynamic From Email";
        private const string DynamicReplyTo = "Dynamic Reply To";
        private const string MethodMapBlastObjects = "MapBlastObjects";
        private const string InternalPreserveStackTrace = "InternalPreserveStackTrace";

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void MapBlatObject_WhenCampaignItemIsNull_ThrowsException(int position)
        {
            // Arrange
            _campaignItem = null;
            _blastObject = new ECN_Framework_Entities.Communicator.BlastAB();

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                CallMapBlastObjectMethod(
                    new object[] {_campaignItem, _blastObject, _suppressionGroups, position});
            });
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void MapBlatObject_WhenBlastAbstractIsNull_ThrowsException(int position)
        {
            // Arrange
            SetupCampaignItem();
            _blastObject = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => { 
                CallMapBlastObjectMethod(
                    new object[] { _campaignItem, _blastObject, _suppressionGroups, position });
            });
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(0, "Suppression Group")]
        [TestCase(1, "")]
        [TestCase(1, "Suppression Group")]
        public void MapBlatObject_ShouldMapBlastPropertiesFromCampaignItem_ReachEnd(int position, string groups)
        {
			// Arrange
            SetupEnumns();
            SetupCampaignItem();
            _blastObject = new ECN_Framework_Entities.Communicator.BlastAB();
            var itemBlast = _campaignItem.BlastList[position];

            // Act
            CallMapBlastObjectMethod(
                new object[] { _campaignItem, _blastObject, groups, position });

            // Assert
            _blastObject.ShouldNotBeNull();
            _blastObject.ShouldSatisfyAllConditions(
                () => _blastObject.StatusCode.ShouldBe(_statusCode),
                () => _blastObject.BlastType.ShouldBe(_blastType),
                () => _blastObject.TestBlast.ShouldBe(TestBlastType),
                () => _blastObject.SampleID.ShouldBe(SampleId),
                () => _blastObject.SendTime.ShouldBe(_now),
                () => _blastObject.BlastScheduleID.ShouldBe(BlastScheduleId),
                () => _blastObject.OverrideAmount.ShouldBe(FinalOverrideAmount),
                () => _blastObject.OverrideIsAmount.ShouldBe(OverrideIsAmount),
                () => _blastObject.IgnoreSuppression.ShouldBe(IgnoreSuppression),
                () => _blastObject.EmailSubject.ShouldBe(itemBlast.EmailSubject),
                () => _blastObject.EmailFrom.ShouldBe(itemBlast.EmailFrom),
                () => _blastObject.EmailFromName.ShouldBe(itemBlast.FromName),
                () => _blastObject.ReplyTo.ShouldBe(itemBlast.ReplyTo),
                () => _blastObject.LayoutID.ShouldBe(itemBlast.LayoutID),
                () => _blastObject.GroupID.ShouldBe(itemBlast.GroupID),
                () => _blastObject.AddOptOuts_to_MS.ShouldBe(itemBlast.AddOptOuts_to_MS),
                () => _blastObject.DynamicFromEmail.ShouldBe(itemBlast.DynamicFromEmail),
                () => _blastObject.DynamicFromName.ShouldBe(itemBlast.DynamicFromName),
                () => _blastObject.DynamicReplyToEmail.ShouldBe(itemBlast.DynamicReplyTo),
                () => _blastObject.BlastSuppression.ShouldBe(groups));
        }

        private static void CallMapBlastObjectMethod(object[] parametersValues)
        {
            var methodInfo = typeof(BlastFromABCampaign).GetAllMethods()
                .FirstOrDefault(x => x.Name == MethodMapBlastObjects
                                     && x.IsPrivate);

            if (methodInfo == null)
            {
                return;
            }

            try
            {
                methodInfo.Invoke(null, parametersValues);
            }
            catch (TargetInvocationException targetException)
            {
                if (targetException.InnerException == null)
                {
                    throw;
                }
                        
                var innerException = targetException.InnerException;
                (Delegate.CreateDelegate(
                        typeof(ThreadStart),
                        innerException,
                        InternalPreserveStackTrace,
                        false,
                        false) as ThreadStart)
                    ?.Invoke();
                throw innerException;
            }
        }

        private void SetupEnumns()
        {
            _statusCode = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString();
            _blastType = ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Sample.ToString();
        }

        private void SetupCampaignItem()
        {
            _suppressionGroups = string.Empty;

            _campaignItem = new ECN_Framework_Entities.Communicator.CampaignItem
            {
                SampleID = SampleId,
                SendTime = _now,
                BlastScheduleID = BlastScheduleId,
                OverrideAmount = OverrideAmount,
                OverrideIsAmount = OverrideIsAmount,
                IgnoreSuppression = IgnoreSuppression,

                BlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>
                {
                    new ECN_Framework_Entities.Communicator.CampaignItemBlast
                    {
                        EmailSubject = EmailSubject1,
                        EmailFrom = SampleEmail1,
                        FromName = FromName1,
                        ReplyTo = ReplyTo,
                        LayoutID = LayoutId,
                        GroupID = GroupId,
                        AddOptOuts_to_MS = AddOptOutsToMs,
                        DynamicFromName = DynamicFromName,
                        DynamicFromEmail = DynamicFromEmail,
                        DynamicReplyTo = DynamicReplyTo
                    },
                    new ECN_Framework_Entities.Communicator.CampaignItemBlast
                    {
                        EmailSubject = EmailSubject2,
                        EmailFrom = SampleEmail2,
                        FromName = FromName2,
                        ReplyTo = ReplyTo,
                        LayoutID = LayoutId,
                        GroupID = GroupId,
                        AddOptOuts_to_MS = AddOptOutsToMs,
                        DynamicFromName = DynamicFromName,
                        DynamicFromEmail = DynamicFromEmail,
                        DynamicReplyTo = DynamicReplyTo
                    }
                }
            };
        }
    }
}
