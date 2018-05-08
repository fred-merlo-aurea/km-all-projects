CREATE proc [dbo].[spActivelistBuilder]
as
Begin

	declare @customerID int,
			@dt datetime,
			@ActivityListGroupID int
			
	set @dt = GETDATE()

	set nocount on

	DECLARE c_cursor CURSOR FOR 
	select customerID from ecn5_accounts.dbo.[CUSTOMER] where BaseChannelID  = 60

	OPEN c_cursor  
	FETCH NEXT FROM c_cursor INTO @customerID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		set @ActivityListGroupID = 0
		
		select @ActivityListGroupID = GroupID from ecn5_communicator.dbo.Groups where CustomerID = @customerID and GroupName = 'Active User List' 
		
		if @ActivityListGroupID = 0
		Begin
			Insert into ecn5_communicator.dbo.Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,
					PublicFolder,OptinHTML,OptinFields,AllowUDFHistory,IsSeedList)
			values
				(@customerID, 0, 'Active User List', 'Automated job runs every night and moves the open & click users to a this group', 'customer', 0,
				0, '', '', 'N', 0)
			
			set @ActivityListGroupID = @@IDENTITY
		End;
		
			WITH tempEG AS 
			( 
				SELECT * from ecn5_communicator.dbo.EmailGroups where groupID = @ActivityListGroupID
			)
			MERGE tempEG as target
			USING  
			(	
				select  distinct bao.EmailID 
				from	ecn5_communicator.dbo.[BLAST] b join 
						ecn_Activity.dbo.BlastActivityOpens bao on b.blastID = bao.BlastID join
						ecn5_communicator.dbo.EmailGroups eg on eg.emailID = bao.EmailID and eg.GroupID = b.groupID join
						ecn5_communicator.dbo.Emails e with (NOLOCK) on eg.emailID = e.emailID left outer join
						ecn_misc..ConservativeContacts_clientseeds cc with (NOLOCK) on cc.emailaddress = e.emailaddress
				where	b.CustomerID = @customerID and 
						b.SendTime >= convert(varchar(10),DATEADD(dd, -7 , getdate()),101) and
						TestBlast = 'N' and 
						b.StatusCode ='sent' and
						eg.SubscribeTypeCode = 'S' 
						and bao.OpenTime between convert(varchar(10),DATEADD(dd, -1 , getdate()),101) and convert(varchar(10),DATEADD(dd, -1 , getdate()),101) + ' 23:59:59'
				union 
				select  distinct bac.EmailID 
				from	ecn5_communicator.dbo.[BLAST] b join 
						ecn_Activity.dbo.BlastActivityClicks bac on b.blastID = bac.BlastID join
						ecn5_communicator.dbo.EmailGroups eg on eg.emailID = bac.EmailID and eg.GroupID = b.groupID join
						ecn5_communicator.dbo.Emails e with (NOLOCK) on eg.emailID = e.emailID left outer join
						ecn_misc..ConservativeContacts_clientseeds cc with (NOLOCK) on cc.emailaddress = e.emailaddress
				where	b.CustomerID = @customerID and 
						b.SendTime >= convert(varchar(10),DATEADD(dd, -7 , getdate()),101) and
						TestBlast = 'N' and 
						b.StatusCode ='sent' and
						eg.SubscribeTypeCode = 'S' 
						and bac.ClickTime between convert(varchar(10),DATEADD(dd, -1 , getdate()),101) and convert(varchar(10),DATEADD(dd, -1 , getdate()),101) + ' 23:59:59'

			) AS Source 
		ON Target.EmailID = Source.EmailID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT 
			(
				EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged
			)
			values 
			(	
				Source.EmailID, @ActivityListGroupID, 'html', 'S', @dt, @dt
			);
	
		FETCH NEXT FROM c_cursor INTO @customerID
	END

	CLOSE c_cursor  
	DEALLOCATE c_cursor  
	
End
