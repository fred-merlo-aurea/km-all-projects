CREATE PROCEDURE [dbo].[e_Campaigns_Save]
(
	@CampaignName varchar(100), 
	@UserID int,
	@BrandID int
)
AS
BEGIN

	set nocount on

	if (@brandID = 0)
	begin
		set @brandID = null;
	end
	
	Insert Into Campaigns (CampaignName, AddedBy, DateAdded, UpdatedBy, DateUpdated, BrandID) values (@CampaignName, @UserID, GETDATE(), @UserID, GETDATE(), @BrandID)
	select @@IDENTITY 			
END