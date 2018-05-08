CREATE Proc [dbo].[sp_GetActivity_Searchdetails]    
(    
 @EditionID int,    
 @BlastID int    
)    
as    
Begin    
 --SUMMARY    
 select (case when isnull(emailaddress,'') = '' then 'Anonymous (' + IPAddress + ')' else Emailaddress end) as EmailAddress,   
	Actiondate, 
	ActionValue as 'SearchText'  
 from editionactivitylog eal left outer join ecn5_communicator..emails e on e.emailID = eal.emailID    
 where    
  EditionID = @EditionID and ActionTypecode='Search' and blastID = (case when @blastID = -1 then blastID else @blastID end)    
 order by Actiondate desc    
    
End
