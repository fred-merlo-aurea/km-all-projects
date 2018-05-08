CREATE Proc [dbo].[sp_GetActivity_ClicksDetails]  
(    
 @EditionID int,  
 @BlastID int  
)    
as    
Begin    
 select   
 ActionDate,  
 PageNumber,  
 case when isnull(Alias,'') = '' then LinkURL else Alias end as link,    
 (case when isnull(emailaddress,'') = '' then 'Anonymous (' + IPAddress + ')' else Emailaddress end) as EmailAddress    
 from   
  editionactivitylog eal join   
  link l on l.LinkID = eal.LinkID join  
  Page p on eal.pageID = p.pageID left outer join   
  ecn5_communicator..emails e on e.emailID = eal.emailID    
 where    
  eal.EditionID = @EditionID and   
  ActionTypecode='Click'   and blastID = (case when @blastID = -1 then blastID else @blastID end)  
 order by 1 desc  
    
End
