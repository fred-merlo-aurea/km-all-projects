CREATE PROCEDURE [dbo].[e_BlastPlans_Select_CustomerID]   
@CustomerID int
AS
	SELECT * FROM BlastPlans WITH (NOLOCK) WHERE CustomerID = @CustomerID and IsDeleted = 0