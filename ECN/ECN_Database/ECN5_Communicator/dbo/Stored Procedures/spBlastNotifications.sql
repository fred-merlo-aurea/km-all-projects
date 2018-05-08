CREATE PROCEDURE [dbo].[spBlastNotifications]
AS
BEGIN
declare @customer varchar(200),
		@blastID int,
		@EmailSubject varchar(800),
		@sendtime datetime,
		@inQueue int,
		@s varchar(1000),
		@b varchar(8000),
		@status varchar(25),
		@sendTotal int,
		@username varchar(50),
		@configid int
		
set @s = 'Blast Issue Notification : '
select @b = ''

DECLARE c_Email CURSOR FOR 
select Customername + ' (' + convert(varchar(10), c.CustomerID) + ')', BlastID, EmailSubject, sendtime, datediff(mi, sendtime, getdate()), statuscode, isnull(sendtotal, 0) as sendtotal, u.username, c.BlastConfigID as configid
from [blast] b join ecn5_accounts..[customer] c on b.customerID = c.customerID left outer join kmplatform..[user] u on u.userID = b.CreatedUserID
where	B.CustomerID NOT IN (2790) and blasttype in ('champion','html','sample','text') and ((
			statuscode='active' and 
			(
				(TestBlast='y' AND datediff(mi, sendtime, getdate()) > 10)
				OR
				(TestBlast='N' AND isnull(SendTotal,0) =0 AND datediff(mi, sendtime, getdate()) > 10)
				OR
				(SendTotal BETWEEN 1 AND 100 AND TestBlast='n' AND datediff(mi, sendtime, getdate()) > 10)
				OR
				(SendTotal BETWEEN 101 AND 10000 AND TestBlast='n' AND datediff(mi, sendtime, getdate()) > 20)
				OR
				(SendTotal BETWEEN 10001 AND 20000 AND TestBlast='n' AND datediff(mi, sendtime, getdate()) > 40)	
				OR
				(SendTotal BETWEEN 20001 AND 30000 AND TestBlast='n' AND datediff(mi, sendtime, getdate()) > 60)				
				OR
				(SendTotal BETWEEN 30001 AND 50000 AND TestBlast='n' AND datediff(mi, sendtime, getdate()) > 60)
				OR
				(SendTotal BETWEEN 50001 AND 100000 AND TestBlast='n' AND datediff(mi, sendtime, getdate()) > 60)
				OR
				(SendTotal > 100000 AND datediff(mi, sendtime, getdate()) > 60) 
			)
	
		) or 
		
		--(StatusCode='pending' and datediff(mi, sendtime, getdate()) > 15 ) and (not exists(select TOP 1 blastID from Blasts where StatusCode='active' and CustomerID = c.customerID) OR c.customerID = 1797 ) )
		(StatusCode='pending' and datediff(mi, sendtime, getdate()) > 15 ) or
		(statuscode='error'))
order by sendtime asc

--select Customername + ' (' + convert(varchar(10), c.CustomerID) + ')', BlastID, EmailSubject, sendtime, datediff(mi, sendtime, getdate()), statuscode, isnull(sendtotal, 0) from blasts b join ecn5_accounts..customers c on b.customerID = c.customerID where (statuscode='active' or StatusCode='pending') and datediff(mi, sendtime, getdate()) > 15 order by sendtime asc

OPEN c_Email  
FETCH NEXT FROM c_Email INTO @customer, @blastID, @EmailSubject, @sendtime, @inQueue, @status, @sendTotal,@username,@configid

WHILE @@FETCH_STATUS = 0  
BEGIN 
	--select @activeblast = COUNT(blastid) from Blasts where StatusCode='active' and CustomerID = @customer
	--If (@status = 'active') or (@status = 'pending' and @customer IN (1797)) or (@status = 'pending' and @customer NOT IN (1797) and @activeblast = 0)
	--Begin
		if(@configid is null)
		Begin
			set @b = @b + '<b>Alert : </b>No MTA setup for customer<BR>'
		End
		set @b = @b+ '<b>Customer : </b>' + @customer + '<BR><b>BlastID : </b>' + convert(varchar(10),@blastID) + ' <BR><b>Email Subject : </b>'  + @EmailSubject  + ' <BR><b>Send Time : </b>'  + convert(varchar(25), @sendtime,100) + ' <BR><b>Send Total : </b>' 

 + convert(varchar(25), @sendTotal,100) +  ' <BR><b>Created by : </b>'  + @userName + ' <BR><b>Status : </b>' + @status + ' for ' + convert(varchar(10),@inQueue) + ' min<BR><BR><hr><BR>'		
	--End
	FETCH NEXT FROM c_Email INTO @customer, @blastID, @EmailSubject, @sendtime, @inQueue, @status, @sendTotal,@username,@configid
END

if @b <> ''
Begin
	EXEC msdb..sp_send_dbmail 
		@profile_name='SQLAdmin', 
		@recipients='DigitalServices@TeamKM.com;dev@knowledgemarketing.com', 
		--@recipients='bill@knowledgemarketing.com', 
		@importance='High',
		@body_format = 'HTML',
		@subject=@s, 
		@body=@b
End	

CLOSE c_Email  
DEALLOCATE c_Email 

END


