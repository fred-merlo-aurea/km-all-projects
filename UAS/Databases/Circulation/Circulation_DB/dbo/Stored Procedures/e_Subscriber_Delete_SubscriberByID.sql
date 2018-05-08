
CREATE PROCEDURE [dbo].[e_Subscriber_Delete_SubscriberByID]
@SubscriberID int
AS
 Declare @ReseedSubscriberID int = @SubscriberID - 1;
 delete Subscriber where SubscriberID = @SubscriberID;
 DBCC CHECKIDENT (Subscriber, reseed, @ReseedSubscriberID);
 Select @SubscriberID;
