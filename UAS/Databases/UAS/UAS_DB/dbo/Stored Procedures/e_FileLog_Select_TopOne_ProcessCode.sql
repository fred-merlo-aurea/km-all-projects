CREATE PROCEDURE [dbo].[e_FileLog_Select_TopOne_ProcessCode]
	@ProcessCode varchar(50)
AS
BEGIN

	set nocount on

	SELECT Top 1 *
	FROM FileLog f With(NoLock) 
	where ProcessCode = @ProcessCode and SourceFileID != -1

END