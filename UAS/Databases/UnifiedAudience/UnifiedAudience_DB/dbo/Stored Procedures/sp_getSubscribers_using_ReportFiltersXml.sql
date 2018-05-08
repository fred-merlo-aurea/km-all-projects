CREATE Proc [dbo].[sp_getSubscribers_using_ReportFiltersXml]
(
	@xmlFilter TEXT
)
as
--set @xmlFilter = [sp_getSubscribers_using_ReportFilters]'<filters><PubIDs></PubIDs><CategoryIDs>1</CategoryIDs><CategoryCodes>10</CategoryCodes><TransactionIDs></TransactionIDs><QsourceIDs></QsourceIDs><StateIDs>CA</StateIDs><CountryIDs></CountryIDs><Email>0</Email><Phone>1</Phone><Fax>1</Fax><Demo7></Demo7><Demo31>1</Demo31><Demo32>1</Demo32><Demo33>1</Demo33><Demo34>1</Demo34><Demo35>1</Demo35><Demo36>1</Demo36><Qfrom></Qfrom><QTo></QTo><Year></Year><AdhocColumn></AdhocColumn><AdhocValue></AdhocValue></filters>'
BEGIN

	SET NOCOUNT ON
 
	declare @FilterString varchar(1000)
	set @FilterString = ''

	exec sp_getReportFilterString @xmlFilter, @FilterString output

	if len(@FilterString) > 0
		Begin

			exec (	' Select s.SubscriptionID , s.CGRP_NO from	Subscriptions s WITH(NOLOCK) left outer join ' + 
					' Category dc WITH(NOLOCK) on s.CategoryID = dc.CategoryID left outer join ' +
					' [Transaction] dt WITH(NOLOCK) on s.TransactionID = dt.TransactionID left outer join ' + 
					' QSource dq WITH(NOLOCK) on s.QsourceID = dq.QsourceID ' +
					' where ' + @FilterString)
		
		End
	else
		Begin
			Select	s.SubscriptionID , s.CGRP_NO
			from	Subscriptions s WITH(NOLOCK)
		end

END