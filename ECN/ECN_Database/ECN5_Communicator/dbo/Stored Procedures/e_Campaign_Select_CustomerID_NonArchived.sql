CREATE PROCEDURE [dbo].[e_Campaign_Select_CustomerID_NonArchived]   
@CustomerID int
AS
	SELECT * FROM Campaign WITH (NOLOCK) WHERE CustomerID = @CustomerID and IsDeleted = 0 and ISNULL(IsArchived, 0) = 0