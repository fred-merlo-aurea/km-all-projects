CREATE  PROC [dbo].[e_QuoteItem_Delete_All] 
(
	@QuoteID int = NULL,
	@CustomerID int = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	UPDATE qi SET qi.IsDeleted = 1, qi.UpdatedDate = GETDATE(), qi.UpdatedUserID = @UserID
	FROM QuoteItem qi
	JOIN quote q WITH (NOLOCK) ON q.QuoteID = qi.QuoteId
	WHERE qi.QuoteID = @QuoteID
END
