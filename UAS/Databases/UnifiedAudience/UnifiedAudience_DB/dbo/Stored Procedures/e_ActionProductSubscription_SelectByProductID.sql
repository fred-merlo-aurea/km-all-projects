CREATE PROCEDURE e_ActionProductSubscription_SelectByProductID
	@ProductID int
AS
BEGIN

	set nocount on

	Select SubscriptionID, PubSubscriptionID, PubCategoryID, PubTransactionID, cc.CategoryCodeValue, Copies, cct.CategoryCodeTypeName as 'CategoryType', tct.TransactionCodeTypeName as 'TransactionType'
	FROM PubSubscriptions ps  WITH (NOLOCK)
		JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..TransactionCode tc WITH (NOLOCK) ON tc.TransactionCodeID = ps.PubTransactionID
		JOIN UAD_Lookup..TransactionCodeType tct WITH (NOLOCK) ON tct.TransactionCodeTypeID = tc.TransactionCodeTypeID
	WHERE PubID = @ProductID AND ISNULL(IsInActiveWaveMailing,0) = 0

END