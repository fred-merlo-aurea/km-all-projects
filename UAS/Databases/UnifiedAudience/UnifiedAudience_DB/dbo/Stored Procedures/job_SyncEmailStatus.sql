CREATE PROCEDURE [job_SyncEmailStatus]
as
BEGIN

	SET NOCOUNT ON


	declare	@Email varchar(100),  
		@pubID int,
		@count int,
		@StatusUpdatedDate datetime
	    
	set @StatusUpdatedDate = GETDATE()
	update PubSubscriptions 
		set EmailStatusID= NULL, StatusUpdatedDate=null, StatusUpdatedReason = null 
		where isnull(Email, '') = ''

	DECLARE c_DupesinPubIDandEmail CURSOR for
	select Email, pubID  
	from PubSubscriptions with (NOLOCK)
	group by Email, pubID
	having COUNT(distinct emailstatusID) > 1 

	OPEN c_DupesinPubIDandEmail    
	  
	FETCH NEXT FROM c_DupesinPubIDandEmail INTO @Email,  @pubID 
	WHILE @@FETCH_STATUS = 0    
		BEGIN    
			print (@email + ' / ' + convert(varchar(100), @pubID)) -- + ' / select Email, pubID, emailstatusID  from PubSubscriptions where email = ''' + @email + ''' and pubID = ' + convert(varchar(100), @pubID))
	    
			if exists (select 1 from PubSubscriptions  with (NOLOCK) where PubID = @pubID and Email = @Email and EmailStatusID = 6)
				Begin 
					--print '               UnSubscribe'
					Update PubSubscriptions 
						set EmailStatusID = 6, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where PubID = @pubID and Email = @Email and EmailStatusID <> 6
				End
			else if exists (select 1 from PubSubscriptions  with (NOLOCK) where PubID = @pubID and Email = @Email and EmailStatusID = 4) 
				Begin 
					--print '               MasterSuppressed'
					Update PubSubscriptions 
						set EmailStatusID = 4, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where PubID = @pubID and Email = @Email and EmailStatusID <> 4
				End
			else if exists (select 1 from PubSubscriptions  with (NOLOCK) where PubID = @pubID and Email = @Email and EmailStatusID = 2) 
				Begin 
					--print '               Bounced'
					Update PubSubscriptions 
						set EmailStatusID = 2, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where PubID = @pubID and Email = @Email and EmailStatusID <> 2
				End
			else if exists (select 1 from PubSubscriptions  with (NOLOCK) where PubID = @pubID and Email = @Email and EmailStatusID = 5) 
				Begin 
					--print '               Spam'	
					Update PubSubscriptions 
						set EmailStatusID = 5, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where PubID = @pubID and Email = @Email and EmailStatusID <> 5
				End 
			else if exists (select 1 from PubSubscriptions  with (NOLOCK) where PubID = @pubID and Email = @Email and EmailStatusID = 1) 
				Begin 
					--print '               Active'	
					Update PubSubscriptions 
						set EmailStatusID = 1, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where PubID = @pubID and Email = @Email and EmailStatusID <> 1	
				End  
			FETCH NEXT FROM c_DupesinPubIDandEmail INTO @Email,  @pubID 
		End
	CLOSE c_DupesinPubIDandEmail    
	DEALLOCATE c_DupesinPubIDandEmail    
	  
	DECLARE c_DupesEmail CURSOR for
		select Email 
		from PubSubscriptions with(NOLOCK)
		group by Email
		having COUNT(distinct emailstatusID) > 1 

	 OPEN c_DupesEmail    
	  
	 FETCH NEXT FROM c_DupesEmail INTO @Email
	 WHILE @@FETCH_STATUS = 0    
		 BEGIN    
			print (@email) -- + ' / select Email, pubID, emailstatusID  from PubSubscriptions where email = ''' + @email + ''' and pubID = ' + convert(varchar(100), @pubID))
			if exists (select 1 from PubSubscriptions where Email = @Email and EmailStatusID = 4) 
				Begin 
					--print '               MasterSuppressed'
					Update PubSubscriptions 
						set EmailStatusID = 4, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where Email = @Email and EmailStatusID not in (4, 6)
				End
			else if exists (select 1 from PubSubscriptions where Email = @Email and EmailStatusID = 2) 
				Begin 
					--print '               Bounced'			
					Update PubSubscriptions 
						set EmailStatusID = 2, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where Email = @Email and EmailStatusID not in (2, 6)	
				End
			else if exists (select 1 from PubSubscriptions where Email = @Email and EmailStatusID = 5) 
				Begin 
					--print '               Spam'
					Update PubSubscriptions 
						set EmailStatusID = 5, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where Email = @Email and EmailStatusID not in (5, 6)	
				End 
			else if exists (select 1 from PubSubscriptions where Email = @Email and EmailStatusID = 1) 
				Begin 
					--print '               Active'	
					Update PubSubscriptions 
						set EmailStatusID = 1, StatusUpdatedDate = @StatusUpdatedDate, StatusUpdatedReason = 'job_SyncEmailStatus'
						where Email = @Email and EmailStatusID not in (1, 6)		
				End  
			FETCH NEXT FROM c_DupesEmail INTO @Email 
		 End    
	     
	CLOSE c_DupesEmail    
	DEALLOCATE c_DupesEmail  
	  
End