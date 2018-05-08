CREATE PROCEDURE [dbo].[e_ApplicationUsers_Select_Email]   
@Email  varchar(100)
AS
BEGIN

	set nocount on

	Select * from ApplicationUsers With(NoLock) 
	where Email = @Email

END