CREATE PROCEDURE [e_HistoryToUserLog_Save]
@HistoryID int,
@UserLogID int
AS
BEGIN

	SET NOCOUNT ON

	IF(SELECT COUNT(*) FROM HistoryToUserLog with(nolock) WHERE HistoryID = @HistoryID AND UserLogID = @UserLogID) = 0
		BEGIN
			INSERT INTO HistoryToUserLog (HistoryID,UserLogID)
			VALUES(@HistoryID,@UserLogID)
		END

END