CREATE PROCEDURE [dbo].[e_RuleCondition_Select_RuleID]   
@RuleID int
AS


SELECT rc.* FROM RuleCondition rc WITH (NOLOCK)
WHERE rc.RuleID = @RuleID and rc.IsDeleted = 0