CREATE PROCEDURE e_History_Select_BatchID
@BatchID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM History With(NoLock) 
	WHERE BatchID = @BatchID

END