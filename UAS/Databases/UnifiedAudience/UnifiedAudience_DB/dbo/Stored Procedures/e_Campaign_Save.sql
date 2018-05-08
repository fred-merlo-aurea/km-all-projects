CREATE PROCEDURE [dbo].[e_Campaign_Save]
	@CampaignID int,
	@CampaignName varchar(100), 		
	@AddedBy int,
	@DateAdded DateTime,
	@UpdatedBy int,
	@DateUpdated DateTime,
	@BrandID int
AS
BEGIN

	set nocount on

	if (@brandID = 0)
	begin
		set @brandID =null;
	end
	
	If (@CampaignID > 0)
	BEGIN
		Update Campaigns
			set
			CampaignName = @CampaignName,			
			UpdatedBy = @UpdatedBy,
			DateUpdated = @DateUpdated,
			BrandID = @BrandID
			where CampaignID = @CampaignID 
		Select @CampaignID
	END
	Else
	BEGIN
		Insert Into Campaigns (CampaignName, AddedBy, DateAdded, UpdatedBy, DateUpdated, BrandID) values (@CampaignName, @AddedBy, @DateAdded, @UpdatedBy, @DateUpdated, @BrandID)
		select @@IDENTITY 			
	END
END
