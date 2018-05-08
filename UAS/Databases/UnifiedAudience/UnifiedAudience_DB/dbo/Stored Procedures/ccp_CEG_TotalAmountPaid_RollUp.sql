CREATE PROCEDURE [dbo].[ccp_CEG_TotalAmountPaid_RollUp]
	@srcFile int,
	@ProcessCode varchar(50),
	@ClientId int
AS
BEGIN

	set nocount on

	DECLARE @field varchar(50) = (Select StandardField from SubscriptionsExtensionMapper WITH(NOLOCK) where CustomField = 'Total Amount Paid')
	DECLARE @demoUpdateId int = (Select C.CodeId from UAD_Lookup..Code C WITH(NOLOCK) join UAD_Lookup..CodeType CT WITH(NOLOCK) on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Demographic Update' and C.CodeValue = 'R')

	CREATE TABLE #ccp_CEG_TotalAmountPaidFinal (TotalAmountPaid varchar(50), IGrp_No uniqueidentifier)

	Select IGrp_No, SUM(Cast(CASE WHEN ISNUMERIC(Value) = 1 THEN Value ELSE 0 END as float)) as totalAmountPaid 
		into #ccp_CEG_TotalAmountPaid
		from SubscriberFinal sf WITH(NOLOCK)
		join SubscriberDemographicFinal sdf WITH(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
		where ProcessCode = @ProcessCode and sdf.MAFField = 'Amount Paid'
		group by IGrp_No
		order by IGrp_No

	DECLARE @sql varchar(max) = 'Insert into #ccp_CEG_TotalAmountPaidFinal
		Select CAST((totalAmountPaid + CAST(CASE WHEN ISNUMERIC(ISNULL(se.' + @field + ',''0'')) = 1 THEN ISNULL(se.' + @field + ',''0'') else ''0'' end as float)) as varchar(50)) as TotalAmountPaid, t.IGrp_No	 
		from #ccp_CEG_TotalAmountPaid t 
		left join Subscriptions s WITH(NOLOCK) on t.IGrp_No = s.IGrp_No
		left join SubscriptionsExtension se WITH(NOLOCK) on s.SubscriptionID = se.SubscriptionID'

	exec(@sql)

	Insert into SubscriberDemographicFinal (PubID, SFRecordIdentifier, MAFField, Value, NotExists, DateCreated, CreatedByUserID, DemographicUpdateCodeId, IsAdhoc, ResponseOther)
	Select p.PubID, sf.SFRecordIdentifier, 'Total Amount Paid', t.TotalAmountPaid, 0, GETDATE(), 1, @demoUpdateId, 1, '' 
		from #ccp_CEG_TotalAmountPaidFinal t
		join SubscriberFinal sf WITH(NOLOCK) on t.IGrp_No = sf.IGrp_No
		join Pubs p WITH(NOLOCK) on sf.PubCode = p.PubCode
		where ProcessCode = @ProcessCode

	DECLARE @sql2 varchar(max) = 'Update se set ' + @field + ' = null from #ccp_CEG_TotalAmountPaid t 
	left join Subscriptions s WITH(NOLOCK) on t.IGrp_No = s.IGrp_No
	left join SubscriptionsExtension se WITH(NOLOCK) on s.SubscriptionID = se.SubscriptionID'

	exec (@sql2)

	drop table #ccp_CEG_TotalAmountPaid
	drop table #ccp_CEG_TotalAmountPaidFinal

END
GO