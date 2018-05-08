CREATE  proc [dbo].[rptCanonTradeshowReport]  
(  
  @customerID int,
  @startdate varchar(20),
  @enddate varchar(20)
)  
AS  
BEGIN 

--declare 
--  @customerID int,
--  @startdate varchar(20),
--  @enddate varchar(20)
  
  --set @customerID = 1794
  --set @startdate = '2012-01-04'
  --set @enddate = '2012-01-04' 
  
 set @startdate = @startdate + ' 00:00:00 '    
 set @enddate = @enddate + '  23:59:59' 
 
 declare @results table (blastID int, sendtime datetime, emailsubject varchar(250), attempttotal int, Field5 varchar(255), Field3 varchar(255), Field4 varchar(255), Total int, Suppressed int, Delivered int, Opened int, Clicked int, Unsub int, Bounced int) 
 --sends
 insert into @results
 select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal, 
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   count(SendID), 0, 0, 0, 0, 0, 0
 from  
	 ecn5_communicator..[BLAST] b with (nolock) 
	 join BlastActivitySends bas with(nolock) on b.BlastID = bas.BlastID 
	 join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc   
 --suppressed
 insert into @results
 select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal,
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   0, count(SuppressID), 0, 0, 0, 0, 0
 from  
	 ecn5_communicator..[BLAST] b with (nolock) 
	 join BlastActivitySuppressed basupp with(nolock) on b.BlastID = basupp.BlastID 
	 join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc   
 --delivered
 --insert into @results
 --select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal,  
 --  bf.Field5 as 'Show', 
 --  bf.Field3 as 'Marketing/Sales/customer service', 
 --  bf.Field4 as 'Marketing Message', 
 --  0, 0, COUNT(SendID), 0, 0, 0, 0
 --from  
	-- ecn5_communicator..[BLAST] b with (nolock) 
	-- join BlastActivitySends bas with(nolock) on b.BlastID = bas.BlastID 
	-- join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
	-- left outer join BlastActivityBounces bab with (nolock) on bas.BlastID = bab.BlastID and bas.EmailID = bab.EmailID
 --where 
	-- b.SendTime between @startdate and @enddate 
	-- and b.CustomerID = @customerID
	-- and bab.BounceID is null
 --group by 
 --    b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 --order by 
 --    b.sendtime asc 
 --opened
 insert into @results
 select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal,  
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   0, 0, 0, COUNT(OpenID), 0, 0, 0
 from  
	 ecn5_communicator..[BLAST] b with (nolock) 
	 join BlastActivityOpens bao with(nolock) on b.BlastID = bao.BlastID 
	 join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc 
 --clicked
 insert into @results
 select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal,  
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   0, 0, 0, 0, COUNT(ClickID), 0, 0
 from  
	 ecn5_communicator..[BLAST] b with (nolock) 
	 join BlastActivityClicks bac with(nolock) on b.BlastID = bac.BlastID 
	 join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc 
 --Unsub
 insert into @results
 select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal,  
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   0, 0, 0, 0, 0, COUNT(UnsubscribeID), 0
 from  
	 ecn5_communicator..[BLAST] b with (nolock) 
	 join BlastActivityUnSubscribes bau with(nolock) on b.BlastID = bau.BlastID 
	 join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
	 join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
	 and uc.UnsubscribeCode = 'subscribe'
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc 
 --Bounced
 insert into @results
 select  b.blastID, b.sendtime, b.emailsubject, b.AttemptTotal,  
   bf.Field5 as 'Show', 
   bf.Field3 as 'Marketing/Sales/customer service', 
   bf.Field4 as 'Marketing Message', 
   0, 0, 0, 0, 0, 0, COUNT(BounceID)
 from  
	 ecn5_communicator..[BLAST] b with (nolock) 
	 join BlastActivityBounces bab with(nolock) on b.BlastID = bab.BlastID 
	 join ecn5_communicator..BlastFields bf with(nolock) on bf.BlastID = b.BlastID
 where 
	 b.SendTime between @startdate and @enddate 
	 and b.CustomerID = @customerID
 group by 
     b.BlastID, b.sendtime, b.emailsubject, b.AttemptTotal, bf.Field5, bf.Field3, bf.Field4
 order by 
     b.sendtime asc     
     
 select blastid, sendtime, emailsubject, field5 as 'Show', field3 as 'Marketing/Sales/customer service', field4 as 'Marketing Message', SUM(Total) as Total, SUM(Suppressed) as Suppressed, (SUM(Total) - SUM(Bounced)) as Delivered, SUM(Opened) as Opened, SUM(Clicked) as Clicked, SUM(Unsub) as Unsub, SUM(Bounced) as Bounced
 from @results
 group by blastid, sendtime, emailsubject, AttemptTotal, field5, field3, field4   
 order by sendtime 
END
