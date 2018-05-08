CREATE PROCEDURE [dbo].[e_SourceFile_Select_SourceFileID] 
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM SourceFile With(NoLock)
	Where SourceFileID = @SourceFileID

END
GO