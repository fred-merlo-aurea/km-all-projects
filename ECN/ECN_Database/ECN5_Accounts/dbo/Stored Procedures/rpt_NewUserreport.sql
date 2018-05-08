CREATE procedure [dbo].[rpt_NewUserreport]
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

	select 	bc.BaseChannelID,
			bc.BaseChannelName,
			c.customerID,
			c.customername, 
			u.username,	
			u.DateCreated,		
			convert(int,isnull((select sum(sendtotal) from [ECN5_COMMUNICATOR].[DBO].[BLAST] where testblast = @testblast  and month(sendtime) = @Month and year(sendtime) = @year  and statuscode in ('sent','deleted') and userID = u.userID),0)) as usage
	From 
			KMPlatform..[User] u
			join KMPlatform..Client cl on u.defaultclientid = cl.clientid
			join [Customer] c on cl.ClientID = c.PlatformClientID
			join [BaseChannel] bc on c.BaseChannelID = bc.BaseChannelID
	where 
			month(u.DateCreated) = @Month and 
			year(u.DateCreated) = @year
End
