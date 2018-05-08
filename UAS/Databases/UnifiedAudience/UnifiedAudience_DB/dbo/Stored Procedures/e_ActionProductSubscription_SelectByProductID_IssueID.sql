CREATE PROCEDURE e_ActionProductSubscription_SelectByProductID_IssueID
	@ProductID int,
	@IssueID int
AS
BEGIN

	set nocount on

	Select SubscriptionID, PubSubscriptionID, PubCategoryID, PubTransactionID, cc.CategoryCodeValue, Copies, cct.CategoryCodeTypeName as 'CategoryType', tct.TransactionCodeTypeName as 'TransactionType'
       FROM IssueArchiveProductSubscription iaps
       JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = iaps.PubCategoryID
       JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
       JOIN UAD_Lookup..TransactionCode tc ON tc.TransactionCodeID = iaps.PubTransactionID
       JOIN UAD_Lookup..TransactionCodeType tct ON tct.TransactionCodeTypeID = tc.TransactionCodeTypeID
       WHERE PubID = @ProductID and IssueID = @IssueID

END
GO