CREATE  PROC [dbo].[e_Campaign_Delete] 
(
	@CustomerID int,
    @CampaignID int,
    @UserID int
)
AS 
BEGIN
	Update Campaign set IsDeleted = 1, UpdatedUserID=@UserID, UpdatedDate=GetDate() WHERE CampaignID = @CampaignID AND CustomerID = @CustomerID
END