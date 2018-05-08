CREATE  PROCEDURE [dbo].[sp_CurrentBlasts]
AS
BEGIN
select BlastID as BLID, b.CustomerID as CID, substring(c.CustomerName,0,15) as CustName, substring(EmailSubject,0,20) as Subject, 
substring(StatusCode,0,9) as Stat, substring(BlastType,0,5) as 'TYPE', 
convert(varchar,SendTime,101)+' '+substring(convert(varchar,SendTime,108),0,9) as SentTime , CodeID as CDE, 
substring(BlastFrequency,0,5) as FREQ, AttemptTotal as Total, SuccessTotal as Succes, sendTotal,
convert(varchar,FinishTime,101)+' '+substring(convert(varchar,FinishTime,108),0,9) as FinishTime, 
LayoutID as LYTID, GroupID as GRP, FilterID as FLTR, TestBlast as TST, RefBlastID as 'REFID'
from [BLAST] b join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on b.customerID = c.customerID where sendtime > convert(datetime,getDate()-2)
order by sendtime desc, blastID desc
/*select BlastID as BLID, CustomerID as CID, EmailSubject, substring(StatusCode,0,5) as Stat, 
convert(varchar,SendTime,101)+' '+substring(convert(varchar,SendTime,108),0,6) as SentTime , CodeID as CDE, 
substring(BlastFrequency,0,6) as FRECY, AttemptTotal as Total, SuccessTotal as Succes, sendTotal,
convert(varchar,FinishTime,101)+' '+substring(convert(varchar,FinishTime,108),0,6) as FinishTime, 
LayoutID as LayID, GroupID as Grp, FilterID as FLTR, TestBlast as Test from [BLAST] where sendtime > convert(datetime,getDate()-2)
and CustomerID = 162 order by sendtime desc, blastID desc*/
/*select BlastID as BLID, b.CustomerID as CID, substring(c.CustomerName,0,20) as CustName, substring(EmailSubject,0,25) as Subject, 
StatusCode as Stat, 
convert(varchar,SendTime,101)+' '+substring(convert(varchar,SendTime,108),0,9) as SentTime , CodeID as CDE, 
substring(BlastFrequency,0,6) as FRECY, AttemptTotal as Total, SuccessTotal as Succes, 
convert(varchar,FinishTime,101)+' '+substring(convert(varchar,FinishTime,108),0,9) as FinishTime, 
LayoutID as LayID, GroupID as Grp, FilterID as FLTR, sendTotal, TestBlast as Test, RefBlastID
from [BLAST] b join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on b.customerID = c.customerID where sendtime > convert(datetime,getDate()-30)
and b.CustomerID in (select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where basechannelID = 29)
order by sendtime desc, blastID desc*/
END
