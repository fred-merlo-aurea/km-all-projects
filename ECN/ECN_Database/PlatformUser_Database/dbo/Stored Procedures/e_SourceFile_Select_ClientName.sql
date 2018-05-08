CREATE PROCEDURE [dbo].[e_SourceFile_Select_ClientName] @ClientName varchar(50)
AS
SELECT * 
FROM SourceFile With(NoLock)
Where ClientID =
(
	Select ClientID
	From Client With(NoLock)
	Where ClientName = @ClientName
)

