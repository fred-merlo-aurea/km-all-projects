CREATE  PROC dbo.e_CampaignItem_Select_Saleforce 
(
	@days int
)
AS 
BEGIN
	set @days = @days * -1
	SELECT ci.*, c.CustomerID 
	FROM CampaignItem ci WITH (NOLOCK)
		JOIN Campaign c WITH (NOLOCK) ON ci.CampaignID = c.CampaignID 
	WHERE 
			ci.IsDeleted = 0 
		and c.IsDeleted = 0 
		and ci.SFCampaignID is not null
		and ci.SendTime < GETDATE()
		and ci.SendTime > DATEADD(D, @days, DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0))
	
END