

CREATE PROCEDURE [dbo].[e_SourceFile_Delete_SourceFileByID]
@SourceFileID int,
@ClientID int
AS
UPDATE SourceFile
SET IsDeleted = 1
WHERE SourceFileID = @SourceFileID and ClientID = @ClientID;
Select @SourceFileID






