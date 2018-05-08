CREATE PROCEDURE [dbo].[sp_bouncescoring]      
   
AS      
Begin    
	
	set nocount on  
	  
 /*---------------------------- Process Top 2500 sends ----------------------------*/  
  
 declare @sends table  
 (      
   EmailID  int,      
   blastID  int  
  )  
  
 insert into @sends  
 select distinct top 25000  eal.EmailID, eal.blastID  
 from emailactivitylog eal     
 where actionTypecode = 'send' and processed='n'  
  
 -- Update emails - decrement bouncescore with -1 for each send  
 update emails       
 set  bouncescore = (case when (ISNULL(bouncescore,0) + (b.bscore * -1)) < 0 then -1 else (ISNULL(bouncescore,0) + (b.bscore * -1)) end)       
 from emails e join       
   (select emailID, count(emailID) as bscore from @sends group by emailID) b on e.emailID = b.emailID       
 --print(nchar(13))
 print (convert(varchar,@@ROWCOUNT) + ' SEND records updated in Emails Table '+ nchar(13))
	 
 
 /* Update the EAL Table & set the Processed Flag to Y.*/      
 update emailactivitylog      
 set processed ='Y'       
 from emailactivitylog eal join @sends s on eal.emailID = s.emailID and eal.blastID = s.blastID      
 where eal.ActionTypeCode = 'send' and eal.processed='n'      
 
	print (convert(varchar,@@ROWCOUNT) + ' SEND records updated in EmailActivityLog Table '+ nchar(13)) 

 /*---------------------------- Process Bounce ----------------------------*/   
  
  declare @bounce table
  (      
   BounceID int identity(1,1),      
   EmailID  int,      
   blastID  int      
   )      
        
   declare @bouncenotprocessed table       
   (      
    EmailID  int,      
    blastID  int      
   )    
  
  /* Create a TMP TBL with all the distinct EmailID & BlastID from the param passed.*/      
   insert into @bounce      
   select distinct top 2500 emailID, blastID from emailactivitylog      
   where (actiontypecode='bounce' and actionvalue in ('hard','hardbounce')) and processed='N'      
         
      
  /* Create a TMP TBL with all the distinct EmailID & BlastID that were not processed before.*/       
   insert into @bouncenotprocessed      
   select EmailID, blastID      
   from @bounce       
   where BounceID not in       
     (      
   select BounceID from @bounce b join emailactivitylog eal on b.emailID = eal.emailID and b.blastID = eal.blastID      
   where eal.processed = 'Y' and eal.actiontypecode='bounce' and eal.actionvalue in ('hard','hardbounce')      
     )      
   group by EmailID, blastID      
         
  /* Update Emails Table with the bounce Score.     
     for a non processed Send add (-1)     
     for a non processed Bounce add (+2)    
     In cases where there are duplicate bounces for same Email & BlastID,    
     will be filtered in the above @bouncenotprocessed TMP Table.    
  */     
   update emails       
   set  bouncescore = ISNULL(bouncescore,0) + (b.bscore * 2)      
   from emails e join       
     (select emailID, count(emailID) as bscore from @bouncenotprocessed group by emailID) b on e.emailID = b.emailID       

	print (convert(varchar,@@ROWCOUNT) + ' hard bounce records updated in Emails Table '+ nchar(13))

      
  /* Update the EAL Table & set the Processed Flag to Y on all Hard Bounced Non-Processed records.*/      
   update emailactivitylog      
   set processed ='Y'       
   from emailactivitylog eal join @bouncenotprocessed b on eal.emailID = b.emailID and eal.blastID = b.blastID      
   where eal.ActionTypeCode = 'bounce' and eal.actionvalue in ('hard','hardbounce') and eal.processed='n'      

	print (convert(varchar,@@ROWCOUNT) + ' hard bounce records updated in EmailActivityLog Table '+ nchar(13)) 

        
end
