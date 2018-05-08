CREATE PROCEDURE [dbo].[e_FileRuleMap_Save]
	@RulesAssignedID int,
	@RulesID int,
    @SourceFileID int,
	@RuleOrder int,
    @IsActive bit,
	@DateCreated datetime,
    @DateUpdated datetime,
    @CreatedByUserID int,
    @UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @RulesAssignedID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FileRuleMap
			SET RulesID = @RulesID,
				SourceFileID = @SourceFileID,
				@RuleOrder = @RuleOrder,
				IsActive = @IsActive,
				DateCreated = @DateCreated,
				DateUpdated = @DateUpdated,
				CreatedByUserID = @CreatedByUserID,
				UpdatedByUserID = @UpdatedByUserID
			WHERE RulesAssignedID = @RulesAssignedID;
		
			SELECT @RulesAssignedID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FileRuleMap (RulesID,SourceFileID,RuleOrder,IsActive,DateCreated,CreatedByUserID)
			VALUES(@RulesID,@SourceFileID,@RuleOrder,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END