CREATE proc [dbo].[spCopyResponseGroups]
@srcResponseGroupID int,
@destPubsXML TEXT,  -- 	set @destPubsXML = '<XML><Pub ID="1" /> <Pub ID="2" /> <Pub ID="3" /> </XML>'
@UserID int
as
BEGIN
	
	SET NOCOUNT ON
	
	declare @ResponseGroupName varchar(50),
			@docHandle int,
			@destPubID int,
			@destResponseGroupID int
			
	select @ResponseGroupName = responsegroupname 
	from ResponseGroups 
	where ResponseGroupID = @srcResponseGroupID
	
	declare @tbl_Destinationpub Table (PubID int)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @destPubsXML  

	insert into @tbl_Destinationpub
	SELECT PubID FROM OPENXML(@docHandle, N'/XML/Pub') WITH ( PubID int '@ID' )  

	EXEC sp_xml_removedocument @docHandle 

	DECLARE c_Emails CURSOR FOR select PubID from @tbl_Destinationpub

	OPEN c_Emails  
	FETCH NEXT FROM c_Emails INTO @destPubID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		set @destResponseGroupID = 0
		select @destResponseGroupID = responsegroupID 
		from ResponseGroups 
		where PubID = @destPubID and ResponseGroupName = @ResponseGroupName
		
		if (@destResponseGroupID = 0)
		Begin
			Insert into ResponseGroups 
			(
				PubID,
				ResponseGroupName,
				DisplayName,
				DateCreated,
				CreatedByUserID,
				IsMultipleValue,
				IsRequired,
				IsActive,
				DisplayOrder,
				ResponseGroupTypeId				
			)
			select @destPubID, ResponseGroupName, DisplayName, GETDATE(), @UserID, IsMultipleValue, IsRequired, IsActive, null,  ResponseGroupTypeId from ResponseGroups where ResponseGroupID = @srcResponseGroupID
			
			select @destResponseGroupID = SCOPE_IDENTITY()
		End

		if ( @destResponseGroupID > 0)
		Begin
			if exists (select top 1 1 from CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) join Codesheet c with (NOLOCK)  on cmb.codesheetID = c.codesheetID where ResponseGroupID = @destResponseGroupID )
			Begin
				if not exists (select top 1 1 from PubSubscriptionDetail psd with (NOLOCK)  join Codesheet c with (NOLOCK)  on psd.codesheetID = c.codesheetID where ResponseGroupID = @destResponseGroupID)
				delete from CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select CodeSheetID from CodeSheet where PubID = @destPubID and ResponseGroupID = @destResponseGroupID )
			end
		                  
			if exists (select top 1 1 from CodeSheet with (NOLOCK) where ResponseGroupID = @destResponseGroupID)
				delete from CodeSheet where PubID = @destPubID and ResponseGroupID = @destResponseGroupID

			insert into codesheet 
			(
				pubID, 
				responsegroup, 
				ResponseValue, 
				Responsedesc, 
				responsegroupID,
				DateCreated,
				CreatedByUserID,
				DisplayOrder,
				IsActive,
				IsOther					
			)
			SELECT @destPubID, @ResponseGroupName, ResponseValue, Responsedesc, @destResponseGroupID, GETDATE(), @UserID, null, IsActive, IsOther   FROM codesheet where responsegroupID = @srcResponseGroupID   
			
			insert into CodeSheet_Mastercodesheet_Bridge (CodesheetID , MasterID)
			SELECT c2.codesheetID, cmb.masterID 
			FROM	CodeSheet_Mastercodesheet_Bridge cmb join 
					codesheet c on cmb.codesheetID = c.codesheetID join 
					codesheet c2 on c2.responsevalue = c.responsevalue  
				where c.responsegroupID = @srcResponseGroupID and 
					c2.responsegroupID = @destResponseGroupID
		End

		FETCH NEXT FROM c_Emails INTO @destPubID
	END

	CLOSE c_Emails  
	DEALLOCATE c_Emails  

End

