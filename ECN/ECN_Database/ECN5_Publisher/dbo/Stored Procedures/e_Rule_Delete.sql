create PROCEDURE [dbo].[e_Rule_Delete]   
@RuleID int,
@UserID int

AS
	Update Rules SET IsDeleted = 1, UpdatedUserID = @UserID, UpdatedDate = GETDATE()  where  RuleID = @RuleID