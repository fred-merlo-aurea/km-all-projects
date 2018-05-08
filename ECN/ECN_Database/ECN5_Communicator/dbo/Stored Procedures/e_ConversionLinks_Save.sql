CREATE  PROC [dbo].[e_ConversionLinks_Save] 
(
	@LinkID int = NULL,
	@LayoutID int = NULL,
	@LinkURL varchar(255) = NULL,
	@LinkParams varchar(255) = NULL,
	@LinkName varchar(255) = NULL,
	@IsActive varchar(1) = NULL,
	@SortOrder int = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	IF @LinkID is NULL or @LinkID <= 0
	BEGIN
		INSERT INTO ConversionLinks
		(
			LayoutID, LinkURL, LinkParams, LinkName, IsActive, SortOrder, CreatedDate, CreatedUserID, IsDeleted
		)
		VALUES
		(
			@LayoutID, @LinkURL, @LinkParams, @LinkName, @IsActive, @SortOrder, GETDATE(), @UserID, 0
		)
		SET @LinkID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE ConversionLinks
			SET LayoutID=@LayoutID, LinkURL=@LinkURL,LinkParams=@LinkParams, LinkName=@LinkName, 
				IsActive=@IsActive, SortOrder=@SortOrder, UpdatedDate=GETDATE(), UpdatedUserID=@UserID
		WHERE
			LinkID = @LinkID
	END

	SELECT @LinkID
END