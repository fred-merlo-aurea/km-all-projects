CREATE PROCEDURE e_SearchResult_Select_IndividualID
@IndividualID int
AS
	SELECT  s.SubscriptionID,
			s.IndividualID,
			pub.PublicationID,
			pub.PublicationName,
			pub.PublicationCode,
			p.PublisherID,
			p.PublisherName,
			p.PublisherCode,
			i.Address1 + ' ' + i.Address2 + ', ' + i.City + ', ' + i.State + ', ' + i.ZipCode + '-' + i.Plus4 as 'FullAddress' 
	FROM Subscription s With(NoLock) 
	JOIN Publication pub With(NoLock) ON s.PublicationID = pub.PublicationID
	JOIN Publisher p With(NoLock) ON pub.PublisherID = p.PublisherID
	JOIN Individual i With(NoLock) ON s.IndividualID = i.IndividualID
	WHERE s.IndividualID = @IndividualID
	ORDER BY pub.PublicationCode
