CREATE  PROCEDURE [dbo].[spGetISPReportingData] (@blastID int, @ISPs varchar(400))
as
Begin
	declare @condition varchar(1000), 
			@col varchar(4000)
	
	set @condition = ''
	set @col = ''

	SET NOCOUNT ON
	
	create table  #tempEmailActivityLog  (EmailID int, BlastID int, ActionTypeCode varchar(255), ActionDate datetime, 
				ActionValue varchar(500), ActionNotes varchar(250)) 									
	 
	if len(ltrim(rtrim(@ISPs))) <> 0
	begin
	
		SELECT 	@condition = @condition + ' e.emailaddress like ''%@' + Items + ''' or ',
				@col = @col + 'when e.emailaddress like ''%' + Items + ''' then ''' + Items + ''''
		FROM ecn5_communicator.dbo.fn_split(@ISPs,',')
	
		set @condition = '(' + substring(@condition,1,len(@condition)-2) + ')'
		set @col  = ' case ' + substring(@col,1,len(@col)) + ' end '
				
		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, 'send' as ActionTypeCode, SendTime as 'ActionDate', SMTPMessage as 'ActionValue'   
				from BlastActivitySends where BlastID = @blastID
				
		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, 'click' as ActionTypeCode, ClickTime as 'ActionDate', URL as 'ActionValue'
				from BlastActivityClicks where BlastID = @blastID

		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, 'open' as ActionTypeCode, OpenTime as 'ActionDate', BrowserInfo as 'ActionValue'   
				from BlastActivityOpens where BlastID = @blastID

		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, 'bounce' as ActionTypeCode, BounceTime as 'ActionDate', bc.BounceCode as 'ActionValue'   
				from BlastActivityBounces bab left join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID where BlastID = @blastID

		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, case when usc.UnsubscribeCode = 'subscribe' OR usc.UnsubscribeCode = 'FEEDBACK_UNSUB' then usc.UnsubscribeCode else '' end 
				as ActionTypeCode, UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as 'ActionValue'   
				from BlastActivityUnSubscribes baus left join UnsubscribeCodes usc on baus.UnsubscribeCodeID = usc.UnsubscribeCodeID 
				where BlastID = @blastID
		
		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, 'resend' as ActionTypeCode, ResendTime as 'ActionDate', '' as 'ActionValue'   
				from BlastActivityResends bars where BlastID = @blastID
				
		insert into #tempEmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue) 
		select EmailID, BlastID, 'refer' as ActionTypeCode, ReferTime as 'ActionDate', '' as 'ActionValue'   
				from BlastActivityRefer barf where BlastID = @blastID
		
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
				#tempEmailActivityLog eal join ecn5_communicator..emails e on eal.emailID = e.emailID
		where 
				blastID = ' + @blastID + ' and ActionTypeCode <> ''nosend'' and ' + @condition +
		' group by ' + @col + ' ) out1 right outer join (SELECT Items FROM ecn5_communicator.dbo.fn_split(''' + @ISPs + ''','','')) out2 on out1.ISPs = out2.Items order by ISPs')
	end
	
	drop table #tempEmailActivityLog
	
end
