CREATE PROCEDURE rpt_ClientFileLog_ClientID
@ClientID int,
@ClientName varchar(50)
AS
BEGIN

	set nocount on

	SELECT  @ClientID, @ClientName,
			s.SourceFileID,s.FileName,
			fst.CodeName as 'FileStatusName',fst.CodeValue as 'FileStatusCode',fst.CodeDescription as 'FileStatusDescription',
			f.LogDate,f.LogTime,f.Message
	FROM FileLog f With(NoLock)
	JOIN SourceFile s With(NoLock) ON f.SourceFileID = f.SourceFileID 
	JOIN UAD_Lookup..Code fst With(NoLock) ON f.FileStatusTypeID = fst.CodeId
	ORDER BY s.ClientID,s.FileName,f.LogDate,f.LogTime

END