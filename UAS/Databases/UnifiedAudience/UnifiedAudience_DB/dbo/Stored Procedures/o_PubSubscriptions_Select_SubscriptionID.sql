Create PROCEDURE [dbo].[o_PubSubscriptions_Select_SubscriptionID]  
@SubscriptionID int  
AS  
BEGIN

	SET NOCOUNT ON
 
	SELECT PS.*, P.PubCode, P.PubName, pt.PubTypeDisplayName , es.Status 
	FROM PubSubscriptions ps With(NoLock)   
		JOIN Pubs p With(NoLock)ON p.PubID = ps.PubID   
		Join PubTypes pt on pt.PubTypeID = p.PubTypeID 
		Join EmailStatus es on es.EmailStatusID = isnull(ps.EmailStatusID, 1)
	WHERE ps.SubscriptionID = @SubscriptionID
	  
End