CREATE VIEW  vw_Markets

AS
--THIS ONLY EXISTS TO WORK AROUND A LINKED SERVER ISSUE WITH XML
--IT CAN GO AWAY AS SOON AS spDataRefreshPart7 IS NO LONGER NEEDED

SELECT 
	MarketID,
	MarketName,
	CAST (MarketXML AS NVARCHAR(MAX)) AS MarketXML,
	BrandID,
	DateCreated,
	DateUpdated,
	CreatedByUserID,
	UpdatedByUserID
FROM
	Markets
