CREATE PROCEDURE [dbo].[e_Orders_Select_userID]
@UserID uniqueidentifier = null
AS
BEGIN

	set nocount on

	select * from Orders with(nolock) where (UserID = @UserID or @UserID is null) and IsProcessed = 1

End