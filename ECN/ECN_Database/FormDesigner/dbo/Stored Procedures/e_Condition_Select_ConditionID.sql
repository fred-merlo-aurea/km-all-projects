CREATE PROCEDURE [dbo].[e_Condition_Select_ConditionID]
	@ConditionID int
AS
	Select * from Condition c with(nolock)
	where c.Condition_Seq_ID = @ConditionID
