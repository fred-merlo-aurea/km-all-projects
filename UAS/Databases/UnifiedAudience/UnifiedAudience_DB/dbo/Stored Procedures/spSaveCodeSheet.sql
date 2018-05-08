CREATE proc [dbo].[spSaveCodeSheet]
@CodeSheetID int = 0, 
@PubID int, 
@ResponseGroupID int, 
@ResponseGroup varchar(255), 
@ResponseValue varchar(255), 
@ResponseDesc varchar(255),
@xmlDocument Text,
@CreatedByUserID int = 0,
@DateCreated datetime = null,
@UpdatedByUserID int = 0,
@DateUpdated datetime = null,
@IsActive bit,
@IsOther bit,
@ReportGroupID int
as
Begin
	set nocount on    	  
	DECLARE @docHandle int
	DECLARE @newID int
			
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument 
	
	if (@CodeSheetID =0)                                         
		Begin
		
			DECLARE @DisplayOrder int
			Select @DisplayOrder = max(DisplayOrder) + 1 From CodeSheet where ResponseGroupID = @ResponseGroupID
		
			INSERT INTO [CodeSheet]([PubID], [ResponseGroupID], [ResponseGroup], [ResponseValue], [ResponseDesc], [DateCreated] , [CreatedByUserID], [DisplayOrder], [IsActive], [IsOther], [ReportGroupID])
			VALUES (@PubID, @ResponseGroupID, @ResponseGroup, @ResponseValue, @ResponseDesc, @DateCreated, @CreatedByUserID, @DisplayOrder, @IsActive, @IsOther, @ReportGroupID)
			                 
			SELECT @newID = SCOPE_IDENTITY()
			
			INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID) 
			SELECT @newID, MasterID FROM OPENXML(@docHandle, N'/ROOT/RECORD')
				WITH (MasterID INT '@MasterID') 
		End
	Else
		Begin
			update 
				CodeSheet
			set 
				ResponseGroupID = @ResponseGroupID, 
				ResponseGroup = @ResponseGroup, 
				ResponseValue = @ResponseValue, 
				ResponseDesc = @ResponseDesc,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				IsActive = @IsActive,
				IsOther = @IsOther,
				ReportGroupID = @ReportGroupID
			where 
				CodeSheetID = @CodeSheetID
				
			delete 
				CodeSheet_Mastercodesheet_Bridge
			where  
				CodeSheetID = @CodeSheetID
			
			INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID) 
			SELECT @CodeSheetID, MasterID FROM OPENXML(@docHandle, N'/ROOT/RECORD')
				WITH (MasterID INT '@MasterID')						
		End	
End