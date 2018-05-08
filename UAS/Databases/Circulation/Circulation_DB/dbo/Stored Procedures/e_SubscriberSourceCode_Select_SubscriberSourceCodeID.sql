CREATE PROCEDURE e_SubscriberSourceCode_Select_SubscriberSourceCodeID
@SubscriberSourceCodeID int
AS
	SELECT * FROM SubscriberSourceCode With(NoLock) WHERE SubscriberSourceCodeID = @SubscriberSourceCodeID
