CREATE PROCEDURE [dbo].[e_Blast_Pause]
	@CustomerID int,
	@BlastID int,
	@IsPause bit,
	@userID int
AS
if @IsPause = 1
BEGIN
	UPDATE Blast
	set StatusCode = 'Paused' , UpdatedDate = GetDate(), UpdatedUserID = @userID
	where BlastID = @BlastID and CustomerID = @CustomerID and StatusCode = 'Pending'
END
ELSE IF @IsPause = 0
BEGIN
	UPDATE Blast
	set StatusCode = 'Pending' , UpdatedDate = GetDate(), UpdatedUserID = @userID
	where BlastID = @BlastID and CustomerID = @CustomerID and StatusCode = 'Paused'
END


