CREATE PROCEDURE [dbo].[e_SourceFile_IsDelete_SourceFileID]
	@SourceFileID int,
	@IsDeleted int,
	@UpdatedByUserID int
AS
BEGIN

	set nocount on

	UPDATE SourceFile
	SET IsDeleted = @IsDeleted,
		UpdatedByUserID = @UpdatedByUserID,
		DateUpdated = GETDATE()
	WHERE SourceFileID = @SourceFileID
	Select @SourceFileID

END
