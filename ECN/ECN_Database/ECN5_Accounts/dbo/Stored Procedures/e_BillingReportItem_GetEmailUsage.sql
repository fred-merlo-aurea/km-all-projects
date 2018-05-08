CREATE PROCEDURE [dbo].[e_BillingReportItem_GetEmailUsage] 
	@CustomerIDs varchar(500),
	@StartDate datetime,
	@EndDate datetime,
	@FieldsToInclude varchar(200),
	@ColumnSQL varchar(500)
AS
BEGIN
	SELECT @EndDate = DATEADD(hour, 23, DATEADD(minute, 59,DATEADD(second, 59,@EndDate)))

	--declare @TempCreation varchar(500) = 'DECLARE @Temp table(BaseChannelName varchar(100),CustomerName varchar(100), InvoiceText varchar(1000), AmountOfItems int)'

	exec (--@TempCreation + ' INSERT INTO @Temp
	'select bc.BaseChannelName,cu.CustomerName ' + @ColumnSQL + ',b.SendTime, CONVERT(int,ISNULL(b.SendTotal,0)) as ''AmountOfItems''
	from ECN5_Communicator..Blast b with(nolock)
	join ECN5_Communicator..CampaignItem ci with(nolock) on ci.CampaignItemID = (SELECT CampaignItemID FROM ECN5_Communicator..CampaignItemBlast where BlastID = b.BlastID)
	join ECN5_Communicator..Campaign c with(nolock) on ci.CampaignID = c.CampaignID
	join Customer cu with(nolock) on c.CustomerID = cu.CustomerID
	join Basechannel bc with(nolock) on cu.BaseChannelID = bc.BaseChannelID
	join ECN5_Communicator..Groups g with(nolock) on b.GroupID = g.GroupID
	where c.CustomerID in (' + @CustomerIDs + ') and ISNULL(b.SendTotal, 0) > 0 and b.statuscode in(''sent'',''deleted'') and b.SendTime between ''' + @StartDate + ''' and ''' + @EndDate + ''' 
	ORDER BY bc.BaseChannelName, cu.CustomerName, b.sendtime')
	
	--SELECT * FROM @Temp')
END