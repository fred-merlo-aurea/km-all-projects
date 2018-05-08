CREATE PROCEDURE [dbo].[e_Rule_Select_RuleID]   
@RuleID int
AS


SELECT r.* FROM [Rule] r WITH (NOLOCK)
WHERE r.RuleID = @RuleID and r.IsDeleted = 0