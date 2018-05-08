CREATE PROCEDURE e_SubscriptionStatus_Select_CatCodeID_TranCodeID
@CategoryCodeID int,
@TransactionCodeID int
AS    
  SELECT ss.*
  FROM SubscriptionStatus ss With(NoLock)
  JOIN SubscriptionStatusMatrix m With(NoLock) ON ss.SubscriptionStatusID = m.SubscriptionStatusID
  WHERE m.CategoryCodeID = @CategoryCodeID
  AND m.TransactionCodeID = @TransactionCodeID
