CREATE Proc [dbo].[sp_GetActivity_VisitDetails]    
(    
 @EditionID int,  
 @BlastID int  
)    
as    
Begin    
 --SUMMARY    
 select   
 (case when isnull(emailaddress,'') = '' then 'Anonymous' else Emailaddress end) as EmailAddress,    
 PageNumber,  
 ActionDate,    
 IPAddress as IP    
 from editionactivitylog eal join Page p on p.pageID = eal.pageID left outer join ecn5_communicator..emails e on e.emailID = eal.emailID    
 where    
  eal.EditionID = @EditionID and ActionTypecode='Visit'   and blastID = (case when @blastID = -1 then blastID else @blastID end)  
 order by ActionDate desc    
    
End
