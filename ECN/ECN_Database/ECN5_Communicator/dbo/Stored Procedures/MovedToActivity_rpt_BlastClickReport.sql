CREATE PROCEDURE [dbo].[MovedToActivity_rpt_BlastClickReport] (@blastID int)
as

Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('rpt_BlastClickReport', GETDATE())
	SET NOCOUNT ON

	declare @layoutID int

	select @layoutID = layoutID from [BLAST] where BlastID = @blastID

	select  count(eal.EAID) as Clickcount,
			count(distinct eal.EMailID) as DistinctClickCount, 
			case when isnull(a.Alias,'') = '' then eal.ActionValue else a.Alias end as Link
	from	
			EmailActivityLog eal left outer join	
			(
				select link, la.Alias
				from linkAlias la join
						 Content c on c.contentiD = la.contentID join
						 [LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
						 l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
						 l.ContentSlot9 = c.contentID)
				WHERE  
						 l.layoutID = @layoutID and isnull(la.Alias, '') <> ''
			) a on eal.ActionValue = a.Link	
	WHERE 
			BlastID= @blastID AND 
			ActionTypeCode='click'
	group by case when isnull(a.Alias,'') = '' then eal.ActionValue else a.Alias end
	order by 1 desc

	SET NOCOUNT OFF
End
