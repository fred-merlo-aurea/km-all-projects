CREATE proc [dbo].[MovedToActivity_sp_AdvertiserClickReport]
(
	@groupID int,
	@startdate varchar(20),
	@enddate varchar(20)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_AdvertiserClickReport', GETDATE())

	set @startdate = @startdate + ' 00:00:00'
	set @enddate = @enddate + ' 23:59:59'
	--select BlastID from [BLAST] where groupID = @groupID and statuscode = 'sent' and sendtime between @startdate and @enddate

	select BlastID, EmailSubject, sendtime, (Case when Alias is null then actionvalue else Alias end) as link, actionvalue as linkURL, linkownername as linkowner, codevalue as linktype, UniqueCount, TotalCount
	from
	(
		SELECT  BlastID, EmailSubject, layoutID, sendtime, actionvalue, ISNULL(SUM(DistinctCount),0) AS UniqueCount, ISNULL(SUM(total),0) AS TotalCount        
		FROM (        
					SELECT  b.BlastID, b.EmailSubject, layoutID, sendtime, actionvalue, COUNT(distinct ActionValue) AS DistinctCount, COUNT(EmailID) AS total         
					FROM	EmailActivityLog eal join [BLAST] b on b.blastID = eal.blastID      
					WHERE	groupID = @groupID and 
							b.testblast='N' and 
							sendtime between @startdate and @enddate and 
							ActionTypeCode = 'click' AND 
							statuscode = 'sent'       
					GROUP BY  b.BlastID, b.EmailSubject, layoutID, sendtime, ActionValue, EmailID        
			 ) AS inn2  
		group by BlastID, EmailSubject, layoutID, sendtime, actionvalue
	)	inn1 left outer join 
		[LAYOUT] l on l.layoutID = inn1.layoutID left outer join 
		content c on (c.contentiD = l.ContentSlot1 or c.contentiD = l.ContentSlot2 or  c.contentiD = l.ContentSlot3 or  c.contentiD = l.ContentSlot4) left outer join
		linkalias la on la.contentID = c.contentID  and inn1.actionvalue = la.Link left outer join
		linkownerindex loi on loi.linkownerindexID = la.linkownerID left outer join
		Code cod on cod.codeID = la.linktypeID
		order by 8 desc --4

End
