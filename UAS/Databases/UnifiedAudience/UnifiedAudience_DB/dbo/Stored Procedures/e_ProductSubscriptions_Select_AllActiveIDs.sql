CREATE Proc [dbo].[e_ProductSubscriptions_Select_AllActiveIDs]
(  
	@ProductID int
) AS
BEGIN

	SET NOCOUNT ON

	Select DISTINCT PubSubscriptionID, Copies
	FROM PubSubscriptions s with(nolock)
		JOIN UAD_Lookup..CategoryCode c with(nolock) on c.CategoryCodeID = s.PubCategoryID
		JOIN UAD_Lookup..TransactionCode t with(nolock) on t.TransactionCodeID = s.PubTransactionID
		JOIN UAD_Lookup..CategoryCodeType cg with(nolock) on cg.CategoryCodeTypeID = c.CategoryCodeTypeID
		JOIN UAD_Lookup..TransactionCodeType tg with(nolock) on tg.TransactionCodeTypeID = t.TransactionCodeTypeID
	WHERE s.PubID = @ProductID
		AND ((cg.CategoryCodeTypeID in (1,2) AND tg.TransactionCodeTypeID in (1)) OR(cg.CategoryCodeTypeID in (3,4) AND tg.TransactionCodeTypeID in (3)))
		AND c.CategoryCodeValue not in (70,71)
		AND s.IsInActiveWaveMailing = 0

END