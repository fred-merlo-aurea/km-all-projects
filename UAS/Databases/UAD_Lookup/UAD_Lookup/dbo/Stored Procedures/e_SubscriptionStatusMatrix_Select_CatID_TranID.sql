CREATE PROCEDURE e_SubscriptionStatusMatrix_Select_CatID_TranID       
@CategoryCodeID int,
@TransactionCodeID int    
AS
BEGIN

	set nocount on

	SELECT *
	FROM SubscriptionStatusMatrix
	WHERE CategoryCodeID = @CategoryCodeID
	AND TransactionCodeID = @TransactionCodeID

END