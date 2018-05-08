
CREATE PROCEDURE o_SubscriberMarketingMap_Select_SubscriberID
@SubscriberID int
AS

SELECT 
	m.MarketingID,
	m.MarketingName,
	m.MarketingCode,
	m.IsActive as 'MarketingIsActive',
	mm.SubscriberID,
	mm.PublicationID,
	mm.IsActive as 'MarketingMapIsActive',
	mm.DateCreated as 'MarketingMapDateCreated',
	mm.DateUpdated as 'MarketingMapDateUpdated',
	mm.CreatedByUserID as 'MarketingMapCreatedByUserID',
	mm.UpdatedByUserID as 'MarketingMapUpdatedByUserID'
FROM MarketingMap mm With(NoLock)
JOIN Marketing m With(NoLock) ON mm.MarketingID = m.MarketingID
WHERE mm.SubscriberID = @SubscriberID
