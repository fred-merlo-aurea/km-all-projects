CREATE proc [dbo].[spCopyPubCodes]
(
	@FromPubID int, 
	@ToPubID int,
	@UserID int
)
as
BEGIN

	SET NOCOUNT ON

	--DECLARE @FromPubID int
	--DECLARE @ToPubID int
	
	--SET @FromPubID = 41
	--SET @ToPubID = 371
	
	DECLARE @dt datetime = getdate(),
			@UADCIRResponseGroupTypeID int
			
	select @UADCIRResponseGroupTypeID = CodeID from UAD_Lookup..Code c join UAD_Lookup..CodeType ct on c.CodeId = ct.CodeTypeId where ct.CodeTypeName = 'Response Group' and c.CodeName = 'Circ and UAD'

	if exists (select top 1 * from CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select CodeSheetID from CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID)))
			delete CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select CodeSheetID from CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID))

	if exists (select top 1 * from CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID))
			delete CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups where PubID = @ToPubID)
			
	if exists (select top 1 * from ResponseGroups where PubID = @ToPubID)
			delete ResponseGroups where PubID = @ToPubID
	
	--insert ResponseGroups		
	DECLARE @ReponseGroupIDNew int
	DECLARE @ReponseGroupID int
	DECLARE @ResponseGroupName varchar(100)
	DECLARE @DisplayName varchar(100)
	DECLARE c_ResponseGroups CURSOR FOR select ResponseGroupID, ResponseGroupName, DisplayName from ResponseGroups where PubID = @FromPubID
	OPEN c_ResponseGroups  
	FETCH NEXT FROM c_ResponseGroups INTO @ReponseGroupID, @ResponseGroupName, @DisplayName
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		SET @ReponseGroupIDNew = 0 

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
		Values
		(
			@ToPubID,
			@ResponseGroupName,
			@DisplayName,
			@dt,
			@UserID,
			0,
			0,
			1,
			null,
			@UADCIRResponseGroupTypeID	
		)
		
		SELECT @ReponseGroupIDNew = SCOPE_IDENTITY() 
		
		--insert CodeSheet
		DECLARE @CodeSheetIDNew int
		DECLARE @CodeSheetID int
		DECLARE @ResponseValue varchar(255)
		DECLARE @ResponseDesc varchar(255)
		DECLARE c_CodeSheet CURSOR FOR select CodeSheetID, ResponseValue, ResponseDesc from CodeSheet where ResponseGroupID = @ReponseGroupID
		OPEN c_CodeSheet
		FETCH NEXT FROM c_CodeSheet INTO @CodeSheetID, @ResponseValue, @ResponseDesc
		WHILE @@FETCH_STATUS = 0  
		BEGIN 
			SET @CodeSheetIDNew = 0 
			
			Insert into CodeSheet 
			( 
				PubID, 
				ResponseGroup, 
				Responsevalue, 
				Responsedesc, 
				ResponseGroupID,
				DateCreated,
				CreatedByUserID,
				DisplayOrder,
				IsActive,
				IsOther				 
			) 
			values 
			( 
				@ToPubID, 
				@ResponseGroupName, 
				@ResponseValue, 
				@ResponseDesc, 
				@ReponseGroupIDNew,
				@dt,
				@UserID,
				null,
				1,
				0
			)
	
			SELECT @CodeSheetIDNew = SCOPE_IDENTITY()
			
			--insert bridge
			DECLARE @MasterID int
			DECLARE c_Bridge CURSOR FOR select MasterID from CodeSheet_Mastercodesheet_Bridge where CodeSheetID = @CodeSheetID
			OPEN c_Bridge
			FETCH NEXT FROM c_Bridge INTO @MasterID
			WHILE @@FETCH_STATUS = 0  
			BEGIN
				Insert into CodeSheet_Mastercodesheet_Bridge ( CodeSheetID, MasterID ) values ( @CodeSheetIDNew, @MasterID )
				
				FETCH NEXT FROM c_Bridge INTO @MasterID
			END
			CLOSE c_Bridge  
			DEALLOCATE c_Bridge			
			
			FETCH NEXT FROM c_CodeSheet INTO @CodeSheetID, @ResponseValue, @ResponseDesc
		END
		CLOSE c_CodeSheet  
		DEALLOCATE c_CodeSheet		
		
		FETCH NEXT FROM c_ResponseGroups INTO @ReponseGroupID, @ResponseGroupName, @DisplayName
	END
	CLOSE c_ResponseGroups  
	DEALLOCATE c_ResponseGroups  		
	
	---------------------------------------------------------------------------------------------------------------
END
GO