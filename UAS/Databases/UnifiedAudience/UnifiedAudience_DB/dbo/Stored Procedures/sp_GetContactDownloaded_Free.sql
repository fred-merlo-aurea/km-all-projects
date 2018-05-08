CREATE PROCEDURE [dbo].[sp_GetContactDownloaded_Free]
	@UserID	uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON

	select count(IsFreeDownload) 
	from Orderdetails od WITH(NOLOCK)
		join orders o WITH(NOLOCK) on od.OrderID = o.OrderID and SearchTypeID = 1
	where  od.UserID = @UserID and od.IsFreeDownload = 1  and MONTH(GETDATE()) =  MONTH(o.OrderDate)

end