CREATE PROCEDURE [dbo].[e_Rule_Delete]   
@RuleID int,
@UserID int
AS

update [Rule] set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE() 
where RuleID=@RuleID