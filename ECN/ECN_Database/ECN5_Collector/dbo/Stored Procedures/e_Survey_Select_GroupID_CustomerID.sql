CREATE PROCEDURE [dbo].[e_Survey_Select_GroupID_CustomerID]
	@GroupID int,
	@CustomerID int
AS
	SELECT * 
	FROM Survey s with(nolock)
	WHERE s.GroupID = @GroupID and s.CustomerID = @CustomerID and s.IsDeleted = 0
