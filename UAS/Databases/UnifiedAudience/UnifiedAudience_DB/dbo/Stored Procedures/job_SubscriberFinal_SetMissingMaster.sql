CREATE PROCEDURE job_SubscriberFinal_SetMissingMaster
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @ign uniqueidentifier

	DECLARE c CURSOR
	FOR 
		SELECT IGrp_No
		FROM SubscriberFinal WITH(NoLock) 
		GROUP BY IGrp_No
		HAVING MIN(IGrp_Rank) = 'S'

	OPEN c

	FETCH NEXT FROM c INTO @ign

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		DECLARE @sfID int = (SELECT TOP 1 SubscriberFinalID FROM SubscriberFinal WHERE IGrp_No = @ign)
	
		UPDATE SubscriberFinal
		SET IGrp_Rank='M'
		WHERE SubscriberFinalID = @sfID
	
		FETCH NEXT FROM c INTO @ign
	END
	CLOSE c
	DEALLOCATE c

END