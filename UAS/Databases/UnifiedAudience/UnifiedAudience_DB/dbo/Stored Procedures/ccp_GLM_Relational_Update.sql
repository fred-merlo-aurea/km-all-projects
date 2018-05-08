CREATE PROCEDURE [dbo].[ccp_GLM_Relational_Update]
AS
BEGIN

	set nocount on

	/* Intial Roll Up of Data */
	INSERT INTO tempGLMRelationalFinal ([EMAIL],[LEADSSENT],[LIKES],[BOARDFOLLOWS],[EXHIBITORFOLLOWS])	
	SELECT EMAIL,
		SUM(LEADSSENT) as LEADSSENT,
		SUM(LIKES) as LIKES,
		SUM(BOARDFOLLOWS) as BOARDFOLLOWS,
		SUM(EXHIBITORFOLLOWS) as EXHIBITORFOLLOWS  
	FROM tempGLMRelational With(NoLock)
	GROUP BY EMAIL
	
	/* 
		FROM HERE ON IT IS SPLIT INTO 4 GROUPS (LIKES, LEADSSENT, BOARDFOLLOWS, EXHIBITORFOLLOWS
		FIRST THING THAT IS DONE IS TO UPDATE CURRENT DATA THAT IS IN EXISTENCE
		SECOND THING IS TO INSERT DATA THAT DOESNT EXIST 
		THIRD UPDATE SUBSCRIBER FINAL THAT DATA WAS UPDATED AND NEEDS TO BE PUSHED TO LIVE 
	*/
	--------------------------------------------------------------------------------------	
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
			JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'Likes')
		BEGIN
			UPDATE SubscriberDemographicFinal	
			SET Value = CAST(CAST(Value as int) + GLM.LIKES as varchar(max))
			From tempGLMRelationalFinal GLM With(NoLock)
			JOIN SubscriberFinal SF With(NoLock) on GLM.EMAIL = SF.Email
			JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'Likes'	
		END
		
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'Likes'))
		BEGIN							
				INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
				SELECT 0,SF.SFRecordIdentifier,
				'Likes',GLM.LIKES,0,GETDATE(),GETDATE(),1,1				
				From tempGLMRelationalFinal GLM With(NoLock)
				JOIN SubscriberFinal SF With(NoLock) on GLM.EMAIL = SF.Email
				--FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SORecordIdentifier = SDF.SORecordIdentifier				
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'Likes')								
		END	
		
	UPDATE SubscriberFinal
	SET IsUpdatedInLive = 0
	From tempGLMRelationalFinal GLM
	JOIN SubscriberFinal SF on GLM.EMAIL = SF.Email
	JOIN SubscriberDemographicFinal SDF on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
	Where SDF.MAFField = 'Likes'	

	
	--------------------------------------------------------------------------------------
		
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
			JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'LeadsSent')
		BEGIN	
			UPDATE SubscriberDemographicFinal	
			SET Value = CAST(CAST(Value as int) + GLM.LEADSSENT as varchar(max))
			From tempGLMRelationalFinal GLM With(NoLock)
			JOIN SubscriberFinal SF With(NoLock) on GLM.EMAIL = SF.Email
			JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'LeadsSent'
		END
		
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'LeadsSent'))
		BEGIN							
				INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
				SELECT 0,SF.SFRecordIdentifier,
				'LeadsSent',GLM.LEADSSENT,0,GETDATE(),GETDATE(),1,1				
				From tempGLMRelationalFinal GLM With(NoLock)
				JOIN SubscriberFinal SF With(NoLock) on GLM.EMAIL = SF.Email
				--FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SORecordIdentifier = SDF.SORecordIdentifier				
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'LeadsSent')								
		END	
	
	UPDATE SubscriberFinal
	SET IsUpdatedInLive = 0
	From tempGLMRelationalFinal GLM
	JOIN SubscriberFinal SF on GLM.EMAIL = SF.Email
	JOIN SubscriberDemographicFinal SDF on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
	Where SDF.MAFField = 'LeadsSent'
	
	--------------------------------------------------------------------------------------
	
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
			JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'BoardFollows')
		BEGIN
			UPDATE SubscriberDemographicFinal	
			SET Value = CAST(CAST(Value as int) + GLM.BOARDFOLLOWS as varchar(max))
			From tempGLMRelationalFinal GLM
			JOIN SubscriberFinal SF on GLM.EMAIL = SF.Email
			JOIN SubscriberDemographicFinal SDF on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'BoardFollows'
		END
	
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'BoardFollows'))
		BEGIN							
				INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
				SELECT 0,SF.SFRecordIdentifier,
				'BoardFollows',GLM.BOARDFOLLOWS,0,GETDATE(),GETDATE(),1,1				
				From tempGLMRelationalFinal GLM With(NoLock)
				JOIN SubscriberFinal SF With(NoLock) on GLM.EMAIL = SF.Email	
				--FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SORecordIdentifier = SDF.SORecordIdentifie		
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'BoardFollows')								
		END	
	
	UPDATE SubscriberFinal
	SET IsUpdatedInLive = 0
	From tempGLMRelationalFinal GLM
	JOIN SubscriberFinal SF on GLM.EMAIL = SF.Email
	JOIN SubscriberDemographicFinal SDF on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
	Where SDF.MAFField = 'BoardFollows'
	
	--------------------------------------------------------------------------------------
	
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
			JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'ExhibitorFollows')
		BEGIN
			UPDATE SubscriberDemographicFinal	
			SET Value = CAST(CAST(Value as int) + GLM.EXHIBITORFOLLOWS as varchar(max))
			From tempGLMRelationalFinal GLM
			JOIN SubscriberFinal SF on GLM.EMAIL = SF.Email
			JOIN SubscriberDemographicFinal SDF on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			Where SDF.MAFField = 'ExhibitorFollows'
		END
	
	IF EXISTS (SELECT * From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'ExhibitorFollows'))
		BEGIN							
				INSERT INTO SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
				SELECT 0,SF.SFRecordIdentifier,
				'ExhibitorFollows',GLM.EXHIBITORFOLLOWS,0,GETDATE(),GETDATE(),1,1				
				From tempGLMRelationalFinal GLM With(NoLock)
				JOIN SubscriberFinal SF With(NoLock) on GLM.EMAIL = SF.Email
				--FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SORecordIdentifier = SDF.SORecordIdentifier				
				Where SF.SubscriberFinalID not in 
				(Select DISTINCT SubscriberFinalID From SubscriberFinal SF With(NoLock)
				FULL JOIN SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
				Where SDF.MAFField = 'ExhibitorFollows')								
		END	
	
	UPDATE SubscriberFinal
	SET IsUpdatedInLive = 0
	From tempGLMRelationalFinal GLM
	JOIN SubscriberFinal SF on GLM.EMAIL = SF.Email
	JOIN SubscriberDemographicFinal SDF on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
	Where SDF.MAFField = 'ExhibitorFollows'

END