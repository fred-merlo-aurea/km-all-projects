CREATE procedure [dbo].[sp_NoUsageReport]   
(     
 @channelID int, 
 @CustomerID varchar(2000),   
 @fromdt varchar(10),    
 @todt varchar(10)  
)    
as
Begin
	
	Set nocount on    
    declare @sqlstring varchar(8000)

	if @fromdt = ''
		set @fromdt = '01/01/' + convert(varchar,year(getdate()))

	if @todt = ''
		set @todt = convert(varchar(10),getdate(),101)



	set @sqlstring = 
	'select	
			bc.basechannelID, 
			basechannelName, 
			customername, 
			c.contactname,
			c.Phone,
			c.Email,
			c.CreatedDate,	
			isnull(sum(sendtotal),0) as usage
	from 
			[BaseChannel] bc join [Customer] c on bc.basechannelID = c.basechannelID left outer join
			[ECN5_COMMUNICATOR].[DBO].[BLAST] b on b.CustomerID = c.customerID and testblast = ''N'' and convert(smalldatetime, convert(varchar(10),sendtime,101)) between ''' + @fromdt + ''' and ''' + @todt + ''' and statuscode in (''sent'',''deleted'') 
	' 

	 if @channelID > 0
		set @sqlstring = @sqlstring + ' where  bc.BaseChannelID = ' + convert(varchar,@channelID) + ' '

	if len(@CustomerID) > 0
		set @sqlstring = @sqlstring + ' and c.CustomerID in (' + @CustomerID + ')  '

	set @sqlstring = @sqlstring + ' group by bc.basechannelID, basechannelName, customername, c.contactname, c.Phone, c.Email, c.createddate having isnull(sum(sendtotal),0) = 0'

	--print(@sqlString)
	exec(@sqlString)
end
