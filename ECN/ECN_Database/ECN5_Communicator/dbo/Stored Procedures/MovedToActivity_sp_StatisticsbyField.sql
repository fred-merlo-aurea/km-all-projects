CREATE proc [dbo].[MovedToActivity_sp_StatisticsbyField]
(
	@blastID int,
	@field varchar(100)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_StatisticsbyField', GETDATE())
	declare 
		@groupdatafieldsID int,
		@groupID int

	create table #emails (emailID int, Field varchar(255))

	select @groupID = groupID from [BLAST] where blastID = @blastID

	if substring(@field,1,3) = 'UDF'
		select @groupdatafieldsID = groupdatafieldsID from groupdatafields where groupID = @groupID and shortname = Replace(@field,'UDF-','')

	if @groupdatafieldsID > 0
	Begin
		insert into #emails
		select emailID, (select top 1 datavalue from emaildatavalues where emailID = emailactivitylog.emailID and groupdatafieldsID = @groupdatafieldsID )
		from emailactivitylog 
		where blastID = @blastID and actiontypecode = 'send'
	end
	else
	Begin
		exec ('insert into #emails select eal.emailID, ' + @field + ' from emailactivitylog eal join emails e on eal.emailID = e.emailID where blastID = ' + @blastID + ' and actiontypecode = ''send''')
	End

	select		Field, 
			isnull(max(case when actiontype='send' then uniquecount end),0) as usend,
			isnull(max(case when actiontype='Hardbounce' then uniquecount end),0) as uHbounce,
			isnull(max(case when actiontype='Softbounce' then uniquecount end),0) as uSbounce,
			isnull(max(case when actiontype='open' then uniquecount end),0) as uopen,
			isnull(max(case when actiontype='open' then TotalCount end),0) as topen,
			isnull(max(case when actiontype='click' then uniquecount end),0) as uClick,
			isnull(max(case when actiontype='click' then TotalCount end),0) as tClick

	from
	(
		SELECT  
			isnull(e.Field,'') as Field, COUNT(DISTINCT eal.EmailID) AS UniqueCount,COUNT(eal.EmailID) AS TotalCount, 
			case	when actiontypecode = 'bounce' and actionvalue='hardbounce' then 'Hardbounce'
					when actiontypecode = 'bounce' and actionvalue='softbounce' then 'Softbounce'
					else actiontypecode
			end as actiontype 
		FROM	
				EmailActivityLog eal join #emails e on eal.emailID = e.emailID
		WHERE	
				blastID = @blastID  and (actiontypecode = 'send' or actiontypecode = 'open' or actiontypecode = 'bounce' )
		GROUP BY isnull(e.Field,''), case	when actiontypecode = 'bounce' and actionvalue='hardbounce' then 'Hardbounce'
					when actiontypecode = 'bounce' and actionvalue='softbounce' then 'Softbounce'
					else actiontypecode
			end    
		UNION        
		SELECT Field, ISNULL(SUM(DistinctCount),0) AS UniqueCount, ISNULL(SUM(total),0) AS TotalCount,'click'         
		FROM (        
					SELECT  isnull(e.Field,'') as Field, COUNT(distinct ActionValue) AS DistinctCount, COUNT(eal.EmailID) AS total         
					FROM   EmailActivityLog eal join #emails e on eal.emailID = e.emailID
					WHERE  ActionTypeCode = 'click' AND blastID = @blastID      
					GROUP BY isnull(e.Field,''), ActionValue, eal.EmailID        
			 ) AS inn  
		group by Field
	) inn
	group by Field
	order by 1 asc

	drop table #emails
End
