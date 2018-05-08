CREATE PROCEDURE [dbo].[e_CampaignItemTemplate_ClearOmniData]
	@BaseChannelID int,
	@CustomerID int = null,
	@IsCustomer bit,
	@UserID int
AS
	if @CustomerID is null
	BEGIN
		Update cit 
		set Omniture1 = null, Omniture2 = null, Omniture3 = null, Omniture4 = null, Omniture5 = null, Omniture6 = null, Omniture7 = null, Omniture8 = null, Omniture9 = null, Omniture10 = null, cit.UpdatedDate = GETDATE(), cit.UpdatedUserID = @UserID
		from CampaignItemTemplates cit
		JOIN ECN5_Accounts..Customer c on cit.CustomerID = c.CustomerID
		where cit.OmnitureCustomerSetup = @IsCustomer and ISNULL(cit.IsDeleted,0) = 0 and c.BaseChannelID = @BaseChannelID and c.ActiveFlag = 'Y' and ISNULL(c.IsDeleted,0) = 0
	END
	ELSE
	BEGIN
		Update cit 
		set Omniture1 = null, Omniture2 = null, Omniture3 = null, Omniture4 = null, Omniture5 = null, Omniture6 = null, Omniture7 = null, Omniture8 = null, Omniture9 = null, Omniture10 = null, cit.UpdatedDate = GETDATE(), cit.UpdatedUserID = @UserID
		from CampaignItemTemplates cit
		JOIN ECN5_Accounts..Customer c on cit.CustomerID = c.CustomerID
		where cit.OmnitureCustomerSetup = @IsCustomer and ISNULL(cit.IsDeleted,0) = 0 and c.CustomerID = @CustomerID and cit.CustomerID = @CustomerID and c.ActiveFlag = 'Y' and ISNULL(c.IsDeleted,0) = 0
	END