CREATE PROCEDURE [dbo].[e_DeliverSubscriptionPaid_Select]
AS
	SELECT * FROM DeliverSubscriptionPaid With(NoLock) 
