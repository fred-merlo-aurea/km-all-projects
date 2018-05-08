/*
sp_helptext sp_GetActivity_ClicksTop20
sp_helptext sp_GetActivity_ClicksDetails
sp_helptext sp_GetActivity_VisitsTop20
*/

CREATE Proc [dbo].[sp_GetActivity_TopClicks] 
(      
 @EditionID int,    
 @BlastID int,
 @TopCount int     
)      
as      
Begin      
	SET ROWCOUNT @TopCount 

	select PageNumber, eal.LinkID, case when isnull(Alias,'') = '' then LinkURL else Alias end as link, count(EAID) as clickcount, count(distinct sessionID) as  distinctclickcount    
	from     
		editionactivitylog eal join     
		link l on l.LinkID = eal.LinkID join    
		Page p on eal.pageID = p.pageID    
	where      
		eal.EditionID = @EditionID and     
		ActionTypecode='Click'   and blastID = (case when @blastID = -1 then blastID else @blastID end)    
	group by     
		PageNumber,     
		eal.LinkID,    
		case when isnull(Alias,'') = '' then LinkURL else Alias end   
	order by clickcount desc, PageNumber asc      
      
	SET ROWCOUNT 0 
End
