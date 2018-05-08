CREATE PROCEDURE [dbo].[e_CustomerLinkTracking_Select_CustomerID]   
@CustomerID int
AS
	SELECT *
	FROM CustomerLinkTracking WITH (NOLOCK)
	WHERE CustomerID = @CustomerID and IsActive = 1