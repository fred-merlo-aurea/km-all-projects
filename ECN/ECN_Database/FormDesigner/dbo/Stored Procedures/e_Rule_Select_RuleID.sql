CREATE PROCEDURE [dbo].[e_Rule_Select_RuleID]
	@RuleID int
AS
	Select * from [Rule] with(nolock)
	where Rule_Seq_ID = @RuleID