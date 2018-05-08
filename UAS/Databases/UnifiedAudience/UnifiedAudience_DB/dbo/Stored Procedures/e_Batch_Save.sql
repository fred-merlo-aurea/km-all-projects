CREATE PROCEDURE [dbo].[e_Batch_Save]
@BatchID int,
@PublicationID int,
@UserID int,
@BatchCount int,
@IsActive bit,
@DateCreated datetime,
@DateFinalized datetime,
@BatchNumber int
AS
BEGIN

	set nocount on

	IF @BatchID > 0
		BEGIN			
			UPDATE Batch
			SET 
				PublicationID = @PublicationID,
				UserID = @UserID,
				BatchCount = @BatchCount,
				IsActive = @IsActive,
				DateFinalized = @DateFinalized,
				BatchNumber = @BatchNumber
			WHERE BatchID = @BatchID;
		
			SELECT @BatchID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END		
		
			declare @bn int = (select MAX(batchNumber) from Batch where PublicationID = @PublicationID) + 1
		 
			if(@bn) is null
				begin
					set	@bn = 1
				end	
			
			INSERT INTO Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,BatchNumber)
			VALUES(@PublicationID,@UserID,@BatchCount,@IsActive,@DateCreated,@bn);SELECT @@IDENTITY;
		
		END

END