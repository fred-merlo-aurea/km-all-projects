CREATE PROCEDURE [dbo].[e_Rule_Delete_RuleID]
	@RuleID int
AS
	Delete from [Rule]
	where Rule_Seq_ID = @RuleID
