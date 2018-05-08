CREATE PROCEDURE [dbo].[e_RequestQueryValue_Select_RuleID]
	@RuleID int
AS
	Select * from RequestQueryValue with(nolock)
	where Rule_Seq_ID = @RuleID
