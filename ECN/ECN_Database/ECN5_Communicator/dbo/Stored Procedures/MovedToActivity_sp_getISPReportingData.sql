CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getISPReportingData] (@blastID int, @ISPs varchar(400))
as

Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getISPReportingData', GETDATE())
	declare @condition varchar(1000),
			@col varchar(4000)
	
	set @condition = ''
	set @col = ''


	SET NOCOUNT ON
	
	if len(ltrim(rtrim(@ISPs))) <> 0
	begin

		SELECT 	@condition = @condition + ' e.emailaddress like ''%@' + Items + ''' or ',
				@col = @col + 'when e.emailaddress like ''%' + Items + ''' then ''' + Items + '''' 
		FROM dbo.fn_split(@ISPs,',')
	
		set @condition = '(' + substring(@condition,1,len(@condition)-2) + ')'
		set @col  = ' case ' + substring(@col,1,len(@col)) + ' end '


		exec('select out2.items as ISPs, 
				Isnull(Sends, 0) as Sends, 
				Isnull(opens,0) as opens, 
				Isnull(clicks, 0) as clicks,
				Isnull(bounces, 0) as bounces,
				Isnull(unsubscribes, 0) as unsubscribes,
				Isnull(resends, 0) as resends,
				Isnull(forwards,  0) as forwards,
				Isnull(feedbackUnsubs,  0) as feedbackUnsubs
		from 
		(select distinct ' + @col + ' as ISPs , 
				count(case when actiontypecode = ''send'' or actiontypecode = ''textsend'' then 1 end) as Sends,
				count(case when actiontypecode = ''open'' then 1 end) as opens,
				count(case when actiontypecode = ''click'' then 1 end) as clicks,
				count(case when actiontypecode = ''bounce'' then 1 end) as bounces,
				count(case when actiontypecode = ''subscribe'' then 1 end) as unsubscribes,
				count(case when actiontypecode = ''resend'' then 1 end) as resends,
				count(case when actiontypecode = ''refer'' then 1 end) as Forwards,
				count(case when actiontypecode = ''FEEDBACK_UNSUB'' then 1 end) as feedbackUnsubs
		from 
				emailactivitylog eal join emails e on eal.emailID = e.emailID
		where 
				blastID = ' + @blastID + ' and ActionTypeCode <> ''nosend'' and ' + @condition + 
		' group by ' + @col + ' ) out1 right outer join (SELECT Items FROM dbo.fn_split(''' + @ISPs + ''','','')) out2 on out1.ISPs = out2.Items order by ISPs')
	end

end
