CREATE PROCEDURE [dbo].[e_ConditionGroup_Select_MainGroupID]
	@MainGroupID int
AS
	Select * from ConditionGroup with(nolock)
	where MainGroup_ID = @MainGroupID
