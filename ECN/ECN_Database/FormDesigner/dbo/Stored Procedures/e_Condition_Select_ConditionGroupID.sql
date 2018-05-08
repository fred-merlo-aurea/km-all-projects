CREATE PROCEDURE [dbo].[e_Condition_Select_ConditionGroupID]
	@ConditionGroupID int
AS
	Select * from Condition c with(nolock)
	where c.ConditionGroup_Seq_ID = @ConditionGroupID
