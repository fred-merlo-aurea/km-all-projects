CREATE PROCEDURE [dbo].[e_RuleCondition_Delete]   
@RuleConditionID int,
@UserID int
AS

update RuleCondition set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where RuleConditionID=@RuleConditionID