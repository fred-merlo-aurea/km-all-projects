CREATE PROCEDURE [dbo].[rptBlastClickReport] (@blastID int)
as

Begin

	SET NOCOUNT ON

	declare @layoutID int

	select @layoutID = layoutID from ecn5_communicator..[BLAST] where BlastID = @blastID

	select  count(bac.ClickID) as Clickcount,
			count(distinct bac.EMailID) as DistinctClickCount, 
			case when isnull(a.Alias,'') = '' then bac.URL else a.Alias end as Link
	from	
			BlastActivityClicks bac left outer join	
			(
				select link, la.Alias
				from ecn5_communicator..linkAlias la join
						 ecn5_communicator..Content c on c.contentiD = la.contentID join
						 ecn5_communicator..[LAYOUT] L on (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR 
						 l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR 
						 l.ContentSlot9 = c.contentID)
				WHERE  
						 l.layoutID = @layoutID and isnull(la.Alias, '') <> ''
			) a on bac.URL = a.Link	
	WHERE 
			BlastID= @blastID
	group by case when isnull(a.Alias,'') = '' then bac.URL else a.Alias end
	order by 1 desc

	SET NOCOUNT OFF
End
