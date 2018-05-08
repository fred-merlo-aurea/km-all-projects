CREATE PROCEDURE [dbo].[e_DynamicTagRule_Delete]   
@DynamicTagRuleID int,
@UserID int
AS

update DynamicTagRule set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where DynamicTagRuleID=@DynamicTagRuleID