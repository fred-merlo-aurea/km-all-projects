-- Procedure
CREATE Proc [dbo].[sp_GetActivity_Summary]        
(        
 @EditionID int,        
 @BlastID int        
)        
as        
Begin        
 --SUMMARY        
 select ActionTypeCode,     
  case when actiontypecode = 'visit' then 1     
    when actiontypecode = 'click' then 2     
    when actiontypecode = 'refer' then 3      
    when actiontypecode = 'subscribe' then 4     
	when actiontypecode = 'print' then 5     
	when actiontypecode = 'search' then 6    
  end as 'sort' ,    
  count(distinct case 
			when actiontypecode in ('visit', 'click', 'print') then (case when isnull(emailID,0)=0 then isnull(SessionID,'') else convert(varchar,emailID) end )  
			when actiontypecode in ('subscribe', 'refer') then convert(varchar,emailID) 
			when actiontypecode in ('search') then convert(varchar,EAID) end) as 'unique',     
  case 
			when actiontypecode <> 'print' then count(EAID) else sum(PageEnd-pagestart+1) end as total   
 from editionactivitylog    
 where editionID = @EditionID  and blastID = (case when @blastID = -1 then blastID else @blastID end)    
 group by actiontypecode    
 order by sort     
        
End
