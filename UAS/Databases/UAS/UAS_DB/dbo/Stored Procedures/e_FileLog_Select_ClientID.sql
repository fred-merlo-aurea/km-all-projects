CREATE PROCEDURE e_FileLog_Select_ClientID
@ClientID int
AS
BEGIN

	set nocount on

	SELECT f.*
	FROM FileLog f With(NoLock)
	JOIN SourceFile s With(NoLock) ON f.SourceFileID = s.SourceFileID 
	WHERE s.ClientID = @ClientID 
	ORDER BY f.LogDate,f.LogTime,f.SourceFileID

END
GO