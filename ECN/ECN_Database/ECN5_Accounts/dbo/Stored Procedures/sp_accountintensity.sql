-- Procedure
CREATE proc [dbo].[sp_accountintensity]
(
	@channelID int,
	@customerID int,
	@customerType varchar(50),
	@AccountExecutiveID int,
	@AccountManagerID int
)
as
Begin

	set nocount on
/*
	select	1 as basechannelID, 'abcdefghijklmnopqrustasjdfkjasdfjlkasdjflkajdfkjalksfjlkasjdflkasjdlfk' as basechannelname, 
			1 as CustomerID, 'abcdefghijklmnopqrustasjdfkjasdfjlkasdjflkajdfkjalksfjlkasjdflkasjdlfk' as CustomerName, 
			'abcdefghijklmnopqrustasjdfkjasdfjlkasdjflkajdfkjalksfjlkasjdflkasjdlfk' as CustomerType,
			'NY' as ActiveFlag, getdate() as CreateDate, 
			1 as AEID, 'abcdefghijklmnopqrustasjdfkjasdfjlkasdjflkajdfkjalksfjlkasjdflkasjdlfk' as AEName,
			1 as AMID, 'abcdefghijklmnopqrustasjdfkjasdfjlkasdjflkajdfkjalksfjlkasjdflkasjdlfk' as AMName,
			'NY'as IsStrategic,
			100 as YTD, 'NA' as vband, 'NA' as Tier, 'ABCDE' as Email, 'ABCDEABCDE' as Survey, 'ABCDEABCDE' as DE

*/
	declare @ytdtotal decimal(18,2)

	declare @cust TABLE (customerID int, YTD decimal(18,2), vpercent decimal(18,2), vband char(2))

	insert into @cust (customerID, YTD)
	select	customerID, isnull(sum(sendtotal),0) as YTD from [ECN5_COMMUNICATOR].[DBO].[BLAST] 
	where	testblast = 'N' and 
			year(sendtime) = Year(getdate())  and 
			statuscode in ('sent','deleted')  
	group by customerID

	select @ytdtotal = sum(ytd) from @cust

	if @ytdtotal > 0
	Begin
		update @cust 
		set vpercent = (ytd*100) /@ytdtotal

		update @cust 
		set vband = 'A' 
		where customerID in (select top 20 percent CustomerID from @cust order by ytd desc)

		update @cust 
		set vband = 'C' 
		where customerID in (select top 20 percent CustomerID  from @cust order by ytd)

		update @cust
		set vband='B'
		where isnull(vband,'') = ''
	end

	--select * from @cust

	select	bc.basechannelID, bc.basechannelname, C.CustomerID, C.CustomerName, C.CustomerType,
			C.ActiveFlag, C.CreatedDate, 
			C.AccountExecutiveID as AEID, s.firstname + ' ' + s.lastname as AEName,
			C.AccountManagerID  as AMID, s1.firstname + ' ' + s1.lastname as AMName,
			(case when Isnull(C.IsStrategic,0) = 0 then 'N' else 'Y' end) as IsStrategic,  --
			convert(int,isnull(YTD,0)) as YTD, 
			isnull(vband,'NA') as vband, 
			(case when Isnull(C.IsStrategic,0) = 1 or vband='A' then 'A' else isnull(vband,'NA') end) as Tier,  
			case when isnull(c.communicatorChannelID,0) > 0 then 'Y' else '' end as Email,
			case when isnull(c.collectorChannelID,0) > 0 then 'Y' + (select (case when count(SurveyID) > 0 then ' (' + ( Convert(varchar,count(case when IsActive=1 then SurveyID end)))  + '/' + Convert(varchar,count(SurveyID)) + ')' else ' (0)' end) from ECN5_collector..survey where CustomerID = c.customerID) else '' end as Survey,
			case when isnull(c.publisherChannelID,0) > 0 then 'Y' + (select (case when count(EditionID) > 0 then ' (' + Convert(varchar,count(case when status in ('active','archieve') then editionID end)) + '/' + Convert(varchar,count(*)) + ')'  else ' (0)' end) from ECN5_publisher..[PUBLICATION] p join ECN5_publisher..[EDITION] e on p.publicationID = e.publicationID where customerID = c.CustomerID) else '' end as DE
	from	[BaseChannel] bc join
			[Customer] C on bc.basechannelID = c.basechannelID left outer join 
			@cust c1 on C.customerID = c1.customerID left outer join
			staff s on c.AccountExecutiveID = s.staffID  left outer join
			staff s1 on c.AccountManagerID = s1.staffID 
			
	where	
			c.basechannelID = (case when @channelID = 0 then c.basechannelID else @channelID end) and
			c.customerID = (case when @customerID = 0 then c.customerID else @customerID end) and
			isnull(c.AccountExecutiveID,0) = (case when @AccountExecutiveID = 0 then isnull(AccountExecutiveID,0) else @AccountExecutiveID end) and
			isnull(c.AccountManagerID,0) = (case when @AccountManagerID = 0 then isnull(c.AccountManagerID,0) else @AccountManagerID end) and 
			isnull(c.customerType,'') = (case when len(@customerType) = 0 then isnull(c.customerType,'') else @customerType end)
	order by YTD desc

	set nocount off
end
