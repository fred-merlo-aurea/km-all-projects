CREATE PROCEDURE e_Propspect_Select_SubscriberID
@SubscriberID int
AS
	SELECT * FROM Prospect With(NoLock)
	WHERE SubscriberID = @SubscriberID
