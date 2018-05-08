CREATE PROCEDURE [dbo].[e_SubscriberClickActivity_Insert]
@PubSubscriptionID int,
@BlastID int,
@Link varchar(255),
@LinkAlias varchar(100),
@LinkSource varchar(50),
@LinkType varchar(50),
@ActivityDate date
AS
BEGIN

	SET NOCOUNT ON

	INSERT INTO SubscriberClickActivity (PubSubscriptionID,BlastID,Link,LinkAlias,LinkSource,LinkType,ActivityDate)
	VALUES(@PubSubscriptionID,@BlastID,@Link,@LinkAlias, @LinkSource,@LinkType,@ActivityDate);SELECT @@IDENTITY;

END