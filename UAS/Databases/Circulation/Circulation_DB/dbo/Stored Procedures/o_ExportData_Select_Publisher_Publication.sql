

CREATE PROCEDURE [dbo].[o_ExportData_Select_Publisher_Publication]
@PublisherID int,
@PublicationID int
AS
--DECLARE @PublisherID int = 12, @PublicationID int = 4
SELECT DISTINCT s.SubscriberID, ss.PublicationID, ss.SubscriptionID,
		[dbo].[fn_GetLastBatchID](ss.SubscriptionID) as 'Batch', 
		[dbo].[fn_GetLastBatchID](ss.SubscriptionID) as 'Hisbatch', 
		ci.Hisbatch1, ci.Hisbatch2, ci.Hisbatch3,
        p.PublicationCode as 'Pubcode', ss.SequenceID as 'SequenceID', cc.CategoryCodeValue as 'Cat', tc.TransactionCodeValue as 'Xact', ss.DateUpdated as 'XactDate',
       s.FirstName as 'Fname', s.LastName as 'Lname', s.Title, s.Company, s.Address1 as 'Address', s.Address2 as 'Mailstop', s.City, s.RegionCode as 'State', s.ZipCode as 'Zip', s.Plus4,
       s.County, c.CountryName as 'Country', c.CountryID as 'CRTY', s.Phone, s.Fax, s.Mobile, s.Email, s.Website, ss.AccountNumber as 'AcctNum',
       ss.OriginalSubscriberSourceCode as 'ORIGSSRC', ss.SubscriberSourceCode as 'SUBSRC', ss.Copies, ci.NANQ,       
       ss.QSourceID as 'Qsource', ss.QSourceDate as 'Qdate', ss.DateCreated as 'Cdate', ss.Par3cID as 'Par3C',
       ci.EmailID, ci.Verify, ci.Interview, ci.Mail, ci.PrevQDate as 'Old_Date', ci.PrevQSource as 'Old_QSRC', ci.MemberID as 'MBR_ID', ci.MemberFlag as 'MBR_Flag', ci.MemberReject as 'MBR_Reject'       
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'SPECIFY') as 'SPECIFY'       
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'SIC') as 'SIC'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'EMPLOY') as 'EMPLOY'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'SALES') as 'SALES'
       ,ci.IMBSerial1 as 'IMB_SERIAL1', ci.IMBSerial2 as 'IMB_SERIAL2', ci.IMBSerial3 as 'IMB_SERIAL3'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'BUSINESS') as 'Business'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'BUSINESS') as 'BUSNTEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'FUNCTION') as 'Function'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'FUNCTION') as 'FUNCTEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO1') as 'DEMO1'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'DEMO1') as 'DEMO1TEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO2') as 'DEMO2'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO3') as 'DEMO3'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO4') as 'DEMO4'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO5') as 'DEMO5'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO6') as 'DEMO6'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'DEMO6') as 'DEMO6TEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO7') as 'DEMO7'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO8') as 'DEMO8'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO9') as 'DEMO9'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO10') as 'DEMO10'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'DEMO10') as 'DEMO10TEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO11') as 'DEMO11'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO12') as 'DEMO12'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO14') as 'DEMO14'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO15') as 'DEMO15'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO16') as 'DEMO16'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO18') as 'DEMO18'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO19') as 'DEMO19'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO20') as 'DEMO20'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO21') as 'DEMO21'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO22') as 'DEMO22'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO23') as 'DEMO23'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO24') as 'DEMO24'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO25') as 'DEMO25'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO26') as 'DEMO26'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO27') as 'DEMO27'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO28') as 'DEMO28'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO29') as 'DEMO29'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO40') as 'DEMO40'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO41') as 'DEMO41'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO42') as 'DEMO42'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO43') as 'DEMO43'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO44') as 'DEMO44'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO45') as 'DEMO45'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO46') as 'DEMO46'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO31') as 'DEMO31'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO32') as 'DEMO32'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO33') as 'DEMO33'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO34') as 'DEMO34'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO35') as 'DEMO35'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO36') as 'DEMO36'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO37') as 'DEMO37'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'DEMO38') as 'DEMO38'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'SECBUS') as 'SECBUS'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'SECFUNC') as 'SECFUNC'       
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'BUSINESS1') as 'Business1'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'FUNCTION1') as 'Function1'       
       ,s.Income as 'Income1', s.Age as 'Age1',ci.HomeValue as 'Home_Value'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'JOBT1') as 'JOBT1'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'JOBT1TEXT') as 'JOBT1TEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'JOBT2') as 'JOBT2'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'JOBT3') as 'JOBT3'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'TOE1') as 'TOE1'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'TOE2') as 'TOE2'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'AOI1') as 'AOI1'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'AOI2') as 'AOI2'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'AOI3') as 'AOI3'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'PROD1') as 'PROD1'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'PROD1TEXT') as 'PROD1TEXT'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'BUYAUTH') as 'BUYAUTH'
       ,[dbo].[fn_ResponseIDs_SubscriptionID_Response_Active](ss.SubscriptionID, 'IND1') as 'IND1'
       ,[dbo].[fn_Response_Other](ss.SubscriptionID, 'IND1TEXT') as 'IND1TEXT'       
       ,ss.IsPaid as 'STATUS', sp.PriceCodeID as 'PRICECODE', sp.TotalIssues as 'NUMISSUES', sp.CPRate as 'CPRATE', pc.Term as 'TERM', ci.IssuesToGo as 'ISSTOGO'
       ,sp.PaymentTypeID as 'CARDTYPE', sp.CreditCardTypeID 'CARDTYPECC', sp.CCNumber as 'CCNUM', sp.CCExpirationMonth + '/' + sp.CCExpirationYear as 'CCEXPIRE'
       ,sp.CCHolderName as 'CCNAME', sp.AmountPaid as 'AMOUNTPD', sp.Amount as 'AMOUNT', sp.BalanceDue as 'BALDUE' ,ci.AmountEarned as 'AMTEARNED', ci.AmountDeferred as 'AMTDEFER'
       ,sp.PaidDate as 'PAYDATE', sp.StartIssueDate as 'STARTISS', sp.ExpireIssueDate as 'EXPIRE' ,ci.NewExpire as 'NWEXPIRE', dsp.DeliverCode as 'DELIVERCODE'
   FROM Subscriber s WITH(NoLock)
   FULL OUTER JOIN Subscription ss WITH(NoLock)
	   ON s.SubscriberID = ss.SubscriberID
   FULL OUTER JOIN Publication p WITH(NoLock)
	   ON ss.PublicationID = p.PublicationID
   FULL OUTER JOIN Action a WITH(NoLock)
	   ON ss.ActionID_Current = a.ActionID
   FULL OUTER JOIN CategoryCode cc WITH(NoLock)
	   ON a.CategoryCodeID = cc.CategoryCodeID
   FULL OUTER JOIN TransactionCode tc WITH(NoLock)
	   ON a.TransactionCodeID = tc.TransactionCodeID
   FULL OUTER JOIN Country c WITH(NoLock)
	   ON s.CountryID = c.CountryID
   FULL OUTER JOIN PriceCode pc WITH(NoLock)
	   ON ss.PublicationID = pc.PublicationID
   FULL OUTER JOIN SubscriptionPaid sp WITH(NoLock)
	   ON ss.SubscriptionID = sp.SubscriptionID
   FULL OUTER JOIN DataImportExport ci With(NoLock)
	   ON ss.SubscriberID = ci.SubscriberID
   FULL OUTER JOIN DeliverSubscriptionPaid dsp With(NoLock)
       ON sp.DeliverID = dsp.DeliverID
	WHERE s.SubscriberID is not null 
	and ss.PublisherID = @PublisherID
	and ss.PublicationID = @PublicationID
	
