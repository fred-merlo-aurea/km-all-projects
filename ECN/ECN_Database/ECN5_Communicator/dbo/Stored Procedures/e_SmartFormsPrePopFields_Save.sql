CREATE  PROC [dbo].[e_SmartFormsPrePopFields_Save] 
(
	@PrePopFieldID int = NULL,
	@SFID int = NULL,
	@ProfileFieldName varchar(50) = NULL,
    @DisplayName varchar(255) = NULL,
    @DataType varchar(50) = NULL,
    @ControlType varchar(50) = NULL,
    @DataValues varchar(500) = NULL,
    @Required varchar(1) = NULL,
    @PrePopulate varchar(1) = NULL,
    @SortOrder int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @PrePopFieldID is NULL or @PrePopFieldID <= 0
	BEGIN
		INSERT INTO SmartFormsPrePopFields
		(
			SFID, ProfileFieldName, DisplayName, DataType, ControlType, DataValues, [Required], PrePopulate, 
			SortOrder, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@SFID, @ProfileFieldName, @DisplayName, @DataType, @ControlType, @DataValues, @Required, @PrePopulate, 
			@SortOrder, @UserID, GetDate(), 0
		)
		SET @PrePopFieldID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE SmartFormsPrePopFields
			SET SFID=@SFID, ProfileFieldName=@ProfileFieldName, DisplayName=@DisplayName, DataType=@DataType, ControlType=@ControlType, 
				DataValues=@DataValues, [Required]=@Required, PrePopulate=@PrePopulate, SortOrder=@SortOrder,
				UpdatedUserID=@UserID, UpdatedDate=GETDATE()
		WHERE
			PrePopFieldID = @PrePopFieldID
	END

	SELECT @PrePopFieldID
END