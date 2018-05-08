

CREATE PROCEDURE [e_SourceFile_Select_SearchClientFileMapping] @ClientName varchar(50), @FileName varchar(100)
AS
SELECT * 
FROM SourceFile With(NoLock)
Where ClientID =
(
	Select ClientId
	From Client With(NoLock)
	Where ClientName = @ClientName
)
and (FileName = @FileName OR FileName + Extension = @FileName)



