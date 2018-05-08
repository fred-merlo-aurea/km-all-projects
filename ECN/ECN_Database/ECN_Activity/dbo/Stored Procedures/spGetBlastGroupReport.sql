CREATE PROCEDURE [dbo].[spGetBlastGroupReport]   
(  
	@ID int,   
	@IsBlastGroup varchar(1),   
	@ReportType  varchar(25),  
	@FilterType varchar(25),   
	@ISP varchar(100),  
	@PageNo int,  
	@PageSize int ,
	@UDFname varchar(100),
	@UDFdata    varchar(100)     
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


	Set @RecordNoStart = (@PageNo * @PageSize) + 1  
	Set @RecordNoEnd = (@PageNo * @PageSize) + 50  
	Set @groupID = ''
   
	if (len(@UDFname) > 0 and len(@UDFdata) > 0 and @IsBlastGroup <> 'Y')
	Begin
		select @groupID = groupID from ecn5_communicator..[BLAST] where blastID = @ID
		SELECT @blastIDs = @ID
	 
		insert into @UDFFilter
		select * from ecn5_communicator..[fn_Blast_Report_Filter_By_UDF](@blastIDs,@UDFname,@UDFdata )

 		SET ROWCOUNT @RecordNoEnd  
	 
		if @ReportType = 'send'  
		Begin  
			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				insert into @reportdata (emailID, emailaddress, actiondate)  
				SELECT e.emailID, e.EmailAddress, SendTime 
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID   join @UDFFilter t on bas.emailID = t.emailID
				WHERE bas.blastID = @blastIDs and e.emailaddress like '%' + @ISP   
				order by SendTime desc, EmailAddress  

				SELECT count(SendID) as 'Total'  
				from BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID join @UDFFilter t on bas.emailID = t.emailID
				WHERE bas.blastID = @blastIDs and e.emailaddress like '%' + @ISP  

				SELECT r.ActionDate As SendTime, r.EmailAddress   
				FROM  @reportdata r 

			end
			else
			Begin
				insert into @reportdata (emailID, actiondate)  
				SELECT bas.emailID, SendTime  
				from BlastActivitySends bas with (NOLOCK)   join @UDFFilter t on bas.emailID = t.emailID
				WHERE  bas.blastID = @blastIDs  
				order by SendTime desc
				
				SELECT count(SendID) as 'Total'  
				from BlastActivitySends bas with (NOLOCK)   join @UDFFilter t on bas.emailID = t.emailID
				WHERE bas.blastID = @blastIDs

				SELECT r.ActionDate As SendTime, e.EmailAddress   
				FROM  @reportdata r join ecn5_communicator..Emails e with (NOLOCK) on e.emailID = r.emailID 
				order by ActionDate desc, e.EmailAddress  
			end
		END  
		else if @ReportType = 'open'  
		Begin
			if @FilterType = 'activeopens'
			Begin

				select TOP 15 COUNT(e.EmailID) AS ActionCount,  E.emailaddress, 'EmailID=' + CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID  AS 'URL'
				from  BlastActivityOpens bao with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID  join @UDFFilter t on bao.emailID = t.emailID
				WHERE 
						BlastID = @blastIDs AND e.emailaddress like '%' + @ISP  
				group by e.EmailID, Emailaddress order by ActionCount desc 
			end
			else if @FilterType = 'allopens'
			Begin
				if len(rtrim(ltrim(@ISP))) > 0
				Begin
					-- Get the Total records count 
					select	Count(OpenID) as 'total'
					from	BlastActivityOpens bao with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID   join @UDFFilter t on bao.emailID = t.emailID
					WHERE bao.blastID = @blastIDs and e.emailaddress like '%' + @ISP 
					
					select e.eMailID, E.emailaddress, OpenTime as ActionDate, BrowserInfo as ActionValue, 'EmailID=' + CONVERT(VARCHAR,e.EmailID)+'&GroupID=' + @groupID  AS URL
					from  BlastActivityOpens bao with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID  join @UDFFilter t on bao.emailID = t.emailID
					WHERE BlastID = @blastIDs AND e.emailaddress like '%' + @ISP   ORDER BY OpenID DESC 
					
				End
				Else
				Begin

					Select count(OpenID) from BlastActivityOpens bao with (NOLOCK)  join @UDFFilter t on bao.emailID = t.emailID
					WHERE  BlastID = @blastIDs

					select e.EmailID, e.EmailAddress, OpenTime as ActionDate, BrowserInfo as ActionValue, 'EmailID=' + CONVERT(VARCHAR,e.EmailID)+'&GroupID=' + @groupID AS 'URL'
					from  BlastActivityOpens bao with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on e.EmailID = bao.EmailID join @UDFFilter t on bao.emailID = t.emailID
					WHERE BlastID = @blastIDs
					ORDER BY OpenID DESC
				End
			End

			exec (@query)			
		End
		Else if @ReportType = 'resend'
		Begin 
			Select count(ResendID) from BlastActivityResends bar with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bar.emailID = e.emailID join @UDFFilter t on bar.emailID = t.emailID
			WHERE  bar.blastID = @blastIDs AND e.emailaddress like '%' + @ISP       

			SELECT	bar.EMailID, e.EmailAddress, bar.ResendTime as ActionDate, '' as ActionValue, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID as 'URL' 
			FROM	ecn5_communicator..Emails e with (NOLOCK) JOIN 
					BlastActivityResends bar with (NOLOCK)  ON e.EMailID=bar.EMailID join @UDFFilter t on bar.emailID = t.emailID   
			WHERE  bar.blastID = @blastIDs and e.emailaddress like '%' + @ISP      

		End
		Else if @ReportType = 'refer'
		Begin 
			Select count(ReferID) from BlastActivityRefer bar with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bar.emailID = e.emailID join @UDFFilter t on bar.emailID = t.emailID
			WHERE  bar.blastID = @blastIDs AND e.emailaddress like '%' + @ISP       

			SELECT	bar.EMailID, e.EmailAddress, bar.ReferTime as ActionDate, bar.EmailAddress as ActionValue, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID as 'URL' 
			FROM	ecn5_communicator..Emails e with (NOLOCK) JOIN 
					BlastActivityRefer bar with (NOLOCK)  ON e.EMailID=bar.EMailID join @UDFFilter t on bar.emailID = t.emailID   
			WHERE  bar.blastID = @blastIDs AND e.emailaddress like '%' + @ISP      

		End
		Else if @ReportType = 'bounce'
		Begin
			if(len(@ISP) <> 0)
			BEGIN 
				 if lower(@FilterType) = '*'      
				 Begin      
					Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bab.emailID = e.emailID join @UDFFilter t on bab.emailID = t.emailID 
					WHERE  bab.blastID = @blastIDs AND e.emailaddress like '%' + @ISP        

					SELECT bab.EMailID, e.EmailAddress, bab.BounceTime as ActionDate, bc.BounceCode as ActionValue, bab.BounceMessage as ActionNotes, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID as 'URL'      
					FROM ecn5_communicator..Emails e with (NOLOCK) JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID join @UDFFilter t on bab.emailID = t.emailID join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID
					WHERE  bab.blastID = @blastIDs AND e.emailaddress like '%' + @ISP        
					ORDER BY bab.BounceID DESC      
				 end       
				 Else
				 Begin      

					Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  
												join ecn5_communicator..Emails e with (NOLOCK) on bab.emailID = e.emailID  
												join @UDFFilter t on bab.emailID = t.emailID
												join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
					WHERE  bab.blastID = @blastIDs and bc.BounceCode = @FilterType  AND e.emailaddress like '%' + @ISP        

					SELECT  bab.EMailID, e.EmailAddress, bab.BounceTime as ActionDate, bc.BounceCode as ActionValue, BounceMessage as ActionNotes, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID as 'URL'      
					FROM	ecn5_communicator..Emails e with (NOLOCK) 
							JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID 
							join @UDFFilter t on bab.emailID = t.emailID 
							join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID  
					WHERE       
							bab.blastID = @blastIDs and bc.BounceCode = @FilterType AND e.emailaddress like '%' + @ISP      
					ORDER BY BounceID DESC      
				 End      
			END
			ELSE 
			BEGIN

				Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  join @UDFFilter t on bab.emailID = t.emailID join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
				WHERE  bab.blastID = @blastIDs AND bc.BounceCode = (CASE WHEN @FilterType = '*' THEN bc.BounceCode ELSE @FilterType END)
			     
				SELECT e.EMailID, e.EmailAddress,inn.ActionDate,inn.ActionValue,inn.ActionNotes, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID as 'URL'   
				FROM ecn5_communicator..Emails e with (NOLOCK) JOIN 
				( 
					  SELECT bab.EMailID, e.EmailAddress, bab.BounceTime as ActionDate, bc.BounceCode as ActionValue, BounceMessage as ActionNotes  
					  FROM    ecn5_communicator..Emails e with (NOLOCK) JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID  join @UDFFilter t on bab.emailID = t.emailID join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID 
					  WHERE       
						bab.blastID = @blastIDs and bc.BounceCode = (CASE WHEN @FilterType = '*' THEN bc.BounceCode ELSE @FilterType END)
				 ) inn ON e.EmailID = inn.EmailID 
				ORDER BY ActionDate DESC
			 END  
		End
		else if @ReportType = 'subscribe'
		Begin
			if len(ltrim(rtrim(@ISP))) = 0  
			begin  

				Select count(UnsubscribeID) from BlastActivityUnSubscribes bau with (NOLOCK)  join @UDFFilter t on bau.emailID = t.emailID join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
				WHERE  bau.blastID = @blastIDs and uc.UnsubscribeCode=@FilterType 

				SELECT  bau.EMailID, e.EmailAddress as EmailAddress, UnsubscribeTime as UnsubscribeTime, Comments as Reason, uc.UnsubscribeCode as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+@groupID AS 'URL'  
				FROM   BlastActivityUnSubscribes bau with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bau.emailid = e.emailID join @UDFFilter t on bau.emailID = t.emailID join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
				where  bau.blastID = @blastIDs and uc.UnsubscribeCode=@FilterType      
				ORDER BY  UnsubscribeID desc  
			end  
			else  
			Begin  

					Select count(UnsubscribeID) from BlastActivityUnSubscribes bau with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bau.emailID = e.emailID  join @UDFFilter t on bau.emailID = t.emailID join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
					WHERE  bau.blastID = @blastIDs and uc.UnsubscribeCode=@FilterType  AND e.emailaddress like '%' + @ISP        

				SELECT  bau.EMailID, e.EmailAddress as EmailAddress, bau.UnsubscribeTime as UnsubscribeTime, uc.UnsubscribeCode as SubscriptionChange, Comments as Reason, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+ @groupID AS 'URL'  
				FROM   BlastActivityUnSubscribes bau with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bau.emailid = e.emailID  join @UDFFilter t on bau.emailID = t.emailID join UnsubscribeCodes uc on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
				where  bau.blastID = @blastIDs and uc.UnsubscribeCode=@FilterType and e.emailaddress like '%' + @ISP    
				ORDER BY  UnsubscribeID desc  
			End  
		End
		else if @ReportType = 'noclick'  
		Begin  
			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				insert into @reportdata (emailID, emailaddress, actiondate)  
				SELECT	e.emailID, e.EmailAddress, SendTime  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID join @UDFFilter t on bas.emailID = t.emailID   
				WHERE 
					bas.blastID = @blastIDs and 
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK) join @UDFFilter t on bac.emailID = t.emailID WHERE bac.blastID = @blastIDs)
				order by SendTime desc, EmailAddress  

				SELECT	count(SendID) as 'Total'  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID  join @UDFFilter t on bas.emailID = t.emailID  
				WHERE	
						bas.blastID = @blastIDs and 
						e.emailaddress like '%' + @ISP  and
						e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK) join @UDFFilter t on bac.emailID = t.emailID WHERE	bac.blastID = @blastIDs)


				SELECT r.ActionDate, r.EmailAddress   
				FROM  @reportdata r  
			end
			else
			Begin
				insert into @reportdata (emailID, actiondate)  
				SELECT	bas.emailID, SendTime  
				from	BlastActivitySends bas with (NOLOCK)  join @UDFFilter t on bas.emailID = t.emailID
				WHERE	bas.blastID = @blastIDs and 
						bas.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK) join @UDFFilter t on bac.emailID = t.emailID WHERE bac.blastID = @blastIDs)
				order by SendTime desc


				SELECT	count(SendID) as 'Total'  
				from	BlastActivitySends bas with (NOLOCK)  join @UDFFilter t on bas.emailID = t.emailID
				WHERE	bas.blastID = @blastIDs and 
						bas.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK) join @UDFFilter t on bac.emailID = t.emailID WHERE bac.blastID = @blastIDs)

				SELECT r.ActionDate, e.EmailAddress   
				FROM  @reportdata r join ecn5_communicator..Emails e with (NOLOCK) on e.emailID = r.emailID 
				order by ActionDate desc, e.EmailAddress 
			end
		end
		else if @ReportType = 'noopen'  
		Begin 
			if len(@ISP) > 0
			Begin

				insert into @reportdata (emailID, emailaddress, actiondate)  
				SELECT	e.emailID, e.EmailAddress, SendTime  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID join @UDFFilter t on bas.emailID = t.emailID
				WHERE	
						bas.blastID = @blastIDs and  
						e.emailaddress like '%' + @ISP  and
						e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) join @UDFFilter t on bao.emailID = t.emailID WHERE bao.blastID = @blastIDs)
				order by SendTime desc, EmailAddress  


				SELECT	count(SendID) as 'Total'  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID join @UDFFilter t on bas.emailID = t.emailID
				WHERE	
						bas.blastID = @blastIDs and 
						e.emailaddress like '%' + @ISP  and
						e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) join @UDFFilter t on bao.emailID = t.emailID WHERE bao.blastID = @blastIDs)

				SELECT r.ActionDate, r.EmailAddress   
				FROM  @reportdata r  
			end
			else
			begin
				insert into @reportdata (emailID, actiondate)  
				SELECT	bas.emailID,  
						SendTime  
				from	
						BlastActivitySends bas with (NOLOCK)  join @UDFFilter t on bas.emailID = t.emailID
				WHERE	
						bas.blastID = @blastIDs and 
						bas.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) join @UDFFilter t on bao.emailID = t.emailID WHERE bao.blastID = @blastIDs)
				order by 
						SendTime desc

				SELECT	count(SendID) as 'Total'	
				from BlastActivitySends bas with (NOLOCK)   join @UDFFilter t on bas.emailID = t.emailID
				WHERE	
						bas.blastID = @blastIDs and 
						bas.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) join @UDFFilter t on bao.emailID = t.emailID WHERE bao.blastID = @blastIDs)
				
				SELECT r.ActionDate, e.EmailAddress   
				FROM  @reportdata r join ecn5_communicator..Emails e with (NOLOCK) on e.emailID = r.emailID 
				order by ActionDate desc, e.EmailAddress 
			end
		end

	end
	Else
	Begin
		
		declare @b TABLE (items varchar(100))
		
		if @IsBlastGroup = 'Y'
		Begin
			SELECT @blastIDs = BlastIDs from ecn5_communicator..BlastGrouping WHERE BlastGroupID = @ID

			insert into @b 
			SELECT ITEMS FROM ecn5_communicator..fn_split(@blastIDs, ',')
		End
		else
		Begin
			select @groupID = groupID from ecn5_communicator..[BLAST] where blastID = @ID
			SELECT @blastIDs = @ID
			
			insert into @b 
			select @BlastIDs
		End
  			
 		SET ROWCOUNT @RecordNoEnd  
	 
		if @ReportType = 'send'  
		Begin  
			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				insert into @reportdata (emailID, emailaddress, actiondate)  
				SELECT e.emailID, e.EmailAddress, SendTime as ActionDate  
				from	BlastActivitySends bas with (NOLOCK)  join 
						ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID  JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE e.emailaddress like '%' + @ISP   
				order by ActionDate desc, EmailAddress  
		
				SELECT count(SendID) as 'Total'  
				from BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID  JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE e.emailaddress like '%' + @ISP  

				SELECT r.ActionDate As SendTime, r.EmailAddress   
				FROM  @reportdata r 

			end
			else
			Begin
				insert into @reportdata (emailID, actiondate)  
				SELECT bas.emailID, SendTime as ActionDate  
				from BlastActivitySends bas with (NOLOCK)    JOIN 
						@b ids ON ids.items = bas.blastID
				order by ActionDate desc
				
				SELECT count(SendID) as 'Total'  
				from BlastActivitySends bas with (NOLOCK)   JOIN 
						@b ids ON ids.items = bas.blastID

				SELECT r.ActionDate As SendTime, e.EmailAddress   
				FROM  @reportdata r join ecn5_communicator..Emails e with (NOLOCK) on e.emailID = r.emailID 
				order by ActionDate desc, e.EmailAddress  
			end
		END  
		else if @ReportType = 'open'  
		Begin
			if @FilterType = 'activeopens'
			Begin

				set @query = ' select TOP 15 COUNT(e.EmailID) AS ActionCount,  E.emailaddress, ''EmailID='' + CONVERT(VARCHAR,e.EmailID) + ''&GroupID=' + @groupID+'''  AS ''URL'' ' +
								' from  BlastActivityOpens bao with (NOLOCK) join ecn5_communicator..emails e with (NOLOCK) on bao.emailID = e.emailID ' +
								' WHERE BlastID in (' + @blastIDs + ') '

		--print (@query)

				if len(rtrim(ltrim(@ISP))) > 0
					set @query = @query + ' AND e.emailaddress like ''%' + @ISP + ''' '

				set @query = @query + ' group by e.EmailID, Emailaddress order by ActionCount desc '
			end
			else if @FilterType = 'allopens'
			Begin
				if len(rtrim(ltrim(@ISP))) > 0
				Begin
					-- Get the Total records count 
					select	Count(OpenID) 
					from	BlastActivityOpens bao with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bao.emailID = e.emailID  JOIN 
						@b ids ON ids.items = bao.blastID
					WHERE e.emailaddress like '%' + @ISP 
					
					set @query = ' select top ' + Convert(varchar,@RecordNoEnd) + ' e.eMailID, E.emailaddress, OpenTime as ActionDate, BrowserInfo as ActionValue, ''EmailID='' + CONVERT(VARCHAR,e.EmailID)+''&GroupID=' + @groupID+''' AS ''URL'' ' + 
								 ' from  BlastActivityOpens bao with (NOLOCK) join ecn5_communicator..emails e with (NOLOCK) on bao.emailID = e.emailID ' +
								 ' WHERE BlastID in (' + @blastIDs + ') AND right(emailaddress,'+ convert(varchar,len(@ISP))+') = ''' + @ISP + ''' ORDER BY OpenID DESC' 
					
				End
				Else
				Begin
					set @innquery = ' select top ' + Convert(varchar,@RecordNoEnd) + ' bao.EmailID, OpenTime as ActionDate, BrowserInfo as ActionValue ' + 
									' from  BlastActivityOpens bao with (NOLOCK) '+
									' WHERE BlastID in (' + @blastIDs + ') ORDER BY OpenID DESC'

					Select count(OpenID) from BlastActivityOpens bao with (NOLOCK)  JOIN 
						@b ids ON ids.items = bao.blastID

					set @query = ' SELECT e.EmailID, e.EmailAddress, inn.ActionDate, inn.ActionValue, ''EmailID='' + CONVERT(VARCHAR,e.EmailID)+''&GroupID=' + @groupID+''' AS ''URL'' '+
								 ' FROM ( ' + @innquery + ' )  inn join ecn5_communicator..Emails e with (NOLOCK) on e.EmailID = inn.EmailID'
				End
			End

			exec (@query)			
		End
		Else if @ReportType = 'resend'
		Begin 
			Select count(ResendID) from BlastActivityResends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bas.emailID = e.emailID  JOIN 
					@b ids ON ids.items = bas.blastID
			WHERE  e.emailaddress like '%' + @ISP       

			SELECT	bas.EMailID, e.EmailAddress, bas.ResendTime as ActionDate, '' as ActionValue, 'EmailID='+CONVERT(VARCHAR,bas.EmailID) + '&GroupID=' + @groupID as 'URL' 
			FROM	ecn5_communicator..Emails e with (NOLOCK) JOIN 
					BlastActivityResends bas with (NOLOCK)  ON e.EMailID=bas.EMailID   JOIN 
						@b ids ON ids.items = bas.blastID 
			WHERE  e.emailaddress like '%' + @ISP      

		End
		Else if @ReportType = 'refer'
		Begin 
			Select count(ReferID) from BlastActivityRefer bar with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bar.emailID = e.emailID  JOIN 
					@b ids ON ids.items = bar.blastID
			WHERE  e.emailaddress like '%' + @ISP       

			SELECT	bar.EMailID, e.EmailAddress, ReferTime as ActionDate, bar.EmailAddress as ActionValue, 'EmailID='+CONVERT(VARCHAR,bar.EmailID) + '&GroupID=' + @groupID as 'URL' 
			FROM	ecn5_communicator..Emails e with (NOLOCK) JOIN 
					BlastActivityRefer bar with (NOLOCK)  ON e.EMailID=bar.EMailID   JOIN 
						@b ids ON ids.items = bar.blastID 
			WHERE  e.emailaddress like '%' + @ISP      

		End
		Else if @ReportType = 'bounce'
		Begin
			if(len(@ISP) <> 0)
			BEGIN 
				 if lower(@FilterType) = '*'      
				 Begin      
					Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bab.emailID = e.emailID  JOIN 
						@b ids ON ids.items = bab.blastID
					WHERE  e.emailaddress like '%' + @ISP        

					SELECT bab.EMailID, e.EmailAddress, bab.BounceTime as ActionDate, bc.BounceCode as ActionValue, bab.BounceMessage as ActionNotes, 'EmailID='+CONVERT(VARCHAR,bab.EmailID) + '&GroupID=' + @groupID as 'URL'      
					FROM ecn5_communicator..Emails e with (NOLOCK) JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID  JOIN BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID JOIN 
							@b ids ON ids.items = bab.blastID        
					WHERE  e.emailaddress like '%' + @ISP        
					ORDER BY BounceID DESC      
				 end       
				 Else
				 Begin      

					Select count(BounceID) from BlastActivityBounces bab with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK) on bab.emailID = e.emailID  JOIN 
						@b ids ON ids.items = bab.blastID join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID
					WHERE  bc.BounceCode = @FilterType  AND e.emailaddress like '%' + @ISP        

					SELECT  bab.EMailID, e.EmailAddress, bab.BounceTime as ActionDate, bc.BounceCode as ActionValue, bab.BounceMessage as ActionNotes, 'EmailID='+CONVERT(VARCHAR,bab.EmailID) + '&GroupID=' + @groupID as 'URL'      
					FROM	ecn5_communicator..Emails e with (NOLOCK) JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID    JOIN 
							@b ids ON ids.items = bab.blastID join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID      
					WHERE       
							bc.BounceCode = @FilterType AND e.emailaddress like '%' + @ISP      
					ORDER BY BounceID DESC      
				 End      
			END
			ELSE 
			BEGIN

				Select count( BounceID) from BlastActivityBounces bab with (NOLOCK)  JOIN 
						@b ids ON ids.items = bab.blastID join BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID
				WHERE  bc.BounceCode = (CASE WHEN @FilterType = '*' THEN bc.BounceCode ELSE @FilterType END)
			     
				SELECT e.EMailID, e.EmailAddress,inn.ActionDate,inn.ActionValue,inn.ActionNotes, 'EmailID='+CONVERT(VARCHAR,e.EmailID) + '&GroupID=' + @groupID as 'URL'   
				FROM ecn5_communicator..Emails e with (NOLOCK) JOIN 
				( 
					  SELECT bab.EMailID, e.EmailAddress, bab.BounceTime as ActionDate, bc.BounceCode as ActionValue, bab.BounceMessage as ActionNotes      
					  FROM    ecn5_communicator..Emails e with (NOLOCK) JOIN BlastActivityBounces bab with (NOLOCK)  on e.EMailID=bab.EMailID  JOIN 
								@b ids ON ids.items = bab.blastID  join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID               
					  WHERE       
						bc.BounceCode = (CASE WHEN @FilterType = '*' THEN bc.BounceCode ELSE @FilterType END)
				 ) inn ON e.EmailID = inn.EmailID 
				ORDER BY ActionDate DESC
			 END  
		End
		else if @ReportType = 'subscribe'
		Begin
			if (@IsBlastGroup = 'Y')
			Begin
				if len(ltrim(rtrim(@ISP))) = 0  
				begin  
	
					Select count(bau.UnsubscribeCodeID) 
					from 
						BlastActivityUnSubscribes bau with (NOLOCK) 
						JOIN @b ids ON ids.items = bau.blastID 
						join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  uc.UnsubscribeCode=@FilterType 
	
					SELECT  bau.EMailID, e.EmailAddress as EmailAddress, bau.UnsubscribeTime as UnsubscribeTime, comments as Reason, uc.UnsubscribeCode as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+@groupID AS 'URL'  
					FROM   BlastActivityUnSubscribes bau with (NOLOCK) join ecn5_Communicator..emails e with (NOLOCK) on bau.emailid = e.emailID   JOIN 
						   @b ids ON ids.items = bau.blastID join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					where  uc.UnsubscribeCode=@FilterType      
					ORDER BY  UnsubscribeID desc  
				end  
				else  
				Begin
					Select count(bau.UnsubscribeCodeID) 
					from 
						BlastActivityUnSubscribes bau with (NOLOCK) 
						join ecn5_Communicator..emails e with (NOLOCK) on bau.emailID = e.emailID
						JOIN @b ids ON ids.items = bau.blastID 
						join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					WHERE  uc.UnsubscribeCode=@FilterType AND e.emailaddress like '%' + @ISP      
					
					SELECT  bau.EMailID, e.EmailAddress as EmailAddress, bau.UnsubscribeTime as UnsubscribeTime, comments as Reason, uc.UnsubscribeCode as SubscriptionChange, 'EmailID='+CONVERT(VARCHAR,bau.EmailID)+'&GroupID='+@groupID AS 'URL'  
					FROM   
						BlastActivityUnSubscribes bau with (NOLOCK) 
						join ecn5_Communicator..emails e with (NOLOCK) on bau.emailid = e.emailID   
						JOIN @b ids ON ids.items = bau.blastID 
						join UnsubscribeCodes uc with (nolock) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
					where  uc.UnsubscribeCode=@FilterType and e.emailaddress like '%' + @ISP      
					ORDER BY  UnsubscribeID desc 
				End  
			end
			else
			Begin
				exec spGetBlastGroupReport_for_Subscribe @ID, @FilterType, @ISP, @PageNo, @PageSize
			End
		End
		else if @ReportType = 'noclick'  
		Begin  
			if len(rtrim(ltrim(@ISP))) > 0
			Begin
				insert into @reportdata (emailID, emailaddress, actiondate)  
				SELECT	e.emailID, e.EmailAddress, bas.SendTime as ActionDate  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID    JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE 
					e.emailaddress like '%' + @ISP  and
					e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK)  JOIN 
						@b ids ON ids.items = bac.blastID )
				order by ActionDate desc, EmailAddress  

				SELECT	count(SendID) as 'Total'  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID     JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	
						e.emailaddress like '%' + @ISP  and
						e.emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac with (NOLOCK)    JOIN 
						@b ids ON ids.items = bac.blastID)


				SELECT r.ActionDate, r.EmailAddress   
				FROM  @reportdata r  
			end
			else
			Begin
				insert into @reportdata (emailID, actiondate)  
				SELECT	emailID, SendTime as ActionDate  
				from	BlastActivitySends bas with (NOLOCK)   JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac with (NOLOCK)  JOIN 
						@b ids ON ids.items = bac.blastID)
				order by ActionDate desc


				SELECT	count(SendID) as 'Total'  
				from	BlastActivitySends bas with (NOLOCK)  JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	
						emailID not in (SELECT distinct bac.EmailID FROM BlastActivityClicks bac  with (NOLOCK) JOIN 
						@b ids ON ids.items = bac.blastID)

				SELECT r.ActionDate, e.EmailAddress   
				FROM  @reportdata r join ecn5_communicator..Emails e with (NOLOCK) on e.emailID = r.emailID 
				order by ActionDate desc, e.EmailAddress 
			end
		end
		else if @ReportType = 'noopen'  
		Begin 
			if len(@ISP) > 0
			Begin

				insert into @reportdata (emailID, emailaddress, actiondate)  
				SELECT	e.emailID, e.EmailAddress, SendTime as ActionDate  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	
						e.emailaddress like '%' + @ISP  and
						e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
						@b ids ON ids.items = bao.blastID)
				order by ActionDate desc, EmailAddress  


				SELECT	count(SendID) as 'Total'  
				from	BlastActivitySends bas with (NOLOCK)  join ecn5_communicator..Emails e with (NOLOCK)  ON e.EmailID = bas.EmailID JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	
						e.emailaddress like '%' + @ISP  and
						e.emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
						@b ids ON ids.items = bao.blastID)

				SELECT r.ActionDate, r.EmailAddress   
				FROM  @reportdata r  
			end
			else
			begin
				insert into @reportdata (emailID, actiondate)  
				SELECT	emailID,  
						SendTime as ActionDate  
				from	
						BlastActivitySends bas with (NOLOCK)  JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	
						emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
						@b ids1 ON ids1.items = bao.blastID)
				order by 
						ActionDate desc

				SELECT	count(SendID) as 'Total'	
				from BlastActivitySends bas with (NOLOCK)   JOIN 
						@b ids ON ids.items = bas.blastID
				WHERE	
						emailID not in (SELECT distinct bao.EmailID FROM BlastActivityOpens bao  with (NOLOCK) JOIN 
						@b ids1 ON ids1.items = bao.blastID)
				
				SELECT r.ActionDate, e.EmailAddress   
				FROM  @reportdata r join ecn5_communicator..Emails e with (NOLOCK) on e.emailID = r.emailID 
				order by ActionDate desc, e.EmailAddress 
			end
		end
	end

	SET ROWCOUNT 0 

End
