CREATE  PROC [dbo].[e_DomainSuppression_Delete_DomainSuppressionID] 
(
	@DomainSuppressionID int = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	UPDATE DomainSuppression SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE DomainSuppressionID = @DomainSuppressionID

END
