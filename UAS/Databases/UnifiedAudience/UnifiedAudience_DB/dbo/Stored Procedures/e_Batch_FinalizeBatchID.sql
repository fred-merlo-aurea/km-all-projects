CREATE PROCEDURE [dbo].[e_Batch_FinalizeBatchID]
	@BatchID int
AS
BEGIN

	set nocount on

	IF @BatchID > 0
		BEGIN			
			UPDATE Batch
			SET 
				IsActive = 'false',
				DateFinalized = GETDATE()
			WHERE BatchID = @BatchID;
		
			SELECT @BatchID;
		END
	ELSE
		BEGIN
			SELECT @BatchID
		END
END