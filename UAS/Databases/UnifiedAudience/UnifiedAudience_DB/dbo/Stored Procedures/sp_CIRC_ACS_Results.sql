CREATE PROCEDURE [dbo].[sp_CIRC_ACS_Results]
	@ProcessCode varchar(50)
AS
BEGIN
	
	SET NOCOUNT ON

	DECLARE @TranID31 int = (Select TransactionCodeID from UAD_Lookup..TransactionCode WITH(NOLOCK) where TransactionCodeValue = 31)
	DECLARE @TranID32 int = (Select TransactionCodeID from UAD_Lookup..TransactionCode WITH(NOLOCK) where TransactionCodeValue = 32)

	DECLARE @ID21Dupes int = (Select COUNT(*) from SubscriberFinal sf WITH(NOLOCK) join UAD_Lookup..TransactionCode tc WITH(NOLOCK) on sf.TransactionID = tc.TransactionCodeID where ProcessCode = @ProcessCode and IsUpdatedInLive = 0 and tc.TransactionCodeValue = 21)
	DECLARE @ID31Dupes int = (Select COUNT(*) from SubscriberFinal WITH(NOLOCK) where ProcessCode = @ProcessCode and IsUpdatedInLive = 0 and TransactionID = @TranID31)
	DECLARE @ID32Dupes int = (Select COUNT(*) from SubscriberFinal WITH(NOLOCK) where ProcessCode = @ProcessCode and IsUpdatedInLive = 0 and TransactionID = @TranID32)

	DECLARE @ACSTable table (SequenceID int, ProductCode varchar(100), ProcessCode varchar(50), TransactionValue int, OldAddressFormatted varchar(500), NewAddressFormatted varchar(500), 
							 CurrentTransactionID int, Match bit, IsPaid bit, HasNoNewAddress bit, IsNoMatch bit, DeliverabilityCode varchar(10))

	Insert into @ACSTable
	Select i.SequenceID, i.ProductCode, i.ProcessCode, i.TransactionCodeValue, 	
			i.OldAddress1, i.NewAddress1, ps.PubTransactionID,
			(CASE WHEN ps.Address1 = i.NewAddress1 THEN 1 ELSE 0 END),
			(CASE WHEN ps.PubTransactionID in (Select tc.TransactionCodeID From UAD_Lookup..TransactionCode tc where tc.TransactionCodeTypeID in (3,4)) THEN 1 ELSE 0 END),
			(CASE WHEN (i.NewAddress1 = '' or i.NewCity = '' or i.NewStateAbbreviation = '' or i.NewZipCode = '') THEN 1 ELSE 0 END),
			(CASE WHEN (ps.Address1 <> i.NewAddress1 and (NewAddress1 <> '' and NewCity <> '' and NewStateAbbreviation <> '' and NewZipCode <> '')) THEN 1 ELSE 0 END),
			i.DeliverabilityCode
			from ACSFileDetail i WITH(NOLOCK) 					
				join Pubs p WITH(NOLOCK) on i.ProductCode = p.PubCode
				join PubSubscriptions ps WITH(NOLOCK) on i.SequenceID = ps.SequenceID and p.PubID = ps.PubID			
			where i.ProcessCode = @ProcessCode

	DECLARE @Dupes int = (Select Count(*) from SubscriberFinal WITH(NOLOCK) where ProcessCode = @ProcessCode and IsUpdatedInLive = 0)
		
	Select [Type], [Count] from 
	(
		Select 1 as 'Row','Total Records In File' as 'Type', Cast(Count(*) as varchar(15)) as 'Count' from ACSFileDetail WITH(NOLOCK) where ProcessCode = @ProcessCode
		union
		Select 2,'Records Matched', Cast(Count(*) as varchar(15)) from @ACSTable where ProcessCode = @ProcessCode
		union
		Select 3,'No Record Exists', Cast(Count(*) as varchar(15))
			from ACSFileDetail i WITH(NOLOCK) 
				left join Pubs p WITH(NOLOCK) on i.ProductCode = p.PubCode
				left join PubSubscriptions ps WITH(NOLOCK) on i.SequenceID = ps.SequenceID and p.PubID = ps.PubID
			where i.ProcessCode = @ProcessCode and ps.PubSubscriptionID is null
		union
		Select 4,'',''
		union
		Select 5,'Records Processed',''
		union
		Select 6,'Transaction 21', Cast((Count(*) - @ID21Dupes) as varchar(15)) from @ACSTable where TransactionValue = 21 and Match = 'true' and IsPaid = 0
		union
		Select 7,'Transaction 31', Cast((Count(*) - @ID31Dupes) as varchar(15)) from @ACSTable where TransactionValue = 31 and IsPaid = 0 and CurrentTransactionID = @TranID31
		union
		Select 8,'Transaction 32', Cast((Count(*) - @ID32Dupes) as varchar(15)) from @ACSTable where TransactionValue = 32 and IsPaid = 0 and CurrentTransactionID = @TranID32
		union
		Select 9,'',''
		union
		Select 10,'Records Ignored',''
		union
		Select 11,'Paid Records Ignored', Cast(Count(*) as varchar(15)) from @ACSTable i where IsPaid = 1
		union	
		Select 12,'Free Inactive (K,G,W,Blank)', Cast(Count(*) as varchar(15)) from @ACSTable where DeliverabilityCode in ('K','G','W','') and TransactionValue in (31,32)
		union
		Select 13,'No Address Match', Cast(Count(*) as varchar(15)) from @ACSTable where TransactionValue = 21 and Match = 0 and IsPaid = 0
		union
		--Select 13,'No Old Address', Cast(Count(*) as varchar(15)) from @ACSTable where OldAddressFormatted = '' and TransactionValue = 21
		--union
		----Select 12,'No New Address', Cast(Count(*) as varchar(15)) from @ACSTable where HasNoNewAddress = 1 and TransactionValue not in (31,32) and IsPaid = 0
		----union
		----Select 13,'No Address Match', Cast(Count(*) as varchar(15)) from @ACSTable where IsNoMatch = 1 and IsPaid = 0	
		----union
		Select 14,'Duplicates', Cast(@Dupes as varchar(15))
	) a
	order by a.Row

END