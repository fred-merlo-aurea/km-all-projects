CREATE PROCEDURE [dbo].[e_ApplicationUsers_Select_UserID]   
@UserID  uniqueidentifier
AS
BEGIN

	set nocount on

	Select * from ApplicationUsers With(NoLock) 
	where UserID = @UserID

END