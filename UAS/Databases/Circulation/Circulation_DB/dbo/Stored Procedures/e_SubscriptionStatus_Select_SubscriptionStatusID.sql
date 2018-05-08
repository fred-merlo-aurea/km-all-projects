CREATE PROCEDURE e_SubscriptionStatus_Select_SubscriptionStatusID
@SubscriptionStatusID int
AS    
  SELECT ss.*
  FROM SubscriptionStatus ss With(NoLock)
  WHERE ss.SubscriptionStatusID = @SubscriptionStatusID
