CREATE PROCEDURE e_Batch_Select_UserID_IsActive
@UserID int,
@IsActive bit
AS
	SELECT *
	FROM Batch With(NoLock)
	WHERE UserID = @UserID AND IsActive = @IsActive
