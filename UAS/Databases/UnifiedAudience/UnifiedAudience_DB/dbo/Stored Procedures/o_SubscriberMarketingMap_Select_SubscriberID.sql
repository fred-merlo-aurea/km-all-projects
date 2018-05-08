CREATE PROCEDURE o_SubscriberMarketingMap_Select_SubscriberID
@SubscriberID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT m.CodeID as 'MarketingID',
		m.CodeName as 'MarketingName',
		m.CodeValue as 'MarketingCode',
		m.IsActive as 'MarketingIsActive',
		mm.PubSubscriptionID as 'SubscriberID',
		mm.PublicationID,
		mm.IsActive as 'MarketingMapIsActive',
		mm.DateCreated as 'MarketingMapDateCreated',
		mm.DateUpdated as 'MarketingMapDateUpdated',
		mm.CreatedByUserID as 'MarketingMapCreatedByUserID',
		mm.UpdatedByUserID as 'MarketingMapUpdatedByUserID'
	FROM MarketingMap mm With(NoLock)
		JOIN UAD_Lookup..Code m With(NoLock) ON mm.MarketingID = m.CodeID
	WHERE mm.PubSubscriptionID = @SubscriberID

END