CREATE PROCEDURE e_History_Select_BatchID
@BatchID int
AS
	SELECT * FROM History With(NoLock) WHERE BatchID = @BatchID
