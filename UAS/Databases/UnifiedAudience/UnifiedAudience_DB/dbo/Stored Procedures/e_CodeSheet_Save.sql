CREATE PROCEDURE [dbo].[e_CodeSheet_Save]
	@CodeSheetID int, 
	@PubID int, 
	@ResponseGroupID int, 
	@ResponseGroup varchar(255), 
	@ResponseValue varchar(255), 
	@ResponseDesc varchar(255),
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int,	
	@DisplayOrder int,	
	@ReportGroupID int,
	@IsActive bit,
	@WQT_ResponseID int,
	@IsOther bit
AS
BEGIN

	set nocount on

	IF @CodeSheetID > 0
	BEGIN						
		UPDATE CodeSheet
			SET ResponseGroupID = @ResponseGroupID, 
				ResponseGroup = @ResponseGroup, 
				ResponseValue = @ResponseValue, 
				ResponseDesc = @ResponseDesc,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				DisplayOrder = @DisplayOrder,	
				ReportGroupID = @ReportGroupID,
				IsActive = @IsActive,
				WQT_ResponseID = @WQT_ResponseID,
				IsOther = @IsOther
			WHERE CodeSheetID = @CodeSheetID
		
		SELECT @CodeSheetID;
	END
	ELSE
		BEGIN			
			INSERT INTO CodeSheet ([PubID], [ResponseGroupID], [ResponseGroup], [ResponseValue], [ResponseDesc], [DateCreated], [CreatedByUserID], [DisplayOrder], [ReportGroupID], [IsActive], [WQT_ResponseID], [IsOther])
			VALUES(@PubID, @ResponseGroupID, @ResponseGroup, @ResponseValue, @ResponseDesc, @DateCreated, @CreatedByUserID, @DisplayOrder, @ReportGroupID, @IsActive, @WQT_ResponseID, @IsOther);SELECT @@IDENTITY;
		END		
END