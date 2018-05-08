CREATE VIEW  vw_Filters

AS
--THIS ONLY EXISTS TO WORK AROUND A LINKED SERVER ISSUE WITH XML
--IT CAN GO AWAY AS SOON AS spDataRefreshPart7 IS NO LONGER NEEDED

SELECT 
	FilterID,
	Name,
	CAST (FilterXML AS NVARCHAR(MAX)) AS FilterXML,
	CreatedDate,
	CreatedUserID,
	FilterType,
	PubID,
	IsDeleted,
	UpdatedDate,
	UpdatedUserID,
	BrandID,
	AddtoSalesView,
	FilterCategoryID,
	QuestionCategoryID,
	QuestionName
FROM
	Filters