CREATE  PROC [dbo].[e_Campaign_Save] 
(
	@CampaignID int = NULL,
	@CustomerID int,
	@UserID int,
	@CampaignName varchar(100) = NULL,	
	@DripDesign text = NULL,
	@IsArchived bit = null
)
AS 
BEGIN
	IF @CampaignID is NULL or @CampaignID <= 0
	BEGIN
		INSERT INTO Campaign
		(
			CustomerID,CreatedUserID,CreatedDate,CampaignName,IsDeleted, DripDesign, IsArchived
		)
		VALUES
		(
			@CustomerID,@UserID,GetDate(),@CampaignName,0, @DripDesign, @IsArchived
		)
		SET @CampaignID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Campaign
			--SET CustomerID=@CustomerID,UserID=@UserID,CampaignName=@CampaignName
			SET CampaignName=@CampaignName,UpdatedUserID=@UserID,UpdatedDate=GETDATE(), DripDesign= @DripDesign, IsArchived = @IsArchived
		WHERE
			CampaignID = @CampaignID
	END

	SELECT @CampaignID
END