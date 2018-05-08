CREATE proc [dbo].[spAdvertiserClickReport]
(
	@groupID int,
	@startdate date,
	@enddate date
)
as
Begin

	--select BlastID from blasts where groupID = @groupID and statuscode = 'sent' and sendtime between @startdate and @enddate

	select BlastID, EmailSubject, sendtime, Alias, URL as linkURL, linkownername as linkowner, codevalue as linktype, UniqueCount, TotalCount
	from
	(
		SELECT  BlastID, EmailSubject, layoutID, sendtime, URL, ISNULL(SUM(DistinctCount),0) AS UniqueCount, ISNULL(SUM(total),0) AS TotalCount        
		FROM (        
					SELECT  b.BlastID, b.EmailSubject, layoutID, sendtime, URL, COUNT(distinct URL) AS DistinctCount, COUNT(ClickID) AS total         
					FROM	BlastActivityClicks bac join ecn5_communicator..[BLAST] b on b.blastID = bac.blastID      
					WHERE	groupID = @groupID and 
							b.testblast='N' and 
							CAST(sendtime as date) between @startdate and @enddate 
							and statuscode = 'sent'       
					GROUP BY  b.BlastID, b.EmailSubject, layoutID, sendtime, URL, EmailID        
			 ) AS inn2  
		group by BlastID, EmailSubject, layoutID, sendtime, URL
	)	inn1 left outer join 
		ecn5_communicator..[LAYOUT] l on l.layoutID = inn1.layoutID left outer join 
		ecn5_communicator..content c on (c.contentiD = l.ContentSlot1 or c.contentiD = l.ContentSlot2 or  c.contentiD = l.ContentSlot3 or  c.contentiD = l.ContentSlot4) left outer join
		ecn5_communicator..linkalias la on la.contentID = c.contentID  and inn1.URL = la.Link left outer join
		ecn5_communicator..linkownerindex loi on loi.linkownerindexID = la.linkownerID left outer join
		ecn5_communicator..Code cod on cod.codeID = la.linktypeID
		order by 8 desc --4

End