CREATE PROCEDURE [dbo].[e_Batch_Create]
@UserID int,
@PublicationID int
AS
BEGIN

	set nocount on

	declare @BatchNumber int = (select MAX(batchNumber) from Batch where PublicationID = @PublicationID) + 1
	 
	if(@BatchNumber) is null
		begin
			set	@BatchNumber = 1
		end

	INSERT INTO Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,BatchNumber)
	VALUES(@PublicationID,@UserID,1,'true',GetDate(),@BatchNumber);SELECT * FROM Batch WHERE PublicationID = @PublicationID AND IsActive = 1;

END