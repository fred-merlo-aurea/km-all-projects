using System;
using System.Collections.Generic;
using System.Fakes;
using System.Globalization;
using System.Threading;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Object.Fakes;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using Entity = FrameworkUAD.Entity;
using FilterTypes = FrameworkUAD.BusinessLogic.Enums.FiltersType;
using Object = FrameworkUAD.Object;
using ViewTypes = FrameworkUAD.BusinessLogic.Enums.ViewType;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    public partial class FilterMVCTest
    {
        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldBRAND_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "BRAND" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  join brand b with (nolock) on b.brandID = bd.brandID  where b.IsDeleted = 0 and bd.BrandID = ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldPRODUCT_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PRODUCT" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.pubid in ( ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldDATACOMPARE1_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "DATACOMPARE", Values = "Test|1" } } };
            ShimCode.AllInstances.SelectCodeIdInt32 = (p1, p2) => new Code { CodeName = p2 == 1 ? "Match" : "Default" };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No  where d.ProcessCode = 'Test';");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldDATACOMPARE0_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "DATACOMPARE", Values = "Test|0" } } };
            ShimCode.AllInstances.SelectCodeIdInt32 = (p1, p2) => new Code { CodeName = p2 == 1 ? "Match" : "Default" };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  left outer join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No and  d.ProcessCode = 'Test' where d.SubscriberFinalId is null ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldSTATE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "STATE"} } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where s.State in ('') ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldSTATE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "STATE" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.RegionCode in ('') ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldCOUNTRY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "COUNTRY", Values = "1,2,3,4" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.CountryID = 1 OR s.CountryID in ( 2 )  OR ((s.CountryID = 1) or (s.CountryID = 2))  OR not ((s.CountryID = 1) or (s.CountryID = 2) or ISNULL(s.CountryID,0) = 0));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldCOUNTRY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "COUNTRY", Values = "1,2,3,4" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where (ps.CountryID = 1 OR ps.CountryID in ( 2 )  OR ((ps.CountryID = 1) or (ps.CountryID = 2))  OR not ((ps.CountryID = 1) or (ps.CountryID = 2) or ISNULL(ps.CountryID,0) = 0));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEMAIL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "EMAIL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.emailexists = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldPHONE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PHONE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.phoneexists = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldFAX_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "FAX", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.faxexists = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldGEOLOCATED_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "GEOLOCATED", Values = "1,1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.IsLatLonValid = 1 OR s.IsLatLonValid = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldQSOURCE_TYPE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "QSOURCE TYPE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.PubQSourceID in (select CodeID from UAD_Lookup..Code with (nolock) where ParentCodeId in ( 1 ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldTEXTPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "TEXTPERMISSION", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.TextPermission = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEMAILRENEWPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "EMAILRENEWPERMISSION", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.EmailRenewPermission = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldTHIRDPARTYPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "THIRDPARTYPERMISSION" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.ThirdPartyPermission = );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldOTHERPRODUCTSPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "OTHERPRODUCTSPERMISSION", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.OtherProductsPermission = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldPHONEPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PHONEPERMISSION", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.PhonePermission = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldFAXPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "FAXPERMISSION", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.FaxPermission = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldMAILPERMISSION_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "MAILPERMISSION", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.MailPermission = 1);");
        }



        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldMEDIA_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "MEDIA", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.Demo7 in (  '1') ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldQFROM_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "QFROM", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where  ps.QualificationDate >= '1' ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldQTO_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "QTO", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where  ps.QualificationDate <= '1 23:59:59' ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEMAIL_STATUS_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "EMAIL STATUS", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.EmailStatusID in (1) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldCATEGORY_CODE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "CATEGORY CODE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.PubCategoryID in ( 1 ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldCATEGORY_CODE_AddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "CATEGORY CODE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0  where ISNULL(sak.PubCategoryID, ps.PubCategoryID) in ( 1 ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldCATEGORY_TYPE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "CATEGORY TYPE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.PubCategoryID  in (select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( 1 ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldCATEGORY_TYPE_AddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "CATEGORY TYPE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0  where ISNULL(sak.PubCategoryID, ps.PubCategoryID) in (select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( 1 ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldTRANSACTION_CODE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "TRANSACTION CODE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.PubTransactionID in ( 1 );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldTRANSACTION_CODE_AddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "TRANSACTION CODE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0  where ISNULL(sak.PubTransactionID,ps.PubTransactionID) in ( 1 );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldXACT_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "XACT", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.PubTransactionID in (select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( 1 ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldXACT_AddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "XACT", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0  where ISNULL(sak.PubTransactionID,ps.PubTransactionID) in (select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( 1 ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldQSOURCE_CODE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "QSOURCE CODE", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.PubQSourceID in ( 1 ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupM_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in  (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock)  join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID  join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=1 where  (ms.MasterDesc = '1' or ms.Mastervalue = '1') );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupM_CONTAINS_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "CONTAINS", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in  (select distinct vrc.subscriptionID from vw_RecentConsensus vrc  join Mastercodesheet ms with (nolock)  on vrc.MasterID = ms.MasterID  where vrc.MasterGroupID=1 and  (ms.MasterDesc like '%1%'  or  ms.Mastervalue like '%1%') );");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ViewTypeNone_ProfileFieldEAdHoc_GroupM_DOES_NOT_CONTAIN_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC {  BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "DOES NOT CONTAIN", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID not in  (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock)  join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID  join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID  join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID  where  (bd.BrandID = 1 and ms.MasterGroupID=1 and ( (ms.MasterDesc like '%1%'  or  ms.Mastervalue like '%1%') )));");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ProductView_ProfileFieldEAdHoc_GroupM_START_WITH_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "START WITH", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in  (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock)  join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID  join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID  join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID  where  (bd.BrandID = 1 and ms.MasterGroupID=1 and ((ms.MasterDesc like '1%' or ms.Mastervalue like '1%') )));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupM_END_WITH_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "END WITH", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in  (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock)  join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID  join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=1 where  (ms.MasterDesc like '%1' or  ms.Mastervalue like '%1') );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupM_IS_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "IS EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in  (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock)  left outer join (select distinct sd.subscriptionID from subscriptiondetails sd WITH (nolock)  join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID  where ms.MasterGroupID=1) inn1 on sfilter.SubscriptionID = inn1.SubscriptionID  where inn1.subscriptionID is null );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupM_IS_NOT_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "m|1", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in  (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock)  join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID  join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupD_qdate_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|qdate", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupD_qualificationdate_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|qualificationdate", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupD_transactiondate_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|[transactiondate]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupD_pubtransactiondate_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|[pubtransactiondate]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupD_statusupdateddate_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|[statusupdateddate]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupD_datecreated_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|datecreated", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupD_dateupdated_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|dateupdated", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupD_default_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|default", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupD_default_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "d|default", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_PRODUCT_COUNT_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[PRODUCT COUNT]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ( s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) group by ps2.subscriptionid  having COUNT(ps2.PubID) = 1));");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ViewTypeNone_ProfileFieldEAdHoc_GroupI_PRODUCT_COUNT_GREATER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[PRODUCT COUNT]", SearchCondition = "GREATER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ( s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) join branddetails bd2 WITH (nolock) ON bd2.pubid = ps2.pubid where bd2.BrandID = 1 group by ps2.subscriptionid  having COUNT(ps2.PubID) > 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_PRODUCT_COUNT_LESSER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[PRODUCT COUNT]", SearchCondition = "LESSER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ( s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) group by ps2.subscriptionid  having COUNT(ps2.PubID) < 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[SCORE] = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join BrandScore bs  with (nolock)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = 1 where (bs.[SCORE] = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupI_SCORE_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where (ps.[SCORE] = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_RANGE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "RANGE", Values = "1|1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[SCORE] >= 1 and s.[SCORE] <= 1);");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_RANGE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "RANGE", Values = "1|1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join BrandScore bs  with (nolock)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = 1 where (bs.[SCORE] >= 1 and bs.[SCORE] <= 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupI_SCORE_RANGE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "RANGE", Values = "1|1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where (ps.[SCORE] >= 1 and ps.[SCORE] <= 1);");
        }


        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_GREATER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "GREATER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[SCORE] > 1);");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_GREATER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "GREATER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join BrandScore bs  with (nolock)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = 1 where (bs.[SCORE] > 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupI_SCORE_GREATER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "GREATER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where (ps.[SCORE] > 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_LESSER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "LESSER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[SCORE] < 1);");
        }

        [Test]
        public void GetFilterQuery_FilterBrandId1_ViewTypeNone_ProfileFieldEAdHoc_GroupI_SCORE_LESSER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "LESSER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join BrandScore bs  with (nolock)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = 1 where (bs.[SCORE] < 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupI_SCORE_LESSER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[SCORE]", SearchCondition = "LESSER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where (ps.[SCORE] < 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_DEFAULT_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[DEFAULT]", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[DEFAULT] = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_DEFAULT_RANGE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[DEFAULT]", SearchCondition = "RANGE", Values = "1|1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[DEFAULT] >= 1 and s.[DEFAULT] <= 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_DEFAULT_GREATER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[DEFAULT]", SearchCondition = "GREATER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[DEFAULT] > 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupI_DEFAULT_LESSER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "i|[DEFAULT]", SearchCondition = "LESSER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.[DEFAULT] < 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_I_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|i", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE (CAST(E.1 AS INT) = 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupE_I_RANGE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|i", SearchCondition = "RANGE", Values = "1|1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where  ps.PubSubscriptionID in (select E.PubSubscriptionID FROM PubSubscriptionsExtension E with (nolock) join pubsubscriptions ps on ps.pubsubscriptionID = E.pubsubscriptionID WHERE ps.PubID = 0 and (CAST(E.1 AS INT) >= 1 and CAST(E.1 AS INT) <= 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_F_GREATER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|f", SearchCondition = "GREATER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE (CAST(E.1 AS FLOAT) > 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_F_LESSER_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|f", SearchCondition = "LESSER", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE (CAST(E.1 AS FLOAT) < 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_B_DEFAULT_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|b", SearchCondition = "DEFAULT", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE CAST(E.1 AS BIT) = 1);");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_D_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|d", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE ());");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_X_EQUAL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "EQUAL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE  E.1 = '1');");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_X_CONTAINS_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "CONTAINS", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE  PATINDEX('%1%', E.1) > 0 );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_X_DOES_NOT_CONTAIN_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "DOES NOT CONTAIN", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID not in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE  PATINDEX('%1%', E.1) > 0 );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_X_START_WITH_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "START WITH", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE  PATINDEX('1%', E.1) > 0 );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_X_END_WITH_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "END WITH", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE  PATINDEX('%1', E.1) > 0 );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupE_X_IS_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "IS EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where  ps.PubSubscriptionID not in (select E.PubSubscriptionID FROM PubSubscriptionsExtension E with (nolock) join pubsubscriptions ps on ps.pubsubscriptionID = E.pubsubscriptionID WHERE ps.PubID = 0 and  (ISNULL(E.1, '') != ''  ));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupE_X_IS_NOT_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "e|1|x", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where  s.SubscriptionID in (select E.SubscriptionID FROM SubscriptionsExtension E with (nolock)  WHERE  (ISNULL(E.1, '') != ''  ));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_COUNTRY_IS_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[COUNTRY]", SearchCondition = "IS EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  left outer join UAD_Lookup..Country c on c.CountryID = s.countryID  where ( (c.CountryID is NULL ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_COUNTRY_IS_NOT_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[COUNTRY]", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join UAD_Lookup..Country c on c.CountryID = s.countryID  where ( (c.CountryID is NOT NULL ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_COUNTRY_IS_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[COUNTRY]", SearchCondition = "IS EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  left outer join UAD_Lookup..Country c on c.CountryID = ps.countryID  where ( (c.CountryID is NULL ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_COUNTRY_IS_NOT_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[COUNTRY]", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  join UAD_Lookup..Country c on c.CountryID = ps.countryID  where ( (c.CountryID is NOT NULL ) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupEMAIL_IS_NOT_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[EMAIL]", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ( (ISNULL(ps.[EMAIL], '') != ''  ));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupIGRP_IS_NOT_EMPTY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[IGRP_NO]", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ( ( cast(s.[IGRP_NO] as varchar(100)) is NOT NULL) );");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEAdHoc_GroupDefault_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[Default]", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Ac
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where ( (ISNULL(s.[Default], '') != ''  ));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEAdHoc_GroupDefault_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ADHOC", Group = "[Default]", SearchCondition = "IS NOT EMPTY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ( (ISNULL(ps.[Default], '') != ''  ));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldZIPCODE_RADIUS_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "ZIPCODE-RADIUS", SearchCondition = "1|1|1" } } };
            ShimLocation.ValidateBingAddressLocationString = (p1, p2) => new Object.Location { IsValid = true };
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where (s.Latitude >= -0.0144927536231884 and s.Latitude <= 0.0144927536231884 and s.Longitude >= -0.0144927540868248 and s.Longitude <= 0.0144927540868248 and (s.Latitude<=-0.0144927536231884 OR 0.0144927536231884<=s.Latitude OR s.Longitude<=-0.0144927540868248 OR 0.0144927540868248<=s.Longitude) and isnull(s.IsLatLonValid,0) = 1  and ( master.dbo.fn_CalcDistanceBetweenLocations(0, 0, s.Latitude, s.Longitude, 0) between  1 and 1));");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldWAVE_MAILING_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "WAVE MAILING", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where  ISNULL(ps.IsInActiveWaveMailing,0) = 1;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldQUALIFICATIONYEAR_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC
            {
                Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "QUALIFICATIONYEAR", Values = "1" },
                new Object.FilterDetails { Name = "PRODUCT", Values = "1" } }
            };
            ShimProduct.AllInstances.SelectInt32ClientConnectionsBooleanBoolean = (p1, p2, p3, p4, p5) => new Entity.Product { YearStartDate = "1.1", YearEndDate = "1.2" };
            ShimDateTime.NowGet = () => new DateTime(2018, 1, 1);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ( ps.QualificationDate between convert(varchar(20), DATEADD(year, -0, '1/1/2017'),111)  and  convert(varchar(20), DATEADD(year, -0, '12/31/2017'),111) + ' 23:59:59') and ps.pubid in (1 ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldLAST_NAME_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "LAST NAME", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where PATINDEX('1%', s.lname ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldLAST_NAME_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "LAST NAME", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where PATINDEX('1%', ps.LastName ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldFIRST_NAME_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "FIRST NAME", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where PATINDEX('1%', s.fname ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldFIRST_NAME_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "FIRST NAME", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where PATINDEX('1%', ps.FirstName ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldCOMPANY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "COMPANY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where PATINDEX('1%', s.company ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldCOMPANY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "COMPANY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where PATINDEX('1%', ps.Company ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldPHONENO_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PHONENO", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where PATINDEX('1%', s.phone ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldPHONENO_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PHONENO", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where PATINDEX('1%', ps.phone ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldEMAILID_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "EMAILID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  where PATINDEX('1%', s.email ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldEMAILID_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "EMAILID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where PATINDEX('1%', ps.emailID ) > 0 ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeDimension_ProductView_IsAddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Dimension}, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) join pubsubscriptions ps1  with (nolock) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where  psd.codesheetID in ( )  intersect select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) join pubsubscriptions ps1  with (nolock) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where  ps1.pubID = 0 and psd.codesheetID in (  ) ;select  from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0 ; drop table #tempDimSub; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeDimension_CrossProductView_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.CrossProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Dimension }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) join pubsubscriptions ps1  with (nolock) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where  psd.codesheetID in ( )  intersect select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) join pubsubscriptions ps1  with (nolock) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where  ps1.pubID = 0 and psd.codesheetID in (  ) ;select  from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID ; drop table #tempDimSub; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeDimension_RecencyView_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.RecencyView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Dimension }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub select distinct vrc.SubscriptionID from vw_RecentConsensus vrc with (nolock) where  vrc.masterid in ( )  intersect select distinct vrc.SubscriptionID from vw_RecentConsensus vrc with (nolock) where  vrc.masterid  in (  ) ;select  from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID ; drop table #tempDimSub; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeDimension_BrandId1_RecencyViewReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, ViewType = ViewTypes.RecencyView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Dimension }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub  select distinct vrbc.subscriptionid from vw_RecentBrandConsensus vrbc with (nolock) where  vrbc.brandid= 1 and vrbc.masterid in ( )  intersect  select distinct vrbc.subscriptionid from vw_RecentBrandConsensus vrbc with (nolock) where  vrbc.brandid = 1 and vrbc.masterid  in (  ) ;select  from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID ; drop table #tempDimSub; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeDimension_ViewNone_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Dimension }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub select distinct sd.SubscriptionID from SubscriptionDetails sd  with (nolock) where  sd.masterid in ( )  intersect select distinct sd.SubscriptionID from SubscriptionDetails sd  with (nolock) where  sd.masterid  in (  ) ;select  from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID ; drop table #tempDimSub; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeDimension_BrandId1_ViewNone_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Dimension }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub  select distinct vbc.subscriptionid from vw_BrandConsensus vbc with (nolock) where  vbc.brandid= 1 and vbc.masterid in ( )  intersect  select distinct vbc.subscriptionid from vw_BrandConsensus vbc with (nolock) where  vbc.brandid= 1 and vbc.masterid  in (  ) ;select  from #tempDimSub x4  with (nolock) join pubsubscriptions ps  with (nolock)  on x4.subscriptionID = ps.subscriptionID ; drop table #tempDimSub; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_CRITERIA_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC {Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN CRITERIA", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("; Create table #tempSOA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid);  Insert into #tempSOA  select so.SubscriptionID  from PubSubscriptions pso   with (NOLOCK)  join SubscriberOpenActivity so  with (NOLOCK) on so.PubSubscriptionID = pso.PubSubscriptionID  group by so.SubscriptionID HAVING Count(so.openactivityid) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ; drop table #tempSOA; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_ACTIVITY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN ACTIVITY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_BLASTID_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_CAMPAIGNS_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN CAMPAIGNS", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_EMAIL_SUBJECT_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN EMAIL SUBJECT", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_EMAIL_SENT_DATE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN EMAIL SENT DATE", Values = "1", SearchCondition = "Default" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_CRITERIA_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK CRITERIA", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("; Create table #tempSCA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid);  Insert into #tempSCA  select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ; drop table #tempSCA; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_ACTIVITY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK ACTIVITY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_LINK_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "LINK", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("; Create table #tempSCA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid);  Insert into #tempSCA  select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  Where ( (Link like'%1%' or LinkAlias like '%1%')) group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ; drop table #tempSCA; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_BLASTID_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_CAMPAIGNS_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK CAMPAIGNS", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_EMAIL_SUBJECT_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK EMAIL SUBJECT", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_EMAIL_SENT_DATE_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK EMAIL SENT DATE", Values = "1", SearchCondition = "Default" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_DOMAIN_TRACKING_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "DOMAIN TRACKING", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_URL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "URL", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_VISIT_ACTIVITY_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "VISIT ACTIVITY", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_VISIT_CRITERIA_AddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "VISIT CRITERIA", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe("; Create table #tempSVA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid);  Insert into #tempSVA  select sv.SubscriptionID from  SubscriberVisitActivity sv  with (NOLOCK)  group by sv.SubscriptionID having COUNT(sv.VisitActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0  join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ; drop table #tempSVA; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ProductView_ProfileFieldPRODUCT_PubIds_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { ViewType = ViewTypes.ProductView, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PRODUCT", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock)  where ps.pubid in (1 ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_BrandId1_ViewNone_OPEN_SEARCH_ALL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC {BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN CRITERIA", Values = "1", SearchCondition = "Search All" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN BLASTID", Values = "1" } } }; 

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("CREATE TABLE #tempOblast (blastid INT); CREATE UNIQUE CLUSTERED INDEX #tempOblast_1 ON #tempOblast (blastid);   insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)   where bla.BlastID in (1); Create table #tempSOA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid);  Insert into #tempSOA select x.subscriptionID from ( select so.SubscriptionID, openactivityID from SubscriberOpenActivity so with (NOLOCK)  join PubSubscriptions pso  with (NOLOCK) on so.PubSubscriptionID = pso.PubSubscriptionID  join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID  where  pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in (1) and  b.Isdeleted = 0)  union select so1.SubscriptionID, so1.openactivityID from SubscriberOpenActivity so1 with (NOLOCK)  join #tempOblast tob  with (NOLOCK) on so1.blastID = tob.blastID  where  so1.pubsubscriptionid IS NULL  ) x GROUP  BY x.subscriptionid  HAVING Count(x.openactivityid) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ; drop table #tempSOA; drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_OPEN_SEARCH_ALL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC {Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN CRITERIA", Values = "1", SearchCondition = "Search All" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("CREATE TABLE #tempOblast (blastid INT); CREATE UNIQUE CLUSTERED INDEX #tempOblast_1 ON #tempOblast (blastid);   insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)   where bla.BlastID in (1); Create table #tempSOA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid);  Insert into #tempSOA  select so.SubscriptionID from SubscriberOpenActivity so  with (NOLOCK)  join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID  group by so.SubscriptionID HAVING Count(so.openactivityid) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ; drop table #tempSOA; drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_BrandId1_ViewNone_OPEN_SEARCH_SPECIFIC_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN CRITERIA", Values = "1", SearchCondition = "Specific" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "OPEN BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("CREATE TABLE #tempOblast (blastid INT); CREATE UNIQUE CLUSTERED INDEX #tempOblast_1 ON #tempOblast (blastid);   insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)   where bla.BlastID in (1); Create table #tempSOA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid);  Insert into #tempSOA  select so.SubscriptionID  from PubSubscriptions pso   with (NOLOCK)  join SubscriberOpenActivity so  with (NOLOCK) on so.PubSubscriptionID = pso.PubSubscriptionID  JOIN branddetails bd  with (NOLOCK) ON bd.pubID = pso.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID  join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID  where  bd.BrandID in (1) and b.Isdeleted = 0 group by so.SubscriptionID HAVING Count(so.openactivityid) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ; drop table #tempSOA; drop table #tempOblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_BrandId1_ViewNone_CLICK_SEARCH_ALL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK CRITERIA", Values = "1", SearchCondition = "Search All" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("CREATE TABLE #tempCblast (blastid INT); CREATE UNIQUE CLUSTERED INDEX #tempCblast_1 ON #tempCblast (blastid);  Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)   where bla.BlastID in (1); Create table #tempSCA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid);  Insert into #tempSCA select x.subscriptionID from ( select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc  with (NOLOCK)  join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID  where  pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in (1) and  b.Isdeleted = 0)union select sc1.SubscriptionID, sc1.ClickActivityID  from  SubscriberClickActivity sc1  with (NOLOCK)  join #tempCblast tcb with (NOLOCK) on sc1.blastID = tcb.blastID  where  sc1.pubsubscriptionid IS NULL  ) x GROUP BY x.subscriptionid  HAVING Count(x.ClickActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ; drop table #tempSCA; drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_CLICK_SEARCH_ALL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK CRITERIA", Values = "1", SearchCondition = "Search All" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("CREATE TABLE #tempCblast (blastid INT); CREATE UNIQUE CLUSTERED INDEX #tempCblast_1 ON #tempCblast (blastid);  Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)   where bla.BlastID in (1); Create table #tempSCA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid);  Insert into #tempSCA  select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK)  join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID  group by sc.SubscriptionID HAVING Count(sc.ClickActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ; drop table #tempSCA; drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_BrandId1_ViewNone_CLICK_SEARCH_SPECIFIC_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK CRITERIA", Values = "1", SearchCondition = "Specific" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "CLICK BLASTID", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("CREATE TABLE #tempCblast (blastid INT); CREATE UNIQUE CLUSTERED INDEX #tempCblast_1 ON #tempCblast (blastid);  Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)   where bla.BlastID in (1); Create table #tempSCA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid);  Insert into #tempSCA  select sc.SubscriptionID from PubSubscriptions psc  with (NOLOCK)  join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID  JOIN branddetails bd  with (NOLOCK) ON bd.pubID = psc.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID  join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID  where  bd.BrandID in (1) and b.Isdeleted = 0 group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ; drop table #tempSCA; drop table #tempCblast; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_ViewNone_VISIT_DOMAIN_URL_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { BrandID = 1, Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "VISIT CRITERIA", Values = "1" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "URL", Values = "test.km.com,test1.km.com" }, new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "DOMAIN TRACKING", Values = "1" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe("; Create table #tempSVA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid);  Insert into #tempSVA  select sv.SubscriptionID from  SubscriberVisitActivity sv  with (NOLOCK)  where sv.DomainTrackingID = 1 and ( sv.url like '%test.km.com%' or sv.url like '%test1.km.com%') group by sv.SubscriptionID having COUNT(sv.VisitActivityID) >= 1;select  from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ; drop table #tempSVA; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeNone_ViewTypeNone_ProfileFieldPRODUCT_AddlFilter_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { Name = "PRODUCT" } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, "testcondition", new ClientConnections(), false);

            // Assert
            result.ShouldBe("select  from pubsubscriptions ps  with (nolock) testcondition where ps.pubid in ( ) ;");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivity_Dimension_ViewNone_VISIT_CRITERIA_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "VISIT CRITERIA", Values = "1" }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), false);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub select distinct sd.SubscriptionID from SubscriptionDetails sd  with (nolock) where  sd.masterid in ( ) ;; Create table #tempSVA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid);  Insert into #tempSVA  select sv.SubscriptionID from  SubscriberVisitActivity sv  with (NOLOCK)  group by sv.SubscriptionID having COUNT(sv.VisitActivityID) >= 1;select  from #tempDimSub x4  with (nolock) join subscriptions s with (nolock) on x4.SubscriptionID = s.SubscriptionID join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ; drop table #tempDimSub;  drop table #tempSVA; ");
        }

        [Test]
        public void GetFilterQuery_FilterTypeActivityDimension_ViewNone_VISIT_CRITERIA_AddRemove_ReturnsQuery()
        {
            // Arrange
            var filter = new Object.FilterMVC { Fields = new List<Object.FilterDetails> { new Object.FilterDetails { FilterType = FilterTypes.Activity, Name = "VISIT CRITERIA", Values = "1" }, new Object.FilterDetails { FilterType = FilterTypes.Dimension } } };

            // Act
            var result = FilterMVC.getFilterQuery(filter, string.Empty, string.Empty, new ClientConnections(), true);

            // Assert
            result.ShouldBe(" Create table #tempDimSub (SubscriptionID int); CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (SubscriptionID);  Insert into #tempDimSub select distinct sd.SubscriptionID from SubscriptionDetails sd  with (nolock) where  sd.masterid in ( ) ;; Create table #tempSVA (subscriptionid int); CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid);  Insert into #tempSVA  select sv.SubscriptionID from  SubscriberVisitActivity sv  with (NOLOCK)  group by sv.SubscriptionID having COUNT(sv.VisitActivityID) >= 1;select  from #tempDimSub x4  with (nolock) join subscriptions s with (nolock) on x4.SubscriptionID = s.SubscriptionID join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID  left join SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0  join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ; drop table #tempDimSub;  drop table #tempSVA; ");
        }
    }
}
