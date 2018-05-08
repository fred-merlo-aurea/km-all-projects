CREATE PROCEDURE [e_SourceFile_Select_SearchClientFileMapping] @ClientId int, @FileName varchar(100)
AS
BEGIN

	set nocount on

	SELECT * 
	FROM SourceFile With(NoLock)
	Where ClientID = @ClientId
	and (FileName = @FileName OR FileName + Extension = @FileName)

END