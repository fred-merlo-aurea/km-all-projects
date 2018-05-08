CREATE  PROC [dbo].[e_Quote_Delete] 
(
    @QuoteID int = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	Update quote set IsDeleted = 1, UpdatedUserID=@UserID, UpdatedDate=GetDate() WHERE QuoteID = @QuoteID and CustomerID = @CustomerID
END
