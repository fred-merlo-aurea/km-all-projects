CREATE PROCEDURE e_TempSubscription_Select_IndividualID
@IndividualID int
AS
SELECT SubscriptionID,Address + ', ' + City + ', ' + State + ', ' + Zip + '-' + Plus4 as 'FullAddress',
PublicationCode,Phone,EmailAddress as 'Email',PublicationID,PublisherID,IndividualID
From tmpImportSubscriptions With(NoLock)
WHERE IndividualID = @IndividualID
