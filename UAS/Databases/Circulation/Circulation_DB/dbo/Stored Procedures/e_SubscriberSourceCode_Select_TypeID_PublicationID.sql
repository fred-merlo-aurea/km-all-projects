CREATE PROCEDURE e_SubscriberSourceCode_Select_TypeID_PublicationID
@SubscriberSourceTypeID int,
@PublicationID int
AS
	SELECT * FROM SubscriberSourceCode With(NoLock) WHERE SubscriberSourceTypeID = @SubscriberSourceTypeID AND PublicationID = @PublicationID
