CREATE  PROC [dbo].[e_Code_Save] 
(
	@CodeID int = NULL,
    @CustomerID int = NULL,
    @CodeType varchar(50) = NULL,
    @CodeValue varchar(50) = NULL,
    @CodeDisplay varchar(250) = NULL,
    @SortOrder int = NULL,
    @DisplayFlag varchar(1) = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @CodeID is NULL or @CodeID <= 0
	BEGIN
		INSERT INTO [Code]
		(
			CustomerID, CodeType, CodeValue, CodeDisplay, SortOrder, DisplayFlag, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@CustomerID, @CodeType, @CodeValue, @CodeDisplay, @SortOrder, @DisplayFlag, @UserID, GetDate(), 0
		)
		SET @CodeID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [Code]
			SET CustomerID=@CustomerID,CodeType=@CodeType,CodeValue=@CodeValue,CodeDisplay=@CodeDisplay,SortOrder=@SortOrder,DisplayFlag=@DisplayFlag,UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			CodeID = @CodeID
	END

	SELECT @CodeID
END