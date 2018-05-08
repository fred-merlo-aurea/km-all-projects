CREATE PROCEDURE [dbo].[e_ConditionGroup_Select_ConditionGroupID]
	@ConditionGroupID int
AS
	Select * from ConditionGroup with(nolock)
	where ConditionGroup_Seq_ID = @ConditionGroupID
