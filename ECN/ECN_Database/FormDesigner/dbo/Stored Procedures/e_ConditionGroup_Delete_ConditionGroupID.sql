CREATE PROCEDURE [dbo].[e_ConditionGroup_Delete_ConditionGroupID]
	@ConditionGroup_Seq_ID int
AS
	Delete from ConditionGroup
	where ConditionGroup_Seq_ID = @ConditionGroup_Seq_ID