CREATE  PROC [dbo].[e_FilterGroup_Save] 
(
	@FilterGroupID int = NULL,
	@FilterID int = NULL,
	@SortOrder int = NULL,
	@Name varchar(50) = NULL,
	@ConditionCompareType varchar(50) = NULL,
	@UserID int = null
)
AS 
BEGIN
	IF @FilterGroupID is NULL or @FilterGroupID <= 0
	BEGIN
		INSERT INTO FilterGroup
		(
			FilterID,SortOrder,Name,ConditionCompareType,CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@FilterID,@SortOrder,@Name,@ConditionCompareType,@UserID,GetDate(),0
		)
		SET @FilterGroupID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE FilterGroup
			SET FilterID=@FilterID,SortOrder=@SortOrder,Name=@Name,ConditionCompareType=@ConditionCompareType,
				UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			FilterGroupID = @FilterGroupID
	END

	SELECT @FilterGroupID
END