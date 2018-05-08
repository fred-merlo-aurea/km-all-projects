CREATE  PROC [dbo].[e_DynamicTagRule_Save] 
(
	@DynamicTagRuleID int = NULL,
    @DynamicTagID int = NULL,
    @RuleID int = NULL,
    @ContentID int = NULL,
    @Priority int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @DynamicTagRuleID = NULL or @DynamicTagRuleID <= 0
	BEGIN
		INSERT INTO DynamicTagRule
		(
			DynamicTagID, RuleID, ContentID, Priority,  CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@DynamicTagID, @RuleID, @ContentID, @Priority, @UserID, GetDate(), 0
		)
		SET @DynamicTagRuleID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE DynamicTagRule
			SET DynamicTagID=@DynamicTagID,RuleID=@RuleID,ContentID=@ContentID,Priority=@Priority, UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			DynamicTagRuleID = @DynamicTagRuleID
	END

	SELECT @DynamicTagRuleID
END