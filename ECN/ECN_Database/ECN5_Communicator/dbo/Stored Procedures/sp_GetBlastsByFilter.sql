CREATE proc [dbo].[sp_GetBlastsByFilter] (
		@ChannelID int,
		@CustomerID int,
		@startDate varchar(10),
		@enddate varchar(10)
)
as 
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_GetBlastsByFilter', GETDATE())
	declare @sqlstr varchar(4000)
	
	set @sqlstr = 	' SELECT 	c.customerName, b.CustomerID, b.BlastID as BlastID, l.LayoutName as MessageName, b.EmailSubject as Subject, b.SendTime as SentTime, COUNT(eal.emailID) AS Sends ' +
				  	' FROM [BLAST] b JOIN ' +
					' [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  C on b.customerID = c.customerID join ' +
					' [LAYOUT] l on l.layoutID = b.LayoutID JOIN  ' +
					' EmailActivityLog eal on b.blastID = eal.BlastID and ((ActionTypeCode = ''send'' OR ActionTypeCode = ''testsend''))  ' +
					' WHERE  ' +
					' C.baseChannelID = ' + convert(varchar(10), @ChannelID) + ' and ' 

	if @CustomerID <> 0
	Begin
		set @sqlstr = @sqlstr + ' b.CustomerID = ' + convert(varchar(10), @CustomerID) + ' AND  ' 
	End

	if len(@startDate) <> 0
	Begin
		set @sqlstr = @sqlstr + ' convert(datetime,convert(varchar(10),b.SendTime,101)) >= ''' + @startDate + ''' AND  ' 
	End

	if len(@enddate) <> 0
	Begin
		set @sqlstr = @sqlstr + ' convert(datetime,convert(varchar(10),b.SendTime,101)) <= ''' + @enddate + ''' AND  '
	End

	set @sqlstr = @sqlstr + ' b.StatusCode=''sent'' GROUP BY c.customerName,  b.CustomerID, l.LayoutName, b.EmailSubject, b.SendTime, b.BlastID ORDER BY b.SendTime DESC '

	print(@sqlstr)
	exec(@sqlstr)
End
