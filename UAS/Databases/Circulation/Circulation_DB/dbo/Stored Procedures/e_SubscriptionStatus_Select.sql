CREATE PROCEDURE [dbo].[e_SubscriptionStatus_Select]

AS    
  SELECT *
  FROM SubscriptionStatus With(NoLock)

