CREATE  proc [dbo].[MovedToActivity_sp_BlastDeliveryReport]  
(  
  @customerID int,  
  @startdate varchar(20),  
  @enddate varchar(20) ,
  @Unique bit = 1

)  
as  
  
Begin 
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_BlastDeliveryReport', GETDATE())
 
  
 set @startdate = @startdate + ' 00:00:00 '   
 set @enddate = @enddate + '  23:59:59'  


if (@Unique = 1)
Begin
  
 select  b1.blastID, b1.sendtime, b1.emailsubject,  
   sum(case when  actiontypecode='send' then dcounts else 0 end) as sendtotal,  
   sum(case when  actiontypecode='send' then dcounts else 0 end) - (sum(case when  actiontypecode='softbounce' then dcounts else 0 end) + sum(case when  actiontypecode='bounce' then dcounts else 0 end) +sum(case when  actiontypecode='hardbounce' then dcounts else 0 end)) as  Delivered,  
   sum(case when  actiontypecode='softbounce' then dcounts else 0 end) as softbouncetotal,  
   sum(case when  actiontypecode='hardbounce' then dcounts else 0 end) as hardbouncetotal,  
   0 as OtherBouncetotal,  
   sum(case when  actiontypecode='softbounce' then dcounts else 0 end) + sum(case when  actiontypecode='bounce' then dcounts else 0 end) +sum(case when  actiontypecode='hardbounce' then dcounts else 0 end) as bouncetotal,  
   sum(case when  actiontypecode='open' then dcounts else 0 end) as opentotal,  
   sum(case when  actiontypecode='click' then dcounts else 0 end) as clicktotal,  
   sum(case when  actiontypecode='subscribe' then dcounts else 0 end) as unsubscribetotal, g.groupname,
   sum(case when  actiontypecode='suppressed' then dcounts else 0 end) as suppressedtotal 
 from  
 (  
	  SELECT  eal.blastID,   
		COUNT(DISTINCT eal.EmailID) AS dcounts,  
		case  when actiontypecode = 'bounce' and actionvalue = 'softbounce' then 'softbounce'  
		   when actiontypecode = 'bounce' and (actionvalue = 'hardbounce' or actionvalue = 'hard') then 'hardbounce'  
		  else actiontypecode   
		end as actiontypecode  
	  FROM    
		EmailActivityLog eal with (NOLOCK)   
	  where   
		eal.blastID in (select blastID from [BLAST] b with (NOLOCK) where b.customerID = @customerID and sendtime between @startdate and @enddate and statuscode = 'sent' and b.testblast='N') and   
		(actiontypecode = 'open' or actiontypecode = 'click' or actiontypecode = 'send'  or actiontypecode = 'bounce' or actiontypecode = 'subscribe' or ActionTypeCode='suppressed')  
	  GROUP BY    
		eal.blastID,   
		case  when actiontypecode = 'bounce' and actionvalue = 'softbounce' then 'softbounce'  
		   when actiontypecode = 'bounce' and (actionvalue = 'hardbounce' or actionvalue = 'hard') then 'hardbounce'  
		  else actiontypecode   
		end     
 ) inn join [BLAST] b1 on inn.blastID = b1.blastID left outer join groups g on b1.groupID = g.groupID  
 group by b1.BlastID, b1.sendtime, b1.emailsubject, g.groupname  
 order by b1.sendtime asc  
 end
 else
 Begin
 select  b1.blastID, b1.sendtime, b1.emailsubject,  
   sum(case when  actiontypecode='send' then dcounts else 0 end) as sendtotal,  
   sum(case when  actiontypecode='send' then dcounts else 0 end) - (sum(case when  actiontypecode='softbounce' then dcounts else 0 end) + sum(case when  actiontypecode='bounce' then dcounts else 0 end) +sum(case when  actiontypecode='hardbounce' then dcounts else 0 end)) as  Delivered,  
   sum(case when  actiontypecode='softbounce' then dcounts else 0 end) as softbouncetotal,  
   sum(case when  actiontypecode='hardbounce' then dcounts else 0 end) as hardbouncetotal,  
   0 as OtherBouncetotal,  
   sum(case when  actiontypecode='softbounce' then dcounts else 0 end) + sum(case when  actiontypecode='bounce' then dcounts else 0 end) +sum(case when  actiontypecode='hardbounce' then dcounts else 0 end) as bouncetotal,  
   sum(case when  actiontypecode='open' then dcounts else 0 end) as opentotal,  
   sum(case when  actiontypecode='click' then dcounts else 0 end) as clicktotal,  
   sum(case when  actiontypecode='subscribe' then dcounts else 0 end) as unsubscribetotal, g.groupname  ,
   sum(case when  actiontypecode='suppressed' then dcounts else 0 end) as suppressedtotal 
 from  
 (  
  SELECT  eal.blastID,   
    COUNT(eal.EAID) AS dcounts,  
    case  when actiontypecode = 'bounce' and actionvalue = 'softbounce' then 'softbounce'  
       when actiontypecode = 'bounce' and (actionvalue = 'hardbounce' or actionvalue = 'hard') then 'hardbounce'  
      else actiontypecode   
    end as actiontypecode  
  FROM    
    EmailActivityLog eal with (NOLOCK)   
  where   
    eal.blastID in (select blastID from [BLAST] b with (NOLOCK) where b.customerID = @customerID and sendtime between @startdate and @enddate and statuscode = 'sent' and b.testblast='N') and   
    (actiontypecode = 'open' or actiontypecode = 'click' or actiontypecode = 'send'  or actiontypecode = 'bounce' or actiontypecode = 'subscribe' or ActionTypeCode='suppressed')  
  GROUP BY    
    eal.blastID,   
    case  when actiontypecode = 'bounce' and actionvalue = 'softbounce' then 'softbounce'  
       when actiontypecode = 'bounce' and (actionvalue = 'hardbounce' or actionvalue = 'hard') then 'hardbounce'  
      else actiontypecode   
    end     
 ) inn join [BLAST] b1 on inn.blastID = b1.blastID left outer join groups g on b1.groupID = g.groupID  
 group by b1.BlastID, b1.sendtime, b1.emailsubject, g.groupname  
 order by b1.sendtime asc  
 End
 
 
  /*  
  select   
    b.blastID,   
    b.sendtime,   
    b.emailsubject,  
    count(distinct case when actiontypecode = 'send' then eal.emailID end) as sendcount,   
    count(distinct case when actiontypecode = 'open' then eal.emailID end) as opencount,   
    count(distinct case when actiontypecode = 'click' then eal.emailID end) as Clickcount,   
    count(distinct case when actiontypecode = 'bounce' and actionvalue = 'softbounce' then eal.emailID end) as softbouncecount,  
    count(distinct case when actiontypecode = 'bounce' and (actionvalue = 'hardbounce' or actionvalue = 'hard') then eal.emailID end) as hardbouncecount,  
    count(distinct case when actiontypecode = 'bounce' then eal.emailID end) as bouncecount,  
    count(distinct case when actiontypecode = 'subscribe' then eal.emailID end) as unsubscribecount  
      
  from   
    emailactivitylog eal with (NOLOCK) join   
    [BLAST] b with (NOLOCK) on eal.blastID = b.blastID  
  where   
    b.customerID =  and  
    sendtime between @startdate and @enddate and      
    statuscode = 'sent' and   
    b.testblast='N' --and   
    --(actiontypecode = 'open' or actiontypecode = 'click' or actiontypecode = 'send'  or actiontypecode = 'bounce' or actiontypecode = 'subscribe')  
  group by b.blastID, b.sendtime, b.emailsubject  
 ) inn  
 order by sendtime asc  
 */  
  
End
