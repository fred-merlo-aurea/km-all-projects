CREATE PROCEDURE [dbo].[spBlastGoneWrong_HighBounces]
AS
BEGIN
DECLARE @BlastID INT,
		@CustomerID INT,
		@CustomerName VARCHAR(150),
		@EmailFrom VARCHAR(100),
		@b VARCHAR(8000) = '',
		@SendCount INT,
		@BounceCount INT,
		@BouncePercentage FLOAT

DECLARE c_Blast CURSOR FOR 
SELECT b.BlastID, b.CustomerID, b.EmailFrom, c.CustomerName 
FROM 
	[Blast] b WITH (NOLOCK)
	JOIN ECN5_ACCOUNTS..[Customer] c WITH (NOLOCK) on b.CustomerID = c.CustomerID
WHERE
	CONVERT(VARCHAR(10),b.SendTime,111) = CONVERT(VARCHAR(10),DateAdd(D,-1, GetDate()),111)
ORDER BY BlastID ASC

OPEN c_Blast  
FETCH NEXT FROM c_Blast INTO @BlastID, @CustomerID, @EmailFrom, @CustomerName

WHILE @@FETCH_STATUS = 0  
BEGIN 
	SELECT @SendCount = COUNT(EAID) FROM [EmailActivityLog] WHERE BlastID = @BlastID AND ActionTypeCode = 'SEND'
	SELECT @BounceCount = COUNT(EAID) FROM [EmailActivityLog] WHERE BlastID = @BlastID AND ActionTypeCode = 'BOUNCE'
	IF @BounceCount > 0 AND @SendCount > 0
	BEGIN
		IF (@BounceCount * 100) / @SendCount > 10
		BEGIN
			--too many bounces
			set @b = @b + '<b>Customer : </b>' + @CustomerName + '(' + CONVERT(VARCHAR(10),@CustomerID) + ')<BR>' + 
							'<b>BlastID : </b>' + CONVERT(VARCHAR(10),@BlastID) + ' <BR>' + 
							'<b>Email From : </b>'  + @EmailFrom  + ' <BR>' +
							'<b>Send Total : </b>'  + convert(varchar(25), @SendCount,100) + ' <BR>' +
							'<b>Bounce Total : </b>'  + convert(varchar(25), @BounceCount,100) + ' <BR>' +
							'<b>Bounce Rate : </b>' + convert(varchar(25),((@BounceCount * 100) / @SendCount )) + '%<BR><BR>'
		END
		--IF @SendCount > 0
		--BEGIN
		--	IF (@BounceCount * 100) / @SendCount > 10
		--	BEGIN
		--		--too many bounces
		--		set @b = @b + '<b>Customer : </b>' + @CustomerName + '(' + CONVERT(VARCHAR(10),@CustomerID) + ')<BR>' + 
		--						'<b>BlastID : </b>' + CONVERT(VARCHAR(10),@BlastID) + ' <BR>' + 
		--						'<b>Email From : </b>'  + @EmailFrom  + ' <BR>' +
		--						'<b>Send Total : </b>'  + convert(varchar(25), @SendCount,100) + ' <BR>' +
		--						'<b>Bounce Total : </b>'  + convert(varchar(25), @BounceCount,100) + ' <BR>' +
		--						'<b>Bounce Rate : </b>' + convert(varchar(25),((@BounceCount * 100) / @SendCount )) + '%<BR><BR>'
		--	END
		--END
		--ELSE
		--BEGIN
		--	--too many bounces
		--	set @b= @b + '<b>Customer : </b>' + @CustomerName + '(' + CONVERT(VARCHAR(10),@CustomerID) + ')<BR>' + 
		--						'<b>BlastID : </b>' + CONVERT(VARCHAR(10),@BlastID) + ' <BR>' + 
		--						'<b>Email From : </b>'  + @EmailFrom  + ' <BR>' +
		--						'<b>Send Total : </b>'  + convert(varchar(25), @SendCount,100) + ' <BR>' +
		--						'<b>Bounce Total : </b>'  + convert(varchar(25), @BounceCount,100) + ' <BR>' +
		--						'<b>Bounce Rate : </b>100%<BR><BR>'		
		--END
	END
	
	FETCH NEXT FROM c_Blast INTO @BlastID, @CustomerID, @EmailFrom, @CustomerName
END


CLOSE c_Blast  
DEALLOCATE c_Blast

if @b <> ''
Begin
	EXEC msdb..sp_send_dbmail 
		@profile_name='SQLAdmin', 
		@recipients='digitalservices@teamkm.com;dev@knowledgemarketing.com;', 
		--@recipients='bill@knowledgemarketing.com', 
		@importance='High',
		@body_format = 'HTML',
		@subject='Blast Gone Wrong Notification :', 
		@body=@b
	--select @b
End	

END


