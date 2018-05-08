CREATE PROCEDURE [dbo].[e_Group_Select_GetSeedLists]
	@CustomerID int
AS
	SELECT * FROM Groups g with(nolock)
	where g.CustomerID = @CustomerID and ISNULL(g.IsSeedList,0) = 1
