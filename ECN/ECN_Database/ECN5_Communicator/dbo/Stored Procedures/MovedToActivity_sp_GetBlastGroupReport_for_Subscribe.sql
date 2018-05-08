CREATE PROCEDURE [dbo].[MovedToActivity_sp_GetBlastGroupReport_for_Subscribe]   
(  
	@BlastID int,   
	@FilterType varchar(25),   
	@ISP varchar(100),  
	@PageNo int,  
	@PageSize int 
)       
as 
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_GetBlastGroupReport_for_Subscribe', GETDATE())
	declare @RecordNoStart int,  
			@RecordNoEnd int,
			@groupID varchar(100)

	Set @groupID = ''
	select @groupID = groupID from [BLAST] where blastID = @BlastID

	Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	Set @RecordNoEnd = (@PageNo * @PageSize) + 50  
	
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@PageNo * @PageSize + 1);
	SELECT @LastRec = (@FirstRec + @PageSize - 1); 

	--SET ROWCOUNT @RecordNoEnd  

	if len(ltrim(rtrim(@ISP))) = 0  
	begin  

		Select count(eal.EAID) from EmailActivityLog eal with (NOLOCK) 
		WHERE  ActionTypeCode=@FilterType and blastID = @BlastID


		SET NOCOUNT ON;
		WITH TempResult as
		(
			SELECT 
				ROW_NUMBER() OVER(ORDER BY eal.EAID desc) as 'RowNum', 
				eal.EMailID, e.EmailAddress as EmailAddress, eal.ActionDate as UnsubscribeTime, eal.ActionNotes as Reason, eal.ActionValue as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+@groupID AS URL
			FROM 
				EmailActivityLog eal WITH (NOLOCK)
				JOIN Emails e WITH (NOLOCK) ON e.emailID = eal.emailID
			WHERE  
				eal.ActionTypeCode=@FilterType  and blastID = @BlastID 
		)				
		SELECT EmailID, EmailAddress, UnsubscribeTime, Reason, SubscriptionChange, URL
		FROM TempResult
		WHERE RowNum >= @FirstRec 
		AND RowNum <= @LastRec
		SET NOCOUNT OFF;  
	end  
	else  
	Begin  

		Select count(eal.EAID) from EmailActivityLog eal with (NOLOCK) join emails e with (NOLOCK) on eal.emailID = e.emailID 
		WHERE  blastID = @BlastID and ActionTypeCode=@FilterType  AND e.emailaddress like '%' + @ISP        

		SET NOCOUNT ON;
		WITH TempResult as
		(
			SELECT 
				ROW_NUMBER() OVER(ORDER BY eal.EAID desc) as 'RowNum', 
				eal.EMailID, e.EmailAddress as EmailAddress, eal.ActionDate as UnsubscribeTime, eal.ActionValue as SubscriptionChange, eal.ActionNotes as Reason, 'EmailID='+CONVERT(VARCHAR,eal.EmailID)+'&GroupID='+ @groupID AS URL
			FROM 
				EmailActivityLog eal WITH (NOLOCK)
				JOIN Emails e WITH (NOLOCK) ON e.emailID = eal.emailID
			WHERE  
				blastID = @BlastID and 	eal.ActionTypeCode=@FilterType and e.emailaddress like '%' + @ISP  
		)				
		SELECT emailID, EmailAddress, UnsubscribeTime, SubscriptionChange, Reason, URL
		FROM TempResult
		WHERE RowNum >= @FirstRec 
		AND RowNum <= @LastRec
		SET NOCOUNT OFF; 
	End  

	--SET ROWCOUNT 0
End
