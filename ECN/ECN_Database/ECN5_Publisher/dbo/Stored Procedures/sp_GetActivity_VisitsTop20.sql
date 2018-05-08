CREATE Proc [dbo].[sp_GetActivity_VisitsTop20]  
(  
 @EditionID int,  
 @BlastID int  
)  
as  
Begin  
 --SUMMARY  
 select top 20 (case when isnull(emailaddress,'') = '' then 'Anonymous' else Emailaddress end) as EmailAddress,  
   count(distinct SessionID) as 'count'  
 from editionactivitylog eal left outer join ecn5_communicator..emails e on e.emailID = eal.emailID  
 where  
  EditionID = @EditionID and ActionTypecode='Visit' and blastID = (case when @blastID = -1 then blastID else @blastID end)  
 group by (case when isnull(emailaddress,'')  = '' then 'Anonymous' else emailaddress end)  
 order by 2 desc  
  
End
