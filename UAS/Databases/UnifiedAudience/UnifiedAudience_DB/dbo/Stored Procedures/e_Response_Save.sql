CREATE PROCEDURE e_Response_Save
@ResponseID int,
@ResponseGroupID int,
@PublicationID int,
@ResponseName varchar(250),
@ResponseCode nchar(10),
@DisplayName varchar(250),
@DisplayOrder int,
@ReportGroupID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@WQT_ResponseID int,
@IsOther bit
AS
BEGIN

	SET NOCOUNT ON

	IF @ResponseID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE Response
			SET ResponseGroupID = @ResponseGroupID,
				PublicationID = @PublicationID,
				ResponseName = @ResponseName,
				ResponseCode = @ResponseCode,
				DisplayName = @DisplayName,
				DisplayOrder = @DisplayOrder,
				ReportGroupID = @ReportGroupID,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				WQT_ResponseID = @WQT_ResponseID,
				IsOther = @IsOther
			WHERE ResponseID = @ResponseID;
		
			SELECT @ResponseID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Response (ResponseGroupID,PublicationID,ResponseName,ResponseCode,DisplayName,DisplayOrder,ReportGroupID,IsActive,DateCreated,CreatedByUserID,WQT_ResponseID,IsOther)
			VALUES(@ResponseGroupID,@PublicationID,@ResponseName,@ResponseCode,@DisplayName,@DisplayOrder,@ReportGroupID,@IsActive,@DateCreated,@CreatedByUserID,@WQT_ResponseID,@IsOther);SELECT @@IDENTITY;
		END

END