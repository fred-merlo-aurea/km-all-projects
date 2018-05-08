CREATE PROCEDURE [dbo].[e_UserDataMask_Delete]
@UserID int
AS
BEGIN

	SET NOCOUNT ON

	delete from 
		UserDataMask 
	where 
		UserID = @UserID

END
