CREATE  PROC [dbo].[e_ContentFilterDetail_Save] 
(
	@FDID int = NULL,
	@FilterID int = NULL,
	@FieldType varchar(50) = NULL,
	@CompareType varchar(50) = NULL,
	@UserID int = NULL,
	@FieldName varchar(100) = NULL,
	@Comparator varchar(100) = NULL,
	@CompareValue varchar(100) = NULL
)
AS 
BEGIN
	IF @FDID is NULL or @FDID <= 0
	BEGIN
		INSERT INTO ContentFilterDetail
		(
			FilterID, FieldType, CreatedUserID, CreatedDate, CompareType, FieldName, Comparator, CompareValue,IsDeleted
		)
		VALUES
		(
			@FilterID, @FieldType, @UserID, GETDATE(), @CompareType, @FieldName, @Comparator, @CompareValue, 0
		)
		SET @FDID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE ContentFilterDetail
			SET FilterID=@FilterID, FieldType=@FieldType, UpdatedUserID=@UserID,UpdatedDate=GETDATE(), CompareType=@CompareType, 
				FieldName=@FieldName, Comparator=@Comparator, CompareValue=@CompareValue
		WHERE
			FDID = @FDID
	END

	SELECT @FDID
END