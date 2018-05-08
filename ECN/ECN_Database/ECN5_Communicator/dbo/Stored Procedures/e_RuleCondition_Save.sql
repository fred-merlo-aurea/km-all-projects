CREATE  PROC [dbo].[e_RuleCondition_Save] 
(
	@RuleConditionID int = NULL,
    @Field varchar(200) = NULL,
    @DataType varchar(200) = NULL,
    @Comparator varchar(50) = NULL,
    @Value varchar(2000) = NULL,
    @RuleID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @RuleConditionID = NULL or @RuleConditionID <= 0
	BEGIN
		INSERT INTO RuleCondition
		(
			Field, DataType, Comparator, Value, RuleID,  CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@Field, @DataType,  @Comparator, @Value, @RuleID, @UserID, GetDate(), 0
		)
		SET @RuleConditionID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE RuleCondition
			SET Field=@Field,DataType=@DataType, Comparator=@Comparator,Value=@Value,RuleID=@RuleID, UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			RuleConditionID = @RuleConditionID
	END

	SELECT @RuleConditionID
END