create procedure e_Batch_CloseBatches
@UserID int
as
BEGIN

	set nocount on

	update Batch
	set IsActive = 'false',
		DateFinalized = GETDATE()
	where UserID = @UserID AND IsActive = 'true' 

END
go