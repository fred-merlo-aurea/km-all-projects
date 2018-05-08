CREATE procedure [dbo].[rpt_NewCustomerreport]
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
			c.customername, 
			c.contactname,
			c.Phone,
			c.Email,
			c.CreatedDate,
			convert(int,isnull((select sum(sendtotal) from [ECN5_COMMUNICATOR].[DBO].[BLAST] where testblast = @testblast  and month(sendtime) = @Month and year(sendtime) = @year  and statuscode in ('sent','deleted') and CustomerID = c.customerID),0)) as usage
	From [Customer] c join [BaseChannel] bc on c.BaseChannelID = bc.BaseChannelID
	where month(c.CreatedDate) = @Month and year(c.CreatedDate) = @year
End
