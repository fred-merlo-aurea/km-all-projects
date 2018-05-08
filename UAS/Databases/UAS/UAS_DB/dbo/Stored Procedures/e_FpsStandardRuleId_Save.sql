CREATE PROCEDURE [dbo].[e_FpsStandardRule_Save]
	@FpsStandardRuleId INT,
    @RuleName VARCHAR(50),
	@DisplayName VARCHAR(100),
	@Description VARCHAR(500),
    @RuleMethod VARCHAR(50),
	@ProcedureTypeId INT,
	@ExecutionPointId INT,
	@IsActive BIT,
    @DateCreated DATETIME,
    @DateUpdated DATETIME,
    @CreatedByUserID INT,
    @UpdatedByUserID INT
AS
BEGIN

	set nocount on

	IF @FpsStandardRuleId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FpsStandardRule
			SET RuleName = @RuleName,
				DisplayName = @DisplayName,
				Description = @Description,
				RuleMethod = @RuleMethod,
				ProcedureTypeId = @ProcedureTypeId,
				ExecutionPointId = @ExecutionPointId,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FpsStandardRuleId = @FpsStandardRuleId;
		
			SELECT @FpsStandardRuleId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FileRule (RuleName,DisplayName,Description,RuleMethod,ProcedureTypeId,ExecutionPointId,IsActive,DateCreated,CreatedByUserID)
			VALUES(@RuleName,@DisplayName,@Description,@RuleMethod,@ProcedureTypeId,@ExecutionPointId,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END	

END