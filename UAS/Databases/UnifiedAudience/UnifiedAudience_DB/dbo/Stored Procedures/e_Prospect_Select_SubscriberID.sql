﻿CREATE PROCEDURE e_Prospect_Select_SubscriberID
@SubscriberID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM Prospect With(NoLock) 
	WHERE SubscriberID = @SubscriberID

END