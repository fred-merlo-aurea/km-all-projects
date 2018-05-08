CREATE PROCEDURE [dbo].[e_User_Select_BaseChannelID] 
@BaseChannelID int
AS
BEGIN
	select u.* from [Users] u with(nolock) join Customer cu with(nolock) on u.CustomerID = cu.CustomerID
	where cu.BaseChannelID = @BaseChannelID and u.IsDeleted = 0 and cu.IsDeleted = 0
END
