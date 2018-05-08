create proc [dbo].[temp_canonunsubscribes]
as
select e.EmailAddress, c.customername, g.groupID, g.groupname, b.BlastID, eal.actiondate from [BLAST] b join EmailActivityLog eal on b.BlastID = eal.blastID join Emails e on e.EmailID = eal.emailID join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on c.CustomerID = e.customerID
join Groups g on g.GroupID = b.groupID
where b.groupID in (38523,38526 ,38525 ,38524 ,36447 ,21809 ,14092) and SendTime between DATEADD(m,-6, getdate()) and GETDATE() and testblast='N'
and StatusCode='sent' and ActionTypeCode not in ('send','open','click','bounce','refer','testsend','read','MASTSUP_UNSUB','FEEDBACK_UNSUB')
order by e.EmailAddress, c.customername, eal.blastID
