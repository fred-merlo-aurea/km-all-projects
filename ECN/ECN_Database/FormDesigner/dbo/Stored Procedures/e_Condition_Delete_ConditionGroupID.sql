CREATE PROCEDURE [dbo].[e_Condition_Delete_ConditionGroupID]
	@ConditionGroup_Seq_ID int
AS
	Delete from Condition
	where ConditionGroup_Seq_ID = @ConditionGroup_Seq_ID
