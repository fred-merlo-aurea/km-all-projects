CREATE  proc [dbo].[MovedToActivity_rpt_CanonTradeshowReport]  
(  
  @customerID int,
  @startdate varchar(20),
  @enddate varchar(20)
)  
AS  
BEGIN  
  
 INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('rpt_CanonTradeshowReport', GETDATE())
 set @startdate = @startdate + ' 00:00:00 '    
 set @enddate = @enddate + '  23:59:59'      


select  b.blastID, b.sendtime, b.emailsubject,  
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   count(case when  actiontypecode='send' then eal.EAID end) as 'Total',        
   count(case when  actiontypecode='suppressed' then eal.EAID end) as 'Suppressed',   
   count(case when  actiontypecode='send' then eal.EAID end) - (count(case when  actiontypecode='softbounce' then eal.EAID end) + count(case when  actiontypecode='bounce' then eal.EAID end) + count(case when  actiontypecode='hardbounce' then eal.EAID end)) as  'Delivered',   
   count(case when  actiontypecode='open' then eal.EAID end) as 'Opened',  
   count(case when  actiontypecode='click' then eal.EAID end) as 'Clicked',  
   count(case when  actiontypecode='subscribe' then eal.EAID end) as 'Unsub', 
   count(case when  actiontypecode='softbounce' then eal.EAID end) + count(case when  actiontypecode='bounce' then eal.EAID end) + count(case when  actiontypecode='hardbounce' then eal.EAID end) as 'Bounced'  
 from  
	 [BLAST] b with (nolock) 
	 join EmailActivityLog eal with(nolock) on b.BlastID = eal.BlastID 
	 join BlastFields bf with(nolock) on bf.BlastID = b.BlastID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc       
END
