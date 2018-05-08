CREATE PROCEDURE [dbo].[o_FileLog_SelectDistinctProcessCodePerSourceFile]
	@ClientID int
AS
BEGIN

	set nocount on

	Select distinct FL.ProcessCode, FL.SourceFileID
	From FileLog FL With(NoLock)
		join SourceFile SF With(NoLock) on FL.SourceFileID = SF.SourceFileID
	where SF.ClientID = @ClientID
			and ProcessCode not in ('File Moving', 'Detected File', 'File Detected')
			and ProcessCode not like '%FileValidator%'

END