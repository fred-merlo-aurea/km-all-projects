CREATE PROCEDURE [dbo].[e_FileLog_Select_FileLog]
	@SourceFileID int,
    @ProcessCode varchar(50)
AS
BEGIN

	set nocount on

	IF LEN(@ProcessCode) > 0
		BEGIN
			Select * from FileLog With(NoLock) where ProcessCode = @ProcessCode
		END
	ELSE
		BEGIN
			Select * from FileLog With(NoLock) where SourceFileID = @SourceFileID
		END

END