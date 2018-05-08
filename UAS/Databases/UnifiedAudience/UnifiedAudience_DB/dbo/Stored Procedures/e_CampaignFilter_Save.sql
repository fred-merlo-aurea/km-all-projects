CREATE PROCEDURE [dbo].[e_CampaignFilter_Save]
(
	@CampaignID int,
	@FilterName varchar(500), 
	@UserID int,
	@PromoCode varchar(50)
)
AS
BEGIN

	set nocount on

	Insert Into CampaignFilter (CampaignID, FilterName, AddedBy, DateAdded, UpdatedBy, DateUpdated, PromoCode) values (@CampaignID, @FilterName, @UserID, GETDATE(), @UserID, GETDATE(), @PromoCode)
	select @@IDENTITY 			
END