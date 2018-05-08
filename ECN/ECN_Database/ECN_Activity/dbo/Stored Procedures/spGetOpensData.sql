CREATE  PROCEDURE [dbo].[spGetOpensData] 
(
	@blastID varchar(10), 
	@ISP varchar(100),
	@ReportType varchar(20),
	@PageNo int,  
	@PageSize int
)
as
Begin

	declare @groupID int, @query varchar (8000), @innquery varchar(5000)
	select @groupID = groupID from ecn5_communicator..[BLAST] with (nolock) where BlastID = @blastID

	if @reporttype = 'activeopens'
	Begin
	
		set @query = ' select TOP 15 COUNT(e.EmailID) AS ActionCount,  E.emailaddress, ''EmailID='' + CONVERT(VARCHAR,e.EmailID) + ''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'' ' +
						' from  BlastActivityOpens bao with (nolock) join ecn5_communicator..emails e with (nolock) on bao.emailID = e.emailID ' +
						' WHERE BlastID= ' + Convert(varchar,@blastID) + ' '

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
			from	BlastActivityOpens bao with (nolock) join ecn5_communicator..emails e  with (nolock)on bao.emailID = e.emailID 
			WHERE BlastID= @blastID AND e.emailaddress like '%' + @ISP 
			
			set @query = ' select top ' + Convert(varchar,@RecordNoEnd) + ' e.eMailID, E.emailaddress, bao.OpenTime as ActionDate, bao.BrowserInfo as ActionValue, ''EmailID='' + CONVERT(VARCHAR,e.EmailID)+''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'' ' + 
						 ' from  BlastActivityOpens bao with (nolock) join ecn5_communicator..emails e with (nolock) on bao.emailID = e.emailID ' +
						 ' WHERE BlastID= ' + Convert(varchar,@blastID) + ' AND right(emailaddress,'+ convert(varchar,len(@ISP))+') = ''' + @ISP + ''' ORDER BY OpenID DESC' 
			
		End
		Else
		Begin
			set @innquery = ' select top ' + Convert(varchar,@RecordNoEnd) + ' bao.EmailID, bao.OpenTime as ActionDate, bao.BrowserInfo as ActionValue ' + 
							' from  BlastActivityOpens bao with (nolock) '+
							' WHERE BlastID= ' + Convert(varchar,@blastID) + ' ORDER BY OpenID DESC'

			Select count(OpenID) from BlastActivityOpens bao with (nolock) WHERE BlastID= @blastID

			set @query = ' SELECT e.EmailID, e.EmailAddress, inn.ActionDate, inn.ActionValue, ''EmailID='' + CONVERT(VARCHAR,e.EmailID)+''&GroupID=' + CONVERT(VARCHAR,@groupID) + ''' AS ''URL'' '+
						 ' FROM ( ' + @innquery + ' )  inn join ecn5_communicator..emails e with (nolock) on e.EmailID = inn.EmailID'
		End
	End

	exec (@query)
End
