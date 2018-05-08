
CREATE PROCEDURE [dbo].[e_SubscriptionPaid_Select_SubscriptionID]
@SubscriptionID int
AS

Select * from SubscriptionPaid With(NoLock)
Where SubscriptionID = @SubscriptionID

