CREATE  PROC [dbo].[e_DynamicTag_Save] 
(
	@DynamicTagID int = NULL,
    @Tag varchar(50) = NULL,
    @CustomerID int = NULL,
    @ContentID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @DynamicTagID = NULL or @DynamicTagID <= 0
	BEGIN
		INSERT INTO DynamicTag
		(
			Tag, CustomerID, ContentID, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@Tag, @CustomerID, @ContentID, @UserID, GetDate(), 0
		)
		SET @DynamicTagID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE DynamicTag
			SET Tag=@Tag, CustomerID=@CustomerID, ContentID=@ContentID, UpdatedUserID=@UserID, UpdatedDate=GETDATE()
		WHERE
			DynamicTagID = @DynamicTagID
	END

	SELECT @DynamicTagID
END