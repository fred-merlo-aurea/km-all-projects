CREATE PROCEDURE [dbo].[spGetCampaignItemReportWithSuppressed]   
(  
	@CampaignItemID int,   
	@CustomerID int,   
	@ReportType  varchar(25),  
	@FilterType varchar(25),   
	@ISP varchar(100),  
	@PageNo int,  
	@PageSize int ,
	@UDFname varchar(100),
	@UDFdata varchar(100),
	@OnlyUnique bit = 0
	
)       
as        
  
Begin      
    declare @RecordNoStart int,  
			@RecordNoEnd int ,
			@query varchar (8000), 
			@innquery varchar(5000),
			@groupID varchar(100),
			@blastIDs varchar(4000)

 	Declare @reportdata TABLE (id int identity(1,1), emailID int, emailaddress varchar(255), ActionDate datetime)  
	Declare @UDFFilter TABLE (emailID int)  


	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@PageNo * @PageSize + 1);
	SELECT @LastRec = (@FirstRec + @PageSize - 1); 
   
	CREATE TABLE #tempSuppressed 
	(   RowNum int,
		EmailAddress varchar(255),
		Reason varchar(50),
		BlastsAlreadySent varchar(255)
	)
		
	declare @b TABLE (bID int, gID int)
	insert into @b
	select blastID, groupID 
	from ecn5_communicator.dbo.Blast
	Where BlastID in 
		(select blastid from ecn5_communicator..CampaignItemBlast where CampaignItemID = @CampaignItemID and IsDeleted = 0) and
		CustomerID = @CustomerID and StatusCode <> 'Deleted'
	
	if @ReportType = 'send'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			SELECT count(SendID) as 'Total'  
			from BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID  JOIN 
					@b ids ON ids.bID = bas.blastID
			WHERE e.emailaddress like '%' + @ISP 
			
			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY SendTime DESC, EmailAddress ASC) as 'RowNum', 
					SendTime AS SendTime, e.EmailAddress
				FROM 
					BlastActivitySends bas WITH (NOLOCK)
					JOIN @b ids ON ids.bID = bas.blastID 
					join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bas.emailID 
				WHERE  
					e.emailaddress like '%' + @ISP 
			)				
			SELECT SendTime, EmailAddress
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF;

		end
		else
		Begin
			SELECT 
				count(SendID) AS 'Total'  
			FROM 
				BlastActivitySends bas WITH (NOLOCK)   
				JOIN @b ids ON ids.bID = bas.blastID
			
			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY SendTime DESC, EmailAddress ASC) as 'RowNum', 
					SendTime AS SendTime, e.EmailAddress
				FROM 
					BlastActivitySends bas WITH (NOLOCK)
					JOIN @b ids ON ids.bID = bas.blastID 
					join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bas.emailID 
			)		
			SELECT SendTime, EmailAddress
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF;
		end
	END  
	else if @ReportType = 'open' 
	Begin
		if @FilterType = 'activeopens'
		Begin
			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				select	TOP 15 COUNT(e.EmailID) AS ActionCount,  E.emailaddress, 'EmailID=' + CONVERT(VARCHAR(100),e.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid) AS URL
				from	BlastActivityOpens bao with (NOLOCK) join 
						@b ids ON ids.bID = bao.blastID JOIN 
						ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID 
				WHERE 
						e.emailaddress like '%' + @ISP 
				group by 
						e.EmailID, Emailaddress, ids.gid order by ActionCount desc 
			End
			Else
			Begin
				select	TOP 15 COUNT(e.EmailID) AS ActionCount,  E.emailaddress, 'EmailID=' + CONVERT(VARCHAR(100),e.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid) AS URL
				from	BlastActivityOpens bao with (NOLOCK) join 
						@b ids ON ids.bID = bao.blastID JOIN 
						ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID
				group by 
						e.EmailID, Emailaddress, ids.gid order by ActionCount desc 
			End				
		end
		else if @FilterType = 'allopens'
		Begin
			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				-- Get the Total records count 
				select	Count(OpenID) 
				from	BlastActivityOpens bao with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID  JOIN 
					@b ids ON ids.bID = bao.blastID
				WHERE e.emailaddress like '%' + @ISP 
				
				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY OpenID DESC) as 'RowNum', 
						e.eMailID, e.emailaddress, OpenTime as ActionDate, BrowserInfo as ActionValue, 'EmailID=' + CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid)  AS URL
					FROM 
						BlastActivityOpens bao WITH (NOLOCK)
						JOIN @b ids ON ids.bID = bao.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bao.emailID 
					WHERE  
						right(emailaddress,convert(varchar,len(@ISP))) = @ISP
				)				
				SELECT eMailID, emailaddress, ActionDate, ActionValue, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;
				
			End
			Else
			Begin
				Select count(OpenID) from BlastActivityOpens bao with (NOLOCK)  JOIN 
					@b ids ON ids.bID = bao.blastID
				
				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY OpenID DESC) as 'RowNum', 
						e.EmailID, e.EmailAddress, OpenTime as ActionDate, BrowserInfo as ActionValue, 'EmailID=' + CONVERT(VARCHAR,e.EmailID)+'&GroupID=' + CONVERT(VARCHAR(100),ids.gid) AS URL
					FROM 
						BlastActivityOpens bao WITH (NOLOCK)
						JOIN @b ids ON ids.bID = bao.blastID
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bao.emailID
				)				
				SELECT EmailID, EmailAddress, ActionDate, ActionValue, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;
			End
		End

		--exec (@query)			
	End
	Else if @ReportType = 'resend'
	Begin 
		Select count(ResendID) from BlastActivityResends bar with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bar.emailID = e.emailID  JOIN 
				@b ids ON ids.bID = bar.blastID
		WHERE  e.emailaddress like '%' + @ISP       

		SET NOCOUNT ON;
		WITH TempResult as
		(
			SELECT 
				ROW_NUMBER() OVER(ORDER BY ResendID DESC) as 'RowNum', 
				bar.EMailID, e.EmailAddress, ResendTime as ActionDate, '' as ActionValue, 'EmailID='+CONVERT(VARCHAR,bar.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid) as URL
			FROM 
				BlastActivityResends bar WITH (NOLOCK)
				JOIN @b ids ON ids.bID = bar.blastID 
				join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bar.emailID 
			WHERE  
				e.emailaddress like '%' + @ISP 
		)				
		SELECT EmailID, EmailAddress, ActionDate, ActionValue, URL
		FROM TempResult
		WHERE RowNum >= @FirstRec 
		AND RowNum <= @LastRec
		SET NOCOUNT OFF;     

	End
	Else if @ReportType = 'refer'
	Begin 
		Select count(ReferID) from BlastActivityRefer bar with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bar.emailID = e.emailID  JOIN 
				@b ids ON ids.bID = bar.blastID
		WHERE  e.emailaddress like '%' + @ISP       

		SET NOCOUNT ON;
		WITH TempResult as
		(
			SELECT 
				ROW_NUMBER() OVER(ORDER BY ReferID DESC) as 'RowNum', 
				bar.EMailID, e.EmailAddress, ReferTime as ActionDate, '' as ActionValue, 'EmailID='+CONVERT(VARCHAR,bar.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid) as URL
			FROM 
				BlastActivityRefer bar WITH (NOLOCK)
				JOIN @b ids ON ids.bID = bar.blastID 
				join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bar.emailID 
			WHERE  
				e.emailaddress like '%' + @ISP 
		)				
		SELECT EmailID, EmailAddress, ActionDate, ActionValue, URL
		FROM TempResult
		WHERE RowNum >= @FirstRec 
		AND RowNum <= @LastRec
		SET NOCOUNT OFF;     

	End
	Else if @ReportType = 'bounce'
	Begin
		if(len(@ISP) <> 0)
		BEGIN 
			 if lower(@FilterType) = '*'      
			 Begin      
				Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bab.emailID = e.emailID  JOIN 
					@b ids ON ids.bID = bab.blastID
				WHERE  e.emailaddress like '%' + @ISP        

				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY BounceID DESC) as 'RowNum', 
						bab.EMailID, e.EmailAddress, BounceTime as ActionDate, bc.BounceCode as ActionValue, bab.BounceMessage as ActionNotes, 'EmailID='+CONVERT(VARCHAR,bab.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid) as URL
					FROM 
						BlastActivityBounces bab WITH (NOLOCK)
						JOIN @b ids ON ids.bID = bab.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bab.emailID 
						join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
					WHERE  
						e.emailaddress like '%' + @ISP 
				)				
				SELECT EmailID, EmailAddress, ActionDate, ActionValue, ActionNotes, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;       
			 end       
			 Else
			 Begin      

				Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bab.emailID = e.emailID  JOIN 
					@b ids ON ids.bID = bab.blastID join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
				WHERE  bc.BounceCode = @FilterType  AND e.emailaddress like '%' + @ISP        

				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY BounceID DESC) as 'RowNum', 
						bab.EMailID, e.EmailAddress, BounceTime as ActionDate, bc.BounceCode as ActionValue, BounceMessage as ActionNotes, 'EmailID='+CONVERT(VARCHAR,bab.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),ids.gid) as URL
					FROM 
						BlastActivityBounces bab WITH (NOLOCK)
						JOIN @b ids ON ids.bID = bab.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bab.emailID 
						join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
					WHERE  
						bc.BounceCode = @FilterType AND e.emailaddress like '%' + @ISP  
				)				
				SELECT EmailID, EmailAddress, ActionDate, ActionValue, ActionNotes, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;     
			 End      
		END
		ELSE 
		BEGIN

			Select count( BounceID) from BlastActivityBounces bab with (NOLOCK)  JOIN 
					@b ids ON ids.bID = bab.blastID join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
			WHERE  bc.BounceCode = (CASE WHEN @FilterType = '*' THEN bc.BounceCode ELSE @FilterType END)
		     
			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY ActionDate DESC) as 'RowNum', 
					e.EMailID, e.EmailAddress,inn.ActionDate,inn.ActionValue,inn.ActionNotes, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + CONVERT(VARCHAR(100),gid) as  URL
				from ecn5_communicator..Emails e with (NOLOCK) JOIN 
				( 
					  SELECT bab.EMailID, e.EmailAddress, BounceTime as ActionDate, bc.BounceCode as ActionValue, BounceMessage as ActionNotes , ids.gID     
					  FROM    ecn5_communicator..Emails e with (NOLOCK) JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID  JOIN 
								@b ids ON ids.bID = bab.blastID join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID         
					  WHERE       
						bc.BounceCode = (CASE WHEN @FilterType = '*' THEN bc.BounceCode ELSE @FilterType END)
				 ) inn ON e.EmailID = inn.EmailID
			)				
			SELECT EmailID, EmailAddress, ActionDate, ActionValue, ActionNotes, URL
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF; 
		 END  
	End
	else if @ReportType = 'subscribe'
	Begin
		if len(ltrim(rtrim(@ISP))) = 0  
		begin  
			if @OnlyUnique = 1
			begin
			--get count
				Select count(distinct bau.EmailID) from BlastActivityUnSubscribes bau with (NOLOCK) 
                  JOIN @b ids ON ids.bID = bau.blastID join UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
                  join ECN5_Communicator..Emails e with(nolock) on bau.EmailID = e.EmailID
                  WHERE  uc.UnsubscribeCode=@FilterType and bau.Comments like 
					case when @filterType = 'subscribe' 
						then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
						else '%'
					end

                  SET NOCOUNT ON;
                  

				  WITH TempResult as
                  (
                        SELECT 
                              ROW_NUMBER() OVER(ORDER BY bau.EmailID DESC) as 'RowNum', 
                              bau.EMailID, e.EmailAddress as EmailAddress, MIN(UnsubscribeTime) as UnsubscribeTime, MIN(Comments) as Reason, 
							  uc.UnsubscribeCode as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+CONVERT(VARCHAR(100),ids.gid) AS URL
                        FROM 
                              BlastActivityUnSubscribes bau WITH (NOLOCK)
                              JOIN @b ids ON ids.bID = bau.blastID 
                              join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID
                              join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
                        WHERE  
                              uc.UnsubscribeCode=@FilterType and bau.Comments like 
								case when @filterType = 'subscribe' 
									then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
									else '%'
								end
						GROUP BY bau.EmailID, e.EmailAddress, uc.UnsubscribeCode,ids.gID
                  )   
				  
				  
				                      
                  SELECT EmailID, EmailAddress, UnsubscribeTime as 'UnsubscribeTime', Reason as 'Reason', SubscriptionChange, URL
                  FROM TempResult
                  WHERE RowNum >= @FirstRec AND RowNum <= @LastRec 
				  ORDER BY UnsubscribeTime DESC
                  SET NOCOUNT OFF;  
            end  
		else if  @OnlyUnique = 0
			begin
				Select count(UnsubscribeID) from BlastActivityUnSubscribes bau with (NOLOCK) JOIN @b ids ON ids.bID = bau.blastID 
				join UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
				WHERE  uc.UnsubscribeCode=@FilterType and bau.Comments like 
					case when @filterType = 'subscribe' 
						then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
						else '%'
					end

				SET NOCOUNT ON;
				WITH TempResult as
				(
					SELECT 
						ROW_NUMBER() OVER(ORDER BY UnsubscribeID DESC) as 'RowNum', 
						bau.EMailID, e.EmailAddress as EmailAddress, UnsubscribeTime as UnsubscribeTime, Comments as Reason, 
						uc.UnsubscribeCode as SubscriptionChange, 
						'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+CONVERT(VARCHAR(100),ids.gid) AS URL
					FROM 
						BlastActivityUnSubscribes bau WITH (NOLOCK)
						JOIN @b ids ON ids.bID = bau.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID
						join UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
					WHERE  
						uc.UnsubscribeCode=@FilterType  and bau.Comments like 
							case when @filterType = 'subscribe' 
								then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
								else '%'
							end
				)				
				SELECT EmailID, EmailAddress, UnsubscribeTime, Reason, SubscriptionChange, URL
				FROM TempResult
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				
				SET NOCOUNT OFF;  
			end  
		end 
		
		else  
		Begin  
			if @OnlyUnique = 1
				begin
					Select count(distinct bau.EmailID) from BlastActivityUnSubscribes bau with (NOLOCK) join ecn5_communicator..Emails e with (NOLOCK) on bau.emailID = e.emailID  JOIN 
						@b ids ON ids.bID = bau.blastID join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  uc.UnsubscribeCode=@FilterType  AND e.emailaddress like '%' + @ISP and bau.Comments like 
						case when @filterType = 'subscribe' 
							then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
							else '%'
						end
					SET NOCOUNT ON;
					WITH TempResult as
					(
						SELECT 
							ROW_NUMBER() OVER(ORDER BY bau.EmailID DESC) as 'RowNum', 
							bau.EMailID, e.EmailAddress as EmailAddress, MIN(UnsubscribeTime) as UnsubscribeTime, uc.UnsubscribeCode as SubscriptionChange,
							 MIN(Comments) as Reason, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+ CONVERT(VARCHAR(100),ids.gid) AS URL
						FROM 
							BlastActivityUnSubscribes bau WITH (NOLOCK)
							JOIN @b ids ON ids.bID = bau.blastID 
							join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID 
							join UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
						WHERE  
							uc.UnsubscribeCode=@FilterType and e.emailaddress like '%' + @ISP 
							and bau.Comments like 
								case when @filterType = 'subscribe' 
									then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
									else '%'
								end
						GROUP BY bau.EmailID, e.EmailAddress, uc.UnsubscribeCode,ids.gID
					)				
					SELECT EmailID, EmailAddress, UnsubscribeTime as 'UnsubscribeTime', SubscriptionChange, Reason as 'Reason', URL
					FROM TempResult
					WHERE RowNum >= @FirstRec AND RowNum <= @LastRec
					ORDER BY UnsubscribeTime DESC
					SET NOCOUNT OFF;   
				End  
				
			else if  @OnlyUnique = 0
				begin
					Select count(UnsubscribeID) from BlastActivityUnSubscribes bau with (NOLOCK) join ecn5_communicator..Emails e with (NOLOCK) on bau.emailID = e.emailID  JOIN 
						@b ids ON ids.bID = bau.blastID join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  uc.UnsubscribeCode=@FilterType  AND e.emailaddress like '%' + @ISP  and bau.Comments like 
						case when @filterType = 'subscribe' 
							then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
							else '%'
						end      
					SET NOCOUNT OFF;  


					SET NOCOUNT ON;
					WITH TempResult as
					(
						SELECT 
							ROW_NUMBER() OVER(ORDER BY UnsubscribeID DESC) as 'RowNum', 
							bau.EMailID, e.EmailAddress as EmailAddress, UnsubscribeTime as UnsubscribeTime, uc.UnsubscribeCode as SubscriptionChange, Comments as Reason, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+ CONVERT(VARCHAR(100),ids.gid) AS URL
						FROM 
							BlastActivityUnSubscribes bau WITH (NOLOCK)
							JOIN @b ids ON ids.bID = bau.blastID 
							join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bau.emailID 
							join UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
						WHERE  
							uc.UnsubscribeCode=@FilterType and e.emailaddress like '%' + @ISP and bau.Comments like 
							case when @filterType = 'subscribe' 
								then '%FOR GROUP: ' + CONVERT(varchar(20), ids.gID) + '%' 
								else '%'
							end
					)				
					SELECT EmailID, EmailAddress, UnsubscribeTime, SubscriptionChange, Reason, URL
					FROM TempResult
					WHERE RowNum >= @FirstRec 
					AND RowNum <= @LastRec
					SET NOCOUNT OFF;   
				End  
		End 
	End
	else if @ReportType = 'noclick'  
	Begin  
		if len(rtrim(ltrim(@ISP))) > 0
		Begin
			SELECT	count(SendID) as 'Total'  
			from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID     JOIN 
					@b ids ON ids.bID = bas.blastID
			WHERE	
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK)    JOIN 
					@b ids ON ids.bID = bac.blastID)


			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY SendTime DESC, EmailAddress ASC) as 'RowNum', 
					SendTime as ActionDate, EmailAddress 
				FROM 
					BlastActivitySends bas WITH (NOLOCK)
					JOIN @b ids ON ids.bID = bas.blastID 
					join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bas.emailID 
				WHERE  
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK)  JOIN 
					@b ids ON ids.bID = bac.blastID)
			)				
			SELECT ActionDate, EmailAddress
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF; 
		end
		else
		Begin
			SELECT	count(SendID) as 'Total'  
			from	BlastActivitySends bas with (NOLOCK)  JOIN 
					@b ids ON ids.bID = bas.blastID
			WHERE	
					emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK) JOIN 
					@b ids ON ids.bID = bac.blastID)

			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY SendTime DESC, EmailAddress ASC) as 'RowNum', 
					SendTime as ActionDate, EmailAddress 
				FROM 
					BlastActivitySends bas WITH (NOLOCK)
					JOIN @b ids ON ids.bID = bas.blastID 
					join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bas.emailID 
				WHERE  
					bas.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac with (NOLOCK)  JOIN 
					@b ids ON ids.bID = bac.blastID)
			)				
			SELECT ActionDate, EmailAddress
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF; 
		end
	end
	else if @ReportType = 'suppressed'  
	Begin 
		if len(ltrim(rtrim(@ISP))) = 0  
		begin  
			if(@FilterType = 'ALL')
			begin
				Select count(SuppressID) from BlastActivitySuppressed basupp with (NOLOCK)  join @b ids ON ids.bID = basupp.blastID

				SET NOCOUNT ON;
				insert into #tempSuppressed
					SELECT 
						ROW_NUMBER() OVER(ORDER BY SuppressID) as 'RowNum', 
						e.EmailAddress as EmailAddress, sc.SupressedCode as Reason , basupp.BlastsAlreadySent
					FROM 
						BlastActivitySuppressed basupp WITH (NOLOCK)
						join SuppressedCodes sc on basupp.SuppressedCodeID = sc.SuppressedCodeID
						JOIN @b ids ON ids.bID = basupp.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = basupp.emailID				
				SELECT EmailAddress, Reason, BlastsAlreadySent
				FROM #tempSuppressed
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;  
			end
			else
			begin
				Select count(SuppressID) from BlastActivitySuppressed basupp with (NOLOCK)  join @b ids ON ids.bID = basupp.blastID join SuppressedCodes sc on basupp.SuppressedCodeID = sc.SuppressedCodeID
				WHERE  sc.SupressedCode = @FilterType 

				SET NOCOUNT ON;
				insert into #tempSuppressed
					SELECT 
						ROW_NUMBER() OVER(ORDER BY SuppressID) as 'RowNum', 
						e.EmailAddress as EmailAddress, sc.SupressedCode as Reason  ,basupp.BlastsAlreadySent
					FROM 
						BlastActivitySuppressed basupp WITH (NOLOCK)
						JOIN @b ids ON ids.bID = basupp.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = basupp.emailID 
						join ECN_ACTIVITY..SuppressedCodes sc on basupp.SuppressedCodeID = sc.SuppressedCodeID
					WHERE  
						sc.SupressedCode = @FilterType				
				SELECT EmailAddress, Reason,BlastsAlreadySent
				FROM #tempSuppressed
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF; 
			end 
		end  
		else  
		Begin  
			if(@FilterType = 'ALL')
			begin
				Select count(SuppressID) from BlastActivitySuppressed basupp with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on basupp.emailID = e.emailID  join @b ids ON ids.bID = basupp.blastID
				WHERE  e.emailaddress like '%' + @ISP        

				SET NOCOUNT ON;
				insert into #tempSuppressed
					SELECT 
						ROW_NUMBER() OVER(ORDER BY SuppressID) as 'RowNum', 
						e.EmailAddress as EmailAddress, sc.SupressedCode as Reason, basupp.BlastsAlreadySent
					FROM 
						BlastActivitySuppressed basupp WITH (NOLOCK)
						join ECN_ACTIVITY..SuppressedCodes sc on basupp.SuppressedCodeID = sc.SuppressedCodeID
						JOIN @b ids ON ids.bID = basupp.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = basupp.emailID 
					WHERE  
						basupp.blastID = @blastIDs and e.emailaddress like '%' + @ISP   			
				SELECT EmailAddress, Reason,BlastsAlreadySent
				FROM #tempSuppressed
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;
			end
			else
			begin
				Select count(SuppressID) 
				from 
					BlastActivitySuppressed basupp with (NOLOCK)  
					join ecn5_communicator..Emails e with (NOLOCK) on basupp.emailID = e.emailID  
					join @b ids ON ids.bID = basupp.blastID
					join ECN_ACTIVITY..SuppressedCodes sc on basupp.SuppressedCodeID = sc.SuppressedCodeID
				WHERE  sc.SupressedCode = @FilterType AND e.emailaddress like '%' + @ISP        

				SET NOCOUNT ON;
				insert into #tempSuppressed
					SELECT 
						ROW_NUMBER() OVER(ORDER BY SuppressID) as 'RowNum', 
						e.EmailAddress as EmailAddress, sc.SupressedCode as Reason, basupp.BlastsAlreadySent
					FROM 
						BlastActivitySuppressed basupp WITH (NOLOCK)
						JOIN @b ids ON ids.bID = basupp.blastID 
						join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = basupp.emailID
						join ECN_ACTIVITY..SuppressedCodes sc on basupp.SuppressedCodeID = sc.SuppressedCodeID 
					WHERE  
						sc.SupressedCode = @FilterType and e.emailaddress like '%' + @ISP  			
				SELECT EmailAddress, Reason,BlastsAlreadySent
				FROM #tempSuppressed
				WHERE RowNum >= @FirstRec 
				AND RowNum <= @LastRec
				SET NOCOUNT OFF;
			end 
		End
	end
	else if @ReportType = 'noopen'  
	Begin 
		if len(@ISP) > 0
		Begin
			SELECT	count(SendID) as 'Total'  
			from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID JOIN 
					@b ids ON ids.bID = bas.blastID
			WHERE	
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
					@b ids ON ids.bID = bao.blastID)

			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY SendTime DESC, EmailAddress ASC ) as 'RowNum', 
					SendTime as ActionDate, EmailAddress  
				FROM 
					BlastActivitySends bas WITH (NOLOCK)
					JOIN @b ids ON ids.bID = bas.blastID 
					join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bas.emailID 
				WHERE  
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
					@b ids ON ids.bID = bao.blastID) 
			)				
			SELECT ActionDate, EmailAddress
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF;
		end
		else
		begin
			insert into @reportdata (emailID, actiondate)  
			SELECT	bas.emailID,  
					SendTime as ActionDate  
			from	
					BlastActivitySends bas with (NOLOCK)  JOIN 
					@b ids ON ids.bID = bas.blastID
					join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID
			WHERE	
					bas.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
					@b ids1 ON ids1.bID = bao.blastID)
			order by 
					ActionDate DESC, EmailAddress ASC

			SELECT	count(SendID) as 'Total'	
			from BlastActivitySends bas with (NOLOCK)   JOIN 
					@b ids ON ids.bID = bas.blastID
			WHERE	
					emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
					@b ids1 ON ids1.bID = bao.blastID)
			
			SET NOCOUNT ON;
			WITH TempResult as
			(
				SELECT 
					ROW_NUMBER() OVER(ORDER BY SendTime DESC, EmailAddress ASC ) as 'RowNum', 
					SendTime as ActionDate, EmailAddress  
				FROM 
					BlastActivitySends bas WITH (NOLOCK)
					JOIN @b ids ON ids.bID = bas.blastID 
					join ecn5_communicator..Emails e WITH (NOLOCK) ON e.emailID = bas.emailID 
				WHERE  
					bas.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
					@b ids1 ON ids1.bID = bao.blastID)
			)				
			SELECT ActionDate, EmailAddress
			FROM TempResult
			WHERE RowNum >= @FirstRec 
			AND RowNum <= @LastRec
			SET NOCOUNT OFF;
		end
	end
	DROP TABLE #tempSuppressed
End