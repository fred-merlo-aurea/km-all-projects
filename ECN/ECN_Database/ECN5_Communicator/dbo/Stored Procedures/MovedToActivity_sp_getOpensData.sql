CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getOpensData] 
(
	@blastID varchar(10), 
	@ISP varchar(100),
	@ReportType varchar(20),
	@PageNo int,  
	@PageSize int
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getOpensData', GETDATE())
	declare @groupID int, @query varchar (8000), @innquery varchar(5000)
	select @groupID = groupID from [BLAST] where BlastID = @blastID

	if @reporttype = 'activeopens'
	Begin

		set @query = ' select TOP 15 COUNT(e.EmailID) AS ActionCount,  E.emailaddress, ''EmailID='' + CONVERT(VARCHAR,e.EmailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'' ' +
						' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID ' +
						' WHERE BlastID= ' + Convert(varchar,@blastID) + ' AND ActionTypeCode=''open'' '

		if len(rtrim(ltrim(@ISP))) > 0
			set @query = @query + ' AND e.emailaddress like ''%' + @ISP + ''' '

		set @query = @query + ' group by e.EmailID, Emailaddress order by ActionCount desc '
	end
	else if @reporttype = 'allopens'
	Begin
		declare @RecordNoStart int,  
				@RecordNoEnd int  
	  
		--set @PageNo = @PageNo - 1
		Set @RecordNoStart = (@PageNo * @PageSize) + 1  
		Set @RecordNoEnd = (@PageNo * @PageSize) + @PageSize  

		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			-- Get the Total records count 
			select	Count(e.emailID) 
			from	EmailActivityLog eal join emails e on eal.emailID = e.emailID 
			WHERE BlastID= @blastID AND ActionTypeCode='open' AND e.emailaddress like '%' + @ISP 
			
			set @query = ' select top ' + Convert(varchar,@RecordNoEnd) + ' e.eMailID, E.emailaddress, eal.ActionDate, eal.ActionValue, ''EmailID='' + CONVERT(VARCHAR,e.EmailID)+''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'' ' + 
						 ' from  EmailActivityLog eal join emails e on eal.emailID = e.emailID ' +
						 ' WHERE BlastID= ' + Convert(varchar,@blastID) + ' AND ActionTypeCode=''open'' AND right(emailaddress,'+ convert(varchar,len(@ISP))+') = ''' + @ISP + ''' ORDER BY EAID DESC' 
			
		End
		Else
		Begin
			set @innquery = ' select top ' + Convert(varchar,@RecordNoEnd) + ' eal.EmailID, eal.ActionDate, eal.ActionValue ' + 
							' from  EmailActivityLog eal '+
							' WHERE BlastID= ' + Convert(varchar,@blastID) + ' AND ActionTypeCode=''open'' ORDER BY EAID DESC'

			Select count(EAID) from EmailActivityLog WHERE BlastID= @blastID AND ActionTypeCode='open'

			set @query = ' SELECT e.EmailID, e.EmailAddress, inn.ActionDate, inn.ActionValue, ''EmailID='' + CONVERT(VARCHAR,e.EmailID)+''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'' '+
						 ' FROM ( ' + @innquery + ' )  inn join Emails e on e.EmailID = inn.EmailID'
		End
	End

	exec (@query)
End
