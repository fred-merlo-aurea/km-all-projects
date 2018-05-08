CREATE PROCEDURE [dbo].[e_SourceFile_Select_ClientID] @ClientID int
AS
SELECT * 
FROM SourceFile With(NoLock)
Where ClientID = @ClientID

