
CREATE PROCEDURE [dbo].[rpt_SolicitationEmails_New] 

 @From datetime,   
 @To datetime  
AS

BEGIN  
 SET @From = @From + ' 00:00:00'  
 SET @To = @To + ' 23:59:00'  
   
 DECLARE @tblSolicitationEmails table (Charity varchar(250), MonthYear varchar(250), State varchar(10), Emails int, EmailType varchar(50), ECNGroupID int)    
   
 INSERT INTO @tblSolicitationEmails  
 SELECT  
     c.CustomerName as 'Charity',    
  CONVERT(varchar, MONTH(b.SendTime)) + '/' + CONVERT(varchar, YEAR(b.SendTime)) as 'MonthYear', e.State as State,
  count (bas.SendID) as 'Emails', 'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'  
 FROM  
  blast b    with (NOLOCK) 
  join ECN5_ACCOUNTS..Customer c  with (NOLOCK) on c.CustomerID = b.CustomerID  
  join Filter  f  with (NOLOCK) on f.FilterID = b.FilterID and f.CustomerID = b.CustomerID  join
  ECN_ACTIVITY..BlastActivitySends bas  with (NOLOCK) on bas.BlastID = b.BlastID join ECN5_COMMUNICATOR..Emails e  with (NOLOCK) on e.EmailID = bas.emailID
  
 WHERE   
   c.customerID = 91-- in (91,112,1309,2461,2661,1939,3165,1897,3199,3241)  
  and b.SendTime between @From and @To and b.TestBlast = 'N'  
  and f.FilterName like '%Mailing Schedule%'  
 GROUP BY  
  c.CustomerID,c.CustomerName,e.State , YEAR(b.SendTime), MONTH(b.SendTime), b.GroupID    
 ORDER BY  
  c.CustomerName  
  
     
 --solicitations for lfa and lupus az     
 INSERT INTO @tblSolicitationEmails  
 SELECT 'Lupus Foundation of America' as Charity,    
   CONVERT(varchar, MONTH(b.SendTime)) + '/' + CONVERT(varchar, YEAR(b.SendTime)) as 'MonthYear',    e.State  as State,
   Count(case when f.FilterName like '%Mailing Schedule%' then bas.SendID end) as 'Emails',  
   'SOLICITATIONS' as 'EmailType', b.GroupID as 'ECNGroupID'         
 FROM   
   blast b with (NOLOCK) left join Filter  f  with (NOLOCK) on b.CustomerID = f.CustomerID and b.FilterID = f.FilterID   join
  ECN_ACTIVITY..BlastActivitySends bas  with (NOLOCK) on bas.BlastID = b.BlastID join ECN5_COMMUNICATOR..Emails e  with (NOLOCK) on e.EmailID = bas.emailID
 WHERE   
   b.CustomerID = 159 and b.SendTime between @From and @To and b.TestBlast = 'N'  
 GROUP BY  
   YEAR(b.SendTime), MONTH(b.SendTime), e.State ,b.GroupID   
        
 SELECT case when CHARINDEX('-',Charity) = 0 then Charity else  SUBSTRING(Charity,0,CHARINDEX('-',Charity)) end as 'Charity',  state, MonthYear, Emails,EmailType,ECNGroupID   
 FROM @tblSolicitationEmails order by Charity asc  
END

