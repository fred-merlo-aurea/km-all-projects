CREATE  PROC [dbo].[e_TriggerPlans_Save] 
(
	@TriggerPlanID int = NULL,
	@RefTriggerID int = NULL,
	@EventType varchar(10) = NULL,
	@BlastID int = NULL,
	@Period float = NULL,
	@Criteria varchar(50) = NULL,
	@CustomerID int = NULL,
	@ActionName varchar(50) = NULL,
	@GroupID int = NULL,
	@Status char(1) = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	IF @TriggerPlanID is NULL or @TriggerPlanID <= 0
	BEGIN
		INSERT INTO TriggerPlans
		(
			RefTriggerID, EventType, BlastID, Period, Criteria, CustomerID, ActionName, GroupID, [Status], CreatedDate, CreatedUserID, IsDeleted
		)
		VALUES
		(
			@RefTriggerID, @EventType, @BlastID, @Period, @Criteria, @CustomerID, @ActionName, @GroupID, @Status, GETDATE(), @UserID, 0
		)
		SET @TriggerPlanID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE TriggerPlans
			SET RefTriggerID=@RefTriggerID, EventType=@EventType, BlastID=@BlastID, Period=@Period, 
			Criteria=@Criteria, CustomerID=@CustomerID, ActionName=@ActionName, GroupID=@GroupID, 
			[Status]=@Status, UpdatedDate=GETDATE(), UpdatedUserID=@UserID
		WHERE
			TriggerPlanID = @TriggerPlanID
	END

	SELECT @TriggerPlanID
END