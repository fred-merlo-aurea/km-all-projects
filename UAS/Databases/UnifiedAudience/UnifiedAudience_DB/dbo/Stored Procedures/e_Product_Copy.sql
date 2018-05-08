CREATE PROCEDURE [dbo].[e_Product_Copy]
	@FromPubID int, 
	@ToPubID int
AS
BEGIN

	set nocount on

	if exists (select top 1 * from CodeSheet_Mastercodesheet_Bridge with(nolock) where CodeSheetID in (select CodeSheetID from CodeSheet with(nolock) where ResponseGroupID in (select ResponseGroupID from ResponseGroups with(nolock) where PubID = @ToPubID)))
			delete CodeSheet_Mastercodesheet_Bridge where CodeSheetID in (select CodeSheetID from CodeSheet with(nolock) where ResponseGroupID in (select ResponseGroupID from ResponseGroups with(nolock) where PubID = @ToPubID))

	if exists (select top 1 * from CodeSheet with(nolock) where ResponseGroupID in (select ResponseGroupID from ResponseGroups with(nolock) where PubID = @ToPubID))
			delete CodeSheet where ResponseGroupID in (select ResponseGroupID from ResponseGroups with(nolock) where PubID = @ToPubID)
			
	if exists (select top 1 * from ResponseGroups with(nolock) where PubID = @ToPubID)
			delete ResponseGroups where PubID = @ToPubID
	
	--insert ResponseGroups		
	DECLARE @ReponseGroupIDNew int
	DECLARE @ReponseGroupID int
	DECLARE @ResponseGroupName varchar(100)
	DECLARE @DisplayName varchar(100)
	DECLARE @Displayorder int
	DECLARE @IsMultipleValue bit
	DECLARE @IsRequired bit
	DECLARE @ResponseGroupTypeId int
	DECLARE c_ResponseGroups CURSOR FOR select ResponseGroupID, ResponseGroupName, DisplayName, Displayorder, IsMultipleValue, IsRequired, ResponseGroupTypeId from ResponseGroups with(nolock) where PubID = @FromPubID
	OPEN c_ResponseGroups  
	FETCH NEXT FROM c_ResponseGroups INTO @ReponseGroupID, @ResponseGroupName, @DisplayName, @Displayorder, @IsMultipleValue, @IsRequired, @ResponseGroupTypeId
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		SET @ReponseGroupIDNew = 0 
		Insert into ResponseGroups ( PubID, ResponseGroupName, DisplayName, Displayorder, IsMultipleValue, IsRequired, ResponseGroupTypeId ) 
			values ( @ToPubID, @ResponseGroupName, @DisplayName, @Displayorder, @IsMultipleValue, @IsRequired, @ResponseGroupTypeId )
		SELECT @ReponseGroupIDNew = SCOPE_IDENTITY() 
		
		--insert CodeSheet
		DECLARE @CodeSheetIDNew int
		DECLARE @CodeSheetID int
		DECLARE @ResponseValue varchar(255)
		DECLARE @ResponseDesc varchar(255)
		DECLARE @codesheetDisplayOrder int
		DECLARE @isActive bit
		DECLARE @isOther bit
		DECLARE c_CodeSheet CURSOR FOR select CodeSheetID, ResponseValue, ResponseDesc, DisplayOrder, isActive, isOther from CodeSheet where ResponseGroupID = @ReponseGroupID
		OPEN c_CodeSheet
		FETCH NEXT FROM c_CodeSheet INTO @CodeSheetID, @ResponseValue, @ResponseDesc, @codesheetDisplayOrder, @isActive, @isOther
		WHILE @@FETCH_STATUS = 0  
		BEGIN 
			SET @CodeSheetIDNew = 0 
			Insert into CodeSheet ( PubID, ResponseGroup, Responsevalue, Responsedesc, ResponseGroupID, DisplayOrder, isActive, isOther) 
				values ( @ToPubID, @ResponseGroupName, @ResponseValue, @ResponseDesc, @ReponseGroupIDNew, @codesheetDisplayOrder, @isActive, @isOther )
			SELECT @CodeSheetIDNew = SCOPE_IDENTITY()
			
			--insert bridge
			DECLARE @MasterID int
			DECLARE c_Bridge CURSOR FOR select MasterID from CodeSheet_Mastercodesheet_Bridge with(nolock) where CodeSheetID = @CodeSheetID
			OPEN c_Bridge
			FETCH NEXT FROM c_Bridge INTO @MasterID
			WHILE @@FETCH_STATUS = 0  
			BEGIN
				Insert into CodeSheet_Mastercodesheet_Bridge ( CodeSheetID, MasterID ) values ( @CodeSheetIDNew, @MasterID )
				
				FETCH NEXT FROM c_Bridge INTO @MasterID
			END
			CLOSE c_Bridge  
			DEALLOCATE c_Bridge			
			
			FETCH NEXT FROM c_CodeSheet INTO @CodeSheetID, @ResponseValue, @ResponseDesc, @codesheetDisplayOrder, @isActive, @isOther
		END
		CLOSE c_CodeSheet  
		DEALLOCATE c_CodeSheet		
		
		FETCH NEXT FROM c_ResponseGroups INTO @ReponseGroupID, @ResponseGroupName, @DisplayName, @Displayorder, @IsMultipleValue, @IsRequired, @ResponseGroupTypeId
	END
	CLOSE c_ResponseGroups  
	DEALLOCATE c_ResponseGroups  				
	
	---------------------------------------------------------------------------------------------------------------
END