CREATE Proc [dbo].[sp_GetActivity_PrintsPerPage]      
(      
 @EditionID int,      
 @BlastID int      
)      
as      
Begin      
 --SUMMARY      
 select pagenumber,       
   Convert(varchar(10), count(distinct case when isnull(emailID,'') = '' then convert(varchar,sessionID) else  convert(varchar,emailID) end)) as 'unique',      
   count(case when isnull(emailID,'') = '' then convert(varchar,sessionID) else convert(varchar,emailID) end) as total      
 from       
   page p join editionactivitylog eal on p.editionID = eal.editionID      
 where       
   p.editionID = @EditionID and       
   ActionTypecode='print' and       
   pagenumber between pagestart and pageend      
 group by pageNumber      
 order by total desc      
      
End
