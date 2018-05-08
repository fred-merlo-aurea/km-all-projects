CREATE PROCEDURE [dbo].[e_OverwritedataPostValue_Select_RuleID]
	@RuleID int
AS
	Select * from OverwritedataPostValue with(nolock)
	where Rule_Seq_ID = @RuleID
