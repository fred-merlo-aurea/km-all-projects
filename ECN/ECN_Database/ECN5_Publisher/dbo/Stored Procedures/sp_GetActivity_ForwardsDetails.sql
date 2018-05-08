CREATE Proc [dbo].[sp_GetActivity_ForwardsDetails]
(  
	@EditionID int,
	@BlastID int
)  
as  
Begin  
 select 
	ActionDate,
	e.EmailAddress,
	ActionValue
 from 
		editionactivitylog eal join 
		ecn5_communicator..emails e on e.emailID = eal.emailID  
 where  
		eal.EditionID = @EditionID and 
		ActionTypecode='refer'   and blastID = (case when @blastID = -1 then blastID else @blastID end)
 order by 1 desc
  
End
