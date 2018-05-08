CREATE proc [dbo].[spStatisticsbyField]
(
	@blastID int,
	@field varchar(100)
)
as
Begin

	declare 
		@groupdatafieldsID int,
		@groupID int
	
	IF LEN(Ltrim(rtrim(@FIELD))) = 0
		set @field = ''''' as field'

	create table #emails (emailID int, Field varchar(255))

	select @groupID = groupID from ecn5_communicator..[BLAST] where blastID = @blastID

	if substring(@field,1,3) = 'UDF'
		select @groupdatafieldsID = groupdatafieldsID from ecn5_communicator..groupdatafields where groupID = @groupID and shortname = Replace(@field,'UDF-','')

	if @groupdatafieldsID > 0
	Begin
		insert into #emails
		select emailID, (select top 1 datavalue from ecn5_communicator..emaildatavalues where emailID = bas.emailID and groupdatafieldsID = @groupdatafieldsID )
		from BlastActivitySends bas
		where blastID = @blastID
	end
	else
	Begin
		exec ('insert into #emails select bas.emailID, ' + @field + ' from BlastActivitySends bas join ecn5_communicator..emails e on bas.emailID = e.emailID where blastID = ' + @blastID)
	End

	select		Field, 
			isnull(max(case when actiontype='send' then uniquecount end),0) as usend,
			isnull(max(case when actiontype='Hardbounce' then uniquecount end),0) as uHbounce,
			isnull(max(case when actiontype='Softbounce' then uniquecount end),0) as uSbounce,
			isnull(max(case when actiontype='open' then uniquecount end),0) as uopen,
			isnull(max(case when actiontype='open' then TotalCount end),0) as topen,
			isnull(max(case when actiontype='click' then uniquecount end),0) as uClick,
			isnull(max(case when actiontype='click' then TotalCount end),0) as tClick,
			isnull(MAX(case when actiontype='clickthrough' then uniquecount end),0) as clickthrough

	from
	(
		SELECT isnull(e.Field,'') as Field, COUNT(DISTINCT bab.BounceID) AS UniqueCount,COUNT(bab.BounceID) AS TotalCount, case	when bc.BounceCode='hardbounce' then 'Hardbounce' when bc.BounceCode='softbounce' then 'Softbounce' end as actiontype
		FROM BlastActivityBounces bab WITH (NOLOCK) JOIN BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID JOIN #emails e on bab.emailID = e.emailID
		WHERE blastID = @blastID
		GROUP BY isnull(e.Field,''), case when bc.BounceCode='hardbounce' then 'Hardbounce' when bc.BounceCode='softbounce' then 'Softbounce' end    
		UNION 
		SELECT isnull(e.Field,'') as Field, COUNT(DISTINCT bao.EmailID) AS UniqueCount,COUNT(bao.EmailID) AS TotalCount, 'open'
		FROM BlastActivityOpens bao WITH (NOLOCK) JOIN #emails e on bao.emailID = e.emailID
		WHERE blastID = @blastID
		GROUP BY isnull(e.Field,'')    
		UNION  			
		SELECT isnull(e.Field,'') as Field, COUNT(DISTINCT bas.SendID) AS UniqueCount,COUNT(bas.SendID) AS TotalCount, 'send'
		FROM BlastActivitySends bas WITH (NOLOCK) JOIN #emails e on bas.emailID = e.emailID
		WHERE blastID = @blastID
		GROUP BY isnull(e.Field,'')    
		UNION        
		SELECT Field, ISNULL(SUM(DistinctCount),0) AS UniqueCount, ISNULL(SUM(total),0) AS TotalCount,'click'         
		FROM (        
					SELECT  isnull(e.Field,'') as Field, COUNT(distinct URL) AS DistinctCount, COUNT(bac.ClickID) AS total         
					FROM   BlastActivityClicks bac WITH (NOLOCK) join #emails e on bac.emailID = e.emailID
					WHERE  bac.blastID = @blastID      
					GROUP BY isnull(e.Field,''), URL, bac.EmailID          
			 ) AS inn  
		group by Field
		UNION
		SELECT isnull(e.Field,'') as Field, ISNULL(COUNT(distinct bac.EmailID),0) as UniqueCount,0 as TotalCount, 'clickthrough'
		FROM BlastActivityClicks bac with(nolock)
		JOIN #emails e on bac.EmailID = e.emailID
		WHERE bac.blastid = @BlastID
		GROUP BY isnull(e.Field,'')
	) inn
	group by Field
	order by 1 asc

	drop table #emails
End
