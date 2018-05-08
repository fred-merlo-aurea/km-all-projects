CREATE PROCEDURE [dbo].[e_SubscriberAddKill_UpdateSubscription]
	@SubscriptionIDs TEXT,
	@ProductID int,
	@AddRemoveID int,
	@DeleteAddRemoveID BIT
AS
	Declare  @docHandle int
	--set @SubscriptionIDs = '<XML><SUBSCRIBERS><ID>5</ID><ID>4</ID></SUBSCRIBERS></XML>'
	DECLARE @Subscriptions  Table (SubscriptionID varchar(20))	
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriptionIDs   
 
	Insert into @Subscriptions
	Select  ID
			FROM OPENXML(@docHandle, N'/XML/SUBSCRIBERS/ID')   
			WITH (ID int '.')  
	
	EXEC sp_xml_removedocument @docHandle  
	
	IF @DeleteAddRemoveID = 0
		UPDATE Subscription
		SET AddRemoveID = @AddRemoveID
		WHERE SubscriptionID IN (Select * FROM @Subscriptions)
		AND PublicationID = @ProductID
		
	ELSE
		UPDATE Subscription
		SET AddRemoveID = 0
		WHERE SubscriptionID IN (Select * FROM @Subscriptions)
		AND PublicationID = @ProductID
		AND AddRemoveID = @AddRemoveID
	
Return 0