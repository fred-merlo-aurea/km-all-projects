CREATE PROCEDURE [dbo].[e_ApplicationUsers_Select_RoleID]   
@RoleID  int
AS
BEGIN

	set nocount on

	Select * from ApplicationUsers  au With(NoLock) 
	join roles r  With(NoLock) on au.RoleID = r.RoleID  
	where au.RoleID = @RoleID

END