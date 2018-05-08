CREATE PROCEDURE [dbo].[e_ReportGroups_Save]
	@ReportGroupID int,
	@ResponseGroupID int,
	@DisplayName varchar(50),
	@DisplayOrder int
AS
BEGIN

	SET NOCOUNT ON

	IF @ReportGroupID > 0
		BEGIN
			UPDATE ReportGroups
			SET ResponseGroupID = @ResponseGroupID,
				DisplayName = @DisplayName,
				DisplayOrder = @DisplayOrder
			WHERE ReportGroupID = @ReportGroupID 
		
			SELECT @ReportGroupID;
		END
	ELSE
		BEGIN
			INSERT INTO ReportGroups (ResponseGroupID,DisplayName,DisplayOrder)
			VALUES(@ResponseGroupID,@DisplayName,@DisplayOrder);SELECT @@IDENTITY;
		END	
		
END	