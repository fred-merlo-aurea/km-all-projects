CREATE proc [dbo].[MovedToActivity_spActivelistBuilder]
as
Begin

	declare @customerID int,
			@dt datetime,
			@ActivityListGroupID int
			
	set @dt = GETDATE()

	set nocount on

	DECLARE c_cursor CURSOR FOR 
	select customerID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where BaseChannelID  = 60

	OPEN c_cursor  
	FETCH NEXT FROM c_cursor INTO @customerID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		set @ActivityListGroupID = 0
		
		select @ActivityListGroupID = GroupID from Groups where CustomerID = @customerID and GroupName = 'Active User List' 
		
		if @ActivityListGroupID = 0
		Begin
			Insert into Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,
					PublicFolder,OptinHTML,OptinFields,AllowUDFHistory,IsSeedList)
			values
				(@customerID, 0, 'Active User List', 'Automated job runs every night and moves the open & click users to a this group', 'customer', 0,
				0, '', '', 'N', 0)
			
			set @ActivityListGroupID = @@IDENTITY
		End;
		
			WITH tempEG AS 
			( 
			SELECT * from EmailGroups where groupID = @ActivityListGroupID
			)
			MERGE tempEG as target
			USING  
			(	
				select  distinct eal.EmailID 
				from	[BLAST] b join 
						emailactivitylog eal on b.blastID = eal.BlastID join
						EmailGroups eg on eg.emailID = eal.EmailID and eg.GroupID = b.groupID
				where	b.CustomerID = @customerID and 
						b.SendTime >= convert(varchar(10),DATEADD(dd, -7 , getdate()),101) and
						TestBlast = 'N' and 
						b.StatusCode ='sent' and
						eg.SubscribeTypeCode = 'S' and
						(eal.ActionTypeCode='open' or  eal.ActionTypeCode='click') 
						and eal.ActionDate between convert(varchar(10),DATEADD(dd, -1 , getdate()),101) and convert(varchar(10),DATEADD(dd, -1 , getdate()),101) + ' 23:59:59'
			) AS Source 
		ON Target.EmailID = Source.EmailID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT 
			(
				EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged
			)
			values 
			(	Source.EmailID, @ActivityListGroupID, 'html', 'S', @dt, @dt
			);
	
		FETCH NEXT FROM c_cursor INTO @customerID
	END

	CLOSE c_cursor  
	DEALLOCATE c_cursor  
	
End
