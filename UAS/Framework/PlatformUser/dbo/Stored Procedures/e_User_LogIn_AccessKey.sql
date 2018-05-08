CREATE PROCEDURE [dbo].[e_User_LogIn_AccessKey]
@AccessKey uniqueidentifier
AS
	select *
	from [User] with(NoLock)
	where AccessKey = @AccessKey
	and IsAccessKeyValid = 'true'
	and IsActive = 'true'
