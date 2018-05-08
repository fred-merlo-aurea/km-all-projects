CREATE  PROC [dbo].[e_BlastPlans_Save] 
(
	@BlastPlanID int = NULL,
	@BlastID int = NULL,
	@CustomerID int = NULL,
	@GroupID int = NULL,
	@EventType varchar(50) = NULL,
	@Period float = NULL,
	@BlastDay int = NULL,
	@PlanType varchar(50) = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	IF @BlastPlanID is NULL or @BlastPlanID <= 0
	BEGIN
		INSERT INTO BlastPlans
		(
			BlastID,CustomerID,GroupID,EventType,Period,BlastDay,PlanType,CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@BlastID,@CustomerID,@GroupID,@EventType,@Period,@BlastDay,@PlanType,@UserID,GetDate(),0
		)
		SET @BlastPlanID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE BlastPlans
			SET BlastID=@BlastID,CustomerID=@CustomerID,GroupID=@GroupID,EventType=@EventType,
			Period=@Period,BlastDay=@BlastDay,PlanType=@PlanType,UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			BlastPlanID = @BlastPlanID
	END

	SELECT @BlastPlanID
END