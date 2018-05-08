CREATE PROCEDURE [dbo].[e_RuleCondition_DeleteByRuleID]   
@RuleID int,
@UserID int
AS

update RuleCondition set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where RuleID=@RuleID