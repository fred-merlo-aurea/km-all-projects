CREATE PROCEDURE [dbo].[e_DomainTracker_Select_CustomerID]
@CustomerID int
AS
	SELECT * FROM DomainTracker cd WITH (NOLOCK) 
	WHERE cd.CustomerID=@CustomerID and cd.IsDeleted=0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTracker_Select_CustomerID] TO [ecn5]
    AS [dbo];

