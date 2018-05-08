CREATE  PROC [dbo].[e_Quote_Exists_ByQuoteID] 
(
	@QuoteID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 QuoteID
		from 
			Quote  with (nolock)
		where 
			QuoteID = @QuoteID AND IsDeleted = 0 and CustomerID = @CustomerID
	) SELECT 1 ELSE SELECT 0
END
