CREATE PROCEDURE [e_SubscriptionStatusMatrix_Select_SubscriptionStatusID_CatID_TranID]       
@CategoryCodeID int,
@TransactionCodeID int    
AS
	SELECT *
	FROM SubscriptionStatusMatrix
	WHERE CategoryCodeID = @CategoryCodeID
	AND TransactionCodeID = @TransactionCodeID
