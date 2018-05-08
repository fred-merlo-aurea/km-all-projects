CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getCustomEmailActivityData]
(
	@CustomerID int,
	@actioncode varchar(400),
	@actionvalue varchar(400),
	@Frequency  varchar(10),
	@daysBlock	int
)
AS
BEGIN
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getCustomEmailActivityData', GETDATE())
	DECLARE @sqlstring varchar(4000)
	PRINT (@actioncode)
	IF UPPER(@Frequency) = 'DAILY'
	BEGIN
		 SET @sqlstring=	      ' SELECT eal.blastID, e.EmailAddress, eal.EmailID, '+  
									  ' (CASE eal.ActionTypeCode  '+  
									  '  	WHEN ''subscribe''   		THEN ''UnSubscribe''  '+  
									  '  	WHEN ''MASTSUP_UNSUB''   	THEN ''UnSubscribe''  '+  
									  '  	WHEN ''ABUSERPT_UNSUB''		THEN ''UnSubscribe''  '+  
									  '  	WHEN ''FEEDBACK_UNSUB''  	THEN ''UnSubscribe''  '+  
									  ' 	ELSE eal.ActionTypeCode END) AS ActionType, '+  
									  ' (CASE eal.ActionValue  '+ 
									  ' 		WHEN ''hardbounce''   		THEN ''hard''  '+	
									  ' 		WHEN ''softbounce''   		THEN ''soft''  '+
									  ' 		WHEN ''spamnotification''   	THEN ''blocked''  '+
									  ' 		WHEN ''autoresponder''   	THEN ''notify''  '+
									  ' 		WHEN ''U''   			THEN ''U''  '+
									  ' 	ELSE ''unknown'' END) AS ActionValue, '+ 
									  ' eal.ActionDate, eal.ActionNotes '+
									  ' FROM [BLAST] b join '+
									  ' Emailactivitylog eal on eal.blastID = b.blastID join '+	 
									  ' Emails e on e.emailID = eal.emailID '+
									  ' WHERE b.CustomerID = ' + CONVERT(VARCHAR(10),@CustomerID) +
									  ' AND eal.ActionTypeCode IN ( ' + @actionCode + ')' 
		IF(LEN(@actionvalue) > 0 and @actioncode = '''bounce''') 
		BEGIN
			SET @sqlstring = @sqlstring + ' AND eal.ActionValue IN ( ' + @actionvalue + ')' 
		END
		 
		SET @sqlstring =	@sqlstring +	' AND CONVERT(VARCHAR,eal.ActionDate,101) = CONVERT(VARCHAR, GETDATE()-1,101) ' + 
													' ORDER BY eal.ActionTypeCode, eal.ActionDate '
		PRINT (@sqlstring)
		EXEC (@sqlstring)
	END
	ELSE
	BEGIN
		 SET @sqlstring=	      ' SELECT eal.blastID, e.EmailAddress, eal.EmailID, '+  
									  ' (CASE eal.ActionTypeCode  '+  
									  '  	WHEN ''subscribe''   		THEN ''UnSubscribe''  '+  
									  '  	WHEN ''MASTSUP_UNSUB''   	THEN ''UnSubscribe''  '+  
									  '  	WHEN ''ABUSERPT_UNSUB''		THEN ''UnSubscribe''  '+  
									  '  	WHEN ''FEEDBACK_UNSUB''  	THEN ''UnSubscribe''  '+  
									  ' 	ELSE eal.ActionTypeCode END) AS ActionType, '+  
									  ' (CASE eal.ActionValue  '+ 
									  ' 		WHEN ''hardbounce''   		THEN ''hard''  '+	
									  ' 		WHEN ''softbounce''   		THEN ''soft''  '+
									  ' 		WHEN ''spamnotification''   	THEN ''blocked''  '+
									  ' 		WHEN ''autoresponder''   	THEN ''notify''  '+
									  ' 		WHEN ''U''   			THEN ''U''  '+
									  ' 	ELSE ''unknown'' END) AS ActionValue, '+ 
									  ' eal.ActionDate, eal.ActionNotes '+
									  ' FROM [BLAST] b join '+
									  '		Emailactivitylog eal on eal.blastID = b.blastID join '+	 
									  '		Emails e on e.emailID = eal.emailID '+
									  ' WHERE b.CustomerID = ' + convert(varchar(10),@CustomerID) +
									  '		AND eal.ActionTypeCode IN ( ' + @actionCode + ')' 
		IF(LEN(@actionvalue) > 0 and @actioncode = '''bounce''') 
		BEGIN
			SET @sqlstring = @sqlstring + ' AND eal.ActionValue IN ( ' + @actionvalue + ')' 
		END
		 
		SET @sqlstring =	@sqlstring + ' AND eal.ActionDate BETWEEN DATEADD(dd,' +  CONVERT(VARCHAR(10),-1 * (@daysBlock)) + ',GETDATE()) AND DATEADD(dd,-1,getDate()) ' +    
									  ' ORDER BY eal.ActionTypeCode, eal.ActionDate '
		PRINT (@sqlstring)
		EXEC (@sqlstring)
	END
END
