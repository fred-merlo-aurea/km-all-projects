CREATE PROCEDURE [dbo].[e_CampaignItemTemplate_Select_SetupLevel]
	@BaseChannelID int,
	@CustomerID int = null,
	@IsCustomer bit 
AS
	if @CustomerID is null
	BEGIN
		select * from CampaignItemTemplates cit with(nolock)
		join ECN5_Accounts..Customer c with(nolock) on cit.CustomerID = c.CustomerID
		where cit.OmnitureCustomerSetup = @IsCustomer and ISNULL(cit.IsDeleted,0) = 0 and c.BaseChannelID = @BaseChannelID and c.ActiveFlag = 'Y' and ISNULL(c.IsDeleted,0) = 0
		and (
				ISNULL(cit.Omniture1,'') <> '' or 
				ISNULL(cit.Omniture2, '') <> '' or 
				ISNULL(cit.Omniture3,'') <> '' or 
				ISNULL(cit.Omniture4,'') <> '' or 
				ISNULL(cit.Omniture5, '') <> '' or 
				ISNULL(cit.Omniture6,'') <> '' or 
				ISNULL(cit.Omniture7,'') <> '' or 
				ISNULL(cit.Omniture8,'') <> '' or
				ISNULL(cit.Omniture9,'') <> '' or
				ISNULL(cit.Omniture10, '') <> ''
			)
	END
	ELSE
	BEGIN
		select * from CampaignItemTemplates cit with(nolock)
			join ECN5_Accounts..Customer c with(nolock) on cit.CustomerID = c.CustomerID
		where cit.OmnitureCustomerSetup = @IsCustomer and ISNULL(cit.IsDeleted,0) = 0 and c.CustomerID = @CustomerID and cit.CustomerID = @CustomerID and c.ActiveFlag = 'Y' and ISNULL(c.IsDeleted,0) = 0
			and (
					ISNULL(cit.Omniture1,'') <> '' or 
					ISNULL(cit.Omniture2, '') <> '' or 
					ISNULL(cit.Omniture3,'') <> '' or 
					ISNULL(cit.Omniture4,'') <> '' or 
					ISNULL(cit.Omniture5, '') <> '' or 
					ISNULL(cit.Omniture6,'') <> '' or 
					ISNULL(cit.Omniture7,'') <> '' or 
					ISNULL(cit.Omniture8,'') <> '' or
					ISNULL(cit.Omniture9,'') <> '' or
					ISNULL(cit.Omniture10, '') <> ''
				)
	END