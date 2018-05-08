
CREATE PROCEDURE [dbo].[e_Campaign_Select_CampaignName]   
@CampaignName varchar(100),
@CustomerID int
AS
	SELECT top 1 * FROM Campaign WITH (NOLOCK) WHERE CampaignName = @CampaignName and CustomerID = @CustomerID and IsDeleted = 0 order by CampaignID asc
