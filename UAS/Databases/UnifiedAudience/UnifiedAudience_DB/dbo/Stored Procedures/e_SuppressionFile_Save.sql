CREATE PROCEDURE [dbo].[e_SuppressionFile_Save]
	@SuppressionFileId int = 0,
	@FileName varchar(50),
	@FileDateModified datetime
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS(SELECT SuppressionFileId FROM SuppressionFile WHERE SuppressionFileId = @SuppressionFileId)
		BEGIN
			UPDATE SuppressionFile 
			SET FileName = @FileName,
				FileDateModified = @FileDateModified,
				dateupdated = GETDATE()
			WHERE SuppressionFileId = @SuppressionFileId
			SELECT @SuppressionFileId
		END
	ELSE
		BEGIN
			INSERT INTO SuppressionFile(FileName,FileDateModified,DateCreated,DateUpdated)
			VALUES (@FileName,@FileDateModified,GETDATE(),NULL)
		END
	

	SELECT @@IDENTITY;

END