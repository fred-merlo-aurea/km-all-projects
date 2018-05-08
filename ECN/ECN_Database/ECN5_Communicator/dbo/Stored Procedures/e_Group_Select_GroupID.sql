CREATE PROCEDURE [dbo].[e_Group_Select_GroupID]  
@groupID int
AS
	SELECT * FROM [Groups] WITH(NOLOCK) WHERE GroupID = @groupID
