CREATE  PROC [dbo].[e_Rule_Save] 
(
	@RuleID int = NULL,
    @ConditionConnector varchar(200) = NULL,
    @CustomerID int = NULL,
    @WhereClause varchar(8000) = NULL,
    @RuleName varchar(200) = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @RuleID = NULL or @RuleID <= 0
	BEGIN
		INSERT INTO [Rule]
		(
			ConditionConnector, CustomerID, WhereClause, RuleName,  CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@ConditionConnector, @CustomerID, @WhereClause, @RuleName, @UserID, GetDate(), 0
		)
		SET @RuleID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [Rule]
			SET ConditionConnector=@ConditionConnector,CustomerID=@CustomerID,WhereClause=@WhereClause,RuleName=@RuleName, UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			RuleID = @RuleID
	END

	SELECT @RuleID
END