create PROCEDURE [dbo].[e_QuoteItem_Select_QuoteID]   
@QuoteID int = NULL
AS
	SELECT qi.*, q.CustomerID 
	FROM QuoteItem qi WITH (NOLOCK)
		JOIN Quote q WITH (NOLOCK) ON qi.QuoteID = q.QuoteID
	WHERE qi.QuoteID = @QuoteID and Qi.IsDeleted = 0
