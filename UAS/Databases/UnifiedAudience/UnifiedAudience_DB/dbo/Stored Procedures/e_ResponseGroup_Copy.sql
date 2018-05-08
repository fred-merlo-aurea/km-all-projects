CREATE PROCEDURE [dbo].[e_ResponseGroup_Copy]	
	@srcResponseGroupID int,
	@destPubsXML TEXT  -- 	set @destPubsXML = '<XML><Pub ID="1" /> <Pub ID="2" /> <Pub ID="3" /> </XML>'
as
BEGIN

	SET NOCOUNT ON
	
	DECLARE @ResponseGroupName varchar(50),
			@docHandle int,
			@destPubID int,
			@destResponseGroupID int
			
	SELECT @ResponseGroupName = responsegroupname 
	from ResponseGroups with(nolock) 
	where ResponseGroupID = @srcResponseGroupID
	
	DECLARE @tbl_Destinationpub Table (PubID int)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @destPubsXML  

	INSERT into @tbl_Destinationpub
	SELECT PubID FROM OPENXML(@docHandle, N'/XML/Pub') WITH ( PubID int '@ID' )  

	EXEC sp_xml_removedocument @docHandle 

	DECLARE c_Emails CURSOR FOR SELECT PubID from @tbl_Destinationpub

	OPEN c_Emails  
	FETCH NEXT FROM c_Emails INTO @destPubID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		set @destResponseGroupID = 0
		SELECT @destResponseGroupID = responsegroupID 
		from ResponseGroups with(nolock) 
		where PubID = @destPubID and ResponseGroupName = @ResponseGroupName
		
		IF (@destResponseGroupID = 0)
			BEGIN
				INSERT into ResponseGroups (PubID,ResponseGroupName,DisplayName)
				SELECT @destPubID, ResponseGroupName, DisplayName 
				from ResponseGroups with(nolock) 
				where ResponseGroupID = @srcResponseGroupID
			
				SELECT @destResponseGroupID = SCOPE_IDENTITY()
			End

		IF ( @destResponseGroupID > 0)
			BEGIN
				DELETE 
				from CodeSheet_Mastercodesheet_Bridge 
				where CodeSheetID in (SELECT CodeSheetID from CodeSheet where ResponseGroupID = @destResponseGroupID )

				DELETE 
				from CodeSheet 
				where ResponseGroupID = @destResponseGroupID

				INSERT into codesheet (pubID, responsegroup, ResponseValue, Responsedesc, responsegroupID)

				SELECT @destPubID, @ResponseGroupName, ResponseValue, Responsedesc, @destResponseGroupID   
				FROM codesheet with(nolock) 
				where responsegroupID = @srcResponseGroupID   
			
				INSERT into CodeSheet_Mastercodesheet_Bridge (CodesheetID , MasterID)
				SELECT c2.codesheetID, cmb.masterID 
				FROM CodeSheet_Mastercodesheet_Bridge cmb with(nolock) 
					join codesheet c with(nolock) on cmb.codesheetID = c.codesheetID 
					join codesheet c2 with(nolock) on c2.responsevalue = c.responsevalue  
				where c.responsegroupID = @srcResponseGroupID and c2.responsegroupID = @destResponseGroupID
			End

		FETCH NEXT FROM c_Emails INTO @destPubID
	END

	CLOSE c_Emails  
	DEALLOCATE c_Emails  

End