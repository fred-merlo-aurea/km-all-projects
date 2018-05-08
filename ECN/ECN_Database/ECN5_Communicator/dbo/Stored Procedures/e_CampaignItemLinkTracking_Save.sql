CREATE  PROC [dbo].[e_CampaignItemLinkTracking_Save] 
(
	@CILTID int = NULL,
	@CampaignItemID int,
	@LTPID int,
	@LTPOID int = NULL,
	@CustomValue varchar(255) = NULL
)
AS 
BEGIN
	IF @CILTID is NULL or @CILTID <= 0
	BEGIN
		INSERT INTO CampaignItemLinkTracking
		(
			CampaignItemID,LTPID,LTPOID, CustomValue
		)
		VALUES
		(
			@CampaignItemID,@LTPID,@LTPOID, @CustomValue
		)
		SET @CILTID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItemLinkTracking
			--SET CustomerID=@CustomerID,UserID=@UserID,CampaignName=@CampaignName
			SET CampaignItemID=@CampaignItemID,LTPID=@LTPID,LTPOID=@LTPOID,CustomValue=@CustomValue
		WHERE
			CILTID = @CILTID
	END

	SELECT @CILTID
END