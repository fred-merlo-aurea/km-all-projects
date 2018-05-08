CREATE PROCEDURE [dbo].[e_DomainTracker_Select_CustomerID]
@CustomerID int
AS
	SELECT * FROM DomainTracker cd WITH (NOLOCK) 
	WHERE cd.CustomerID=@CustomerID and cd.IsDeleted=0