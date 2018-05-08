CREATE procedure [dbo].[sp_ECNToday]
(	
	@Month int,
	@year int,
	@testblast varchar(1)
)
as

Begin
	Set nocount on
	
	declare @dt varchar(10)

	set @dt = convert(varchar,@month) + '/' + '01/' + convert(varchar,@year)

		select basechannelID, basechannelName, CustomerCount, UserCount, convert(int,isnull(MTD,0)) as MTD, convert(int,isnull(YTD,0)) as YTD
	from 
	(
		select 	BaseChannelID,
			BaseChannelName,
			(select count(CustomerID) from [Customer] where BaseChannelID = [BaseChannel].BaseChannelID and [Customer].ActiveFlag = 'Y' and CreatedDate < dateadd(m,1,@dt)) as CustomerCount,
			(select count(distinct users.userid) from KMPlatform..[User] users 
								  join KMPlatform..UserClientSecurityGroupMap ucsgm with(nolock) on users.UserID = ucsgm.UserID
								  join [Customer] on ucsgm.ClientID = [Customer].PlatformClientID
								  where BaseChannelID = [BaseChannel].BaseChannelID and [Customer].ActiveFlag='Y' and users.IsActive = 1 and ucsgm.IsActive = 1 and users.DateCreated < dateadd(m,1,@dt)) as UserCount
,
			(select sum(sendtotal) from [ECN5_COMMUNICATOR].[DBO].[BLAST] where testblast = @testblast  and month(sendtime) = @Month and year(sendtime) = @year  and statuscode in ('sent','deleted') and CustomerID in 
				(select CustomerID from [Customer] where BaseChannelID = [BaseChannel].BaseChannelID)
			) as MTD,  
			(select sum(sendtotal) from [ECN5_COMMUNICATOR].[DBO].[BLAST] where testblast = @testblast and month(sendtime) <= @Month and year(sendtime) = @year and statuscode in ('sent','deleted') and  CustomerID in 
				(select CustomerID from [Customer] where BaseChannelID = [BaseChannel].BaseChannelID)
			) as YTD
	From [BaseChannel]) as  inn1
	where customercount > 0 order by 2 
	/*
	select 	BaseChannelID,
			BaseChannelName,
			(select count(CustomerID) from [Customer] where BaseChannelID = [BaseChannel].BaseChannelID) as CustomerCount,
			(select count(userid) from users join [Customer] on users.customerID = [Customer].customerID where BaseChannelID = BaseChannels.BaseChannelID) as UserCount,
			(select sum(sendtotal) from [ECN5_COMMUNICATOR].[DBO].[BLAST] where CustomerID in 
				(select CustomerID from [Customer] where BaseChannelID = [BaseChannel].BaseChannelID and month(sendtime) = @Month and year(sendtime) = @year)
			) as MTD,  
			(select sum(sendtotal) from [ECN5_COMMUNICATOR].[DBO].[BLAST] where CustomerID in 
				(select CustomerID from [Customer] where BaseChannelID = [BaseChannel].BaseChannelID and month(sendtime) <= @Month and year(sendtime) = @year)
			) as YTD
	From [BaseChannel]
	*/
End
