CREATE PROCEDURE [dbo].[spGetBlastGroupReport_for_Subscribe]   
(  
	@BlastID int,   
	@FilterType varchar(25),   
	@ISP varchar(100),  
	@PageNo int,  
	@PageSize int,
	@OnlyUnique bit = 0
--	@RecordsType varchar(10)='T' 
)       

--declare
--	@BlastID int=1321995,   
--	@FilterType varchar(25)='subscribe',   
--	@ISP varchar(100)='',  
--	@PageNo int=0,  
--	@PageSize int=1000,
--	@RecordsType VARCHAR(10)='U'
as 
Begin

	declare @RecordNoStart int,  
			@RecordNoEnd int,
			@groupID varchar(100)

	Set @groupID = ''
	select @groupID = groupID from ecn5_communicator..[BLAST] where blastID = @BlastID

	Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	Set @RecordNoEnd = (@PageNo * @PageSize) + 50  
	
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@PageNo * @PageSize + 1);
	SELECT @LastRec = (@FirstRec + @PageSize - 1); 

	--SET ROWCOUNT @RecordNoEnd  

	if len(ltrim(rtrim(@ISP))) = 0  
	BEGIN  
	if @OnlyUnique = 1
			BEGIN 
				Select count(distinct bau.EmailID) from BlastActivityUnSubscribes bau with (NOLOCK) 
				join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
				WHERE  uc.UnsubscribeCode=@FilterType and blastID = @BlastID 


				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY bau.EmailID desc) as 'RowNum', 
						bau.EMailID, e.EmailAddress as EmailAddress, MIN(UnsubscribeTime) as UnsubscribeTime, MIN(bau.Comments) as Reason, 
						uc.UnsubscribeCode as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+@groupID AS URL
					FROM 
						BlastActivityUnSubscribes bau WITH (NOLOCK)
						JOIN ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID
						join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  
						uc.UnsubscribeCode=@FilterType  and blastID = @BlastID --and bau.Comments like '%FOR GROUP: ' + @groupID + '%'
					GROUP BY  bau.EmailID, e.EmailAddress, uc.UnsubscribeCode
				)				
				SELECT EmailID, EmailAddress, UnsubscribeTime as 'UnsubscribeTime', Reason as 'Reason', SubscriptionChange, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec AND RowNum <= @LastRec
				ORDER BY UnsubscribeTime desc
				SET NOCOUNT OFF;  
			END 
		ELSE IF @OnlyUnique = 0
			BEGIN 
				Select count(bau.UnsubscribeID) from BlastActivityUnSubscribes bau with (NOLOCK) join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
				WHERE  uc.UnsubscribeCode=@FilterType and blastID = @BlastID


				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY bau.UnsubscribeID desc) as 'RowNum', 
						bau.EMailID, e.EmailAddress as EmailAddress, UnsubscribeTime as UnsubscribeTime, bau.Comments as Reason, 
						uc.UnsubscribeCode as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+@groupID AS URL
					FROM 
						BlastActivityUnSubscribes bau WITH (NOLOCK)
						JOIN ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID
						join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  
						uc.UnsubscribeCode=@FilterType  and blastID = @BlastID 
				)				
				SELECT EmailID, EmailAddress, UnsubscribeTime, Reason, SubscriptionChange, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;  
			END 
		end  
	else  
	Begin  
		if @OnlyUnique = 1
			BEGIN
				Select count(distinct bau.EmailID) 
				from 
					BlastActivityUnSubscribes bau with (NOLOCK) 
					join ecn5_communicator..emails e with (NOLOCK) on bau.emailID = e.emailID 
					join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
				WHERE  blastID = @BlastID and uc.UnsubscribeCode=@FilterType  AND e.emailaddress like '%' + @ISP  
				

				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY bau.EmailID desc) as 'RowNum', 
						bau.EMailID, e.EmailAddress as EmailAddress, bau.UnsubscribeTime as UnsubscribeTime, uc.UnsubscribeCode as SubscriptionChange, Comments as Reason, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+ @groupID AS URL
					FROM 
						BlastActivityUnSubscribes bau WITH (NOLOCK)
						JOIN ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID
						join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  
						blastID = @BlastID and 	uc.UnsubscribeCode=@FilterType and e.emailaddress like '%' + @ISP  
						
				)				
				SELECT emailID, EmailAddress, UnsubscribeTime as 'UnsubscribeTime', SubscriptionChange, Reason as 'Reason', URL
				FROM TempResult
				WHERE RowNum >= @FirstRec AND RowNum <= @LastRec
				ORDER BY UnsubscribeTime desc
				SET NOCOUNT OFF; 
			END
	  
		ELSE if @OnlyUnique = 0
		begin
			Select count(UnsubscribeID) 
			from 
				BlastActivityUnSubscribes bau with (NOLOCK) 
				join ecn5_communicator..emails e with (NOLOCK) on bau.emailID = e.emailID 
				join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
			WHERE  blastID = @BlastID and uc.UnsubscribeCode=@FilterType  AND e.emailaddress like '%' + @ISP        

			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY bau.UnsubscribeID desc) as 'RowNum', 
					bau.EMailID, e.EmailAddress as EmailAddress, bau.UnsubscribeTime as UnsubscribeTime, uc.UnsubscribeCode as SubscriptionChange, Comments as Reason, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+ @groupID AS URL
				FROM 
					BlastActivityUnSubscribes bau WITH (NOLOCK)
					JOIN ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID
					join UnsubscribeCodes uc  WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
				WHERE  
					blastID = @BlastID and 	uc.UnsubscribeCode=@FilterType and e.emailaddress like '%' + @ISP  
			)				
			SELECT emailID, EmailAddress, UnsubscribeTime, SubscriptionChange, Reason, URL
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF; 
		End  
	END 

	--SET ROWCOUNT 0
End
