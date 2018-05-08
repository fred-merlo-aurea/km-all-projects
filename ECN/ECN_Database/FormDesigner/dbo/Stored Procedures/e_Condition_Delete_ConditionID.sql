CREATE PROCEDURE [dbo].[e_Condition_Delete_ConditionID]
	@Condition_Seq_ID int
AS
	Delete from Condition
	where Condition_Seq_ID = @Condition_Seq_ID