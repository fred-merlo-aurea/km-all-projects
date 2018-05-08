CREATE  PROC [dbo].[e_QuoteItem_Exists_ByQuoteID] 
(
	@QuoteID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 qi.QuoteID
		from 
			QuoteItem qi with (nolock) join
			Quote q on qi.QuoteID = q.QuoteID
		where 
			qi.QuoteID = @QuoteID AND qi.IsDeleted = 0 and q.CustomerID = @customerID
	) SELECT 1 ELSE SELECT 0
END
