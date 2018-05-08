CREATE PROCEDURE [dbo].[e_Select_Users_ChannelID] 
@ChannelID int
AS
BEGIN
	select u.* from Users u with(nolock) join [Customer] cu with(nolock) on u.CustomerID = cu.CustomerID
	where cu.BaseChannelID = @ChannelID
END
