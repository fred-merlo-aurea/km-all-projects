CREATE PROCEDURE [dbo].[e_Batch_Create]
@UserID int,
@PublicationID int
AS

	declare @BatchNumber int = (select MAX(batchNumber) from Batch where PublicationID = @PublicationID) + 1
	 
	if(@BatchNumber) is null
	begin
		set	@BatchNumber = 1
	end


	INSERT INTO Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,BatchNumber)
	VALUES(@PublicationID,@UserID,0,'true',GetDate(),@BatchNumber);SELECT @@IDENTITY;
