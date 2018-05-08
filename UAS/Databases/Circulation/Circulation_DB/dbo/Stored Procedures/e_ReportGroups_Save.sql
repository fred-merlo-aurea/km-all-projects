CREATE PROCEDURE [dbo].[e_ReportGroups_Save]
	@ReportGroupID int,
	@ResponseTypeID int,
	@DisplayName varchar(50),
	@DisplayOrder int
AS
	IF @ReportGroupID > 0
		BEGIN
			UPDATE ReportGroups
			SET 				
				ResponseTypeID = @ResponseTypeID,
				DisplayName = @DisplayName,
				DisplayOrder = @DisplayOrder
			WHERE ReportGroupID = @ReportGroupID 
		
			SELECT @ReportGroupID;
		END
	ELSE
		BEGIN
			INSERT INTO ReportGroups (ResponseTypeID,DisplayName,DisplayOrder)
			VALUES(@ResponseTypeID,@DisplayName,@DisplayOrder);SELECT @@IDENTITY;
		END		