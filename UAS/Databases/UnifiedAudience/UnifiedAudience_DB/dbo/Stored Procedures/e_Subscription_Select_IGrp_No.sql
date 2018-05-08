CREATE PROCEDURE [dbo].[e_Subscription_Select_IGrp_No]
@IGrp_No  uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT *
    FROM Subscriptions  With(NoLock) 
	WHERE IGrp_No = @IGrp_No

END