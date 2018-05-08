CREATE proc [dbo].[sp_GetBlastComparision_Group]
(
	@groupsIDs  XML,
	@blastsnum int
)
as
    
Begin        
			declare @blastIDs xml;	
			
			declare @Groups TABLE (gID int)			
			insert into @Groups 
			SELECT GroupValues.ID.value('./@GroupID','INT')FROM @groupsIDs.nodes('/Groups') as GroupValues(ID) 
			
			SET @blastIDs=(Select top (@blastsnum) BlastID from ecn5_communicator.dbo.[BLAST] as Blasts
			where GroupID IN (select * from @Groups) and StatusCode='sent'
			order by SendTime DESC
			for XML auto)
			
			exec [sp_GetBlastReportComparision] @blastIDs;
			
			
			
			--Select top (@blastsnum) BlastID from ecn5_communicator.dbo.[BLAST] as Blasts
			--where GroupID IN (select * from @Groups)
			--order by SendTime DESC
			--for XML auto
			
End
