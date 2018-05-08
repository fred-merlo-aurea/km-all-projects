CREATE  PROC [dbo].[e_LayoutPlans_Save] 
(
	@LayoutPlanID int = NULL,
	@LayoutID int = NULL,
	@EventType varchar(10) = NULL,
	@BlastID int = NULL,
	@Period float = NULL,
	@Criteria varchar(255) = NULL,
	@CustomerID int = NULL,
	@ActionName varchar(50) = NULL,
	@GroupID int = NULL,
	@Status char(1) = NULL,
	@SmartFormID int = NULL,
	@UserID int = NULL,
	@CampaignItemID int = null,
    @TokenUID uniqueidentifier =null
)
AS 
BEGIN
	IF @LayoutPlanID is NULL or @LayoutPlanID <= 0
	BEGIN
		INSERT INTO LayoutPlans
		(
			LayoutID, EventType, BlastID, Period, Criteria, CustomerID, ActionName, GroupID, [Status], SmartFormID, CreatedDate, CreatedUserID, IsDeleted, CampaignItemID,TokenUID
		)
		VALUES
		(
			@LayoutID, @EventType, @BlastID, @Period, @Criteria, @CustomerID, @ActionName, @GroupID, @Status, @SmartFormID, GETDATE(), @UserID, 0, @CampaignItemID,@TokenUID
		)
		SET @LayoutPlanID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE LayoutPlans
			SET LayoutID=@LayoutID, EventType=@EventType, BlastID=@BlastID, Period=@Period, 
			Criteria=@Criteria, CustomerID=@CustomerID, ActionName=@ActionName, GroupID=@GroupID, 
			[Status]=@Status, SmartFormID=@SmartFormID, UpdatedDate=GETDATE(), UpdatedUserID=@UserID,
			CampaignItemID = @CampaignItemID,TokenUID =@TokenUID
		WHERE
			LayoutPlanID = @LayoutPlanID
	END

	SELECT @LayoutPlanID
END