CREATE PROCEDURE [dbo].[sp_GetOpportunityDownloaded_Free]
	@UserID	uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON

	select count(IsFreeDownload) 
	from Orderdetails od WITH(NOLOCK)
		join orders o WITH(NOLOCK) on od.OrderID = o.OrderID and SearchTypeID = 2
	where  od.UserID = @UserID and od.IsFreeDownload = 1  and MONTH(GETDATE()) =  MONTH(o.OrderDate)

END