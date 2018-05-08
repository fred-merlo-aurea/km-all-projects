create PROCEDURE [dbo].[e_Quote_Select_QuoteID]
@QuoteID int
AS

SELECT * FROM Quote WHERE QuoteID = @QuoteID and IsDeleted=0
