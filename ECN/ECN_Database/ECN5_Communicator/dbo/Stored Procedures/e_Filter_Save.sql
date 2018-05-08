CREATE  PROC [dbo].[e_Filter_Save] 
(
	@FilterID int = NULL,
	@CustomerID int = NULL,
	@GroupID int = NULL,
	@FilterName varchar(50) = NULL,
	@GroupCompareType varchar(50) = NULL,
	@UserID int = null,
	@Archived bit = null
)
AS 
BEGIN
	IF @FilterID is NULL or @FilterID <= 0
	BEGIN
		INSERT INTO Filter
		(
			CustomerID,GroupID,FilterName,CreatedUserID,CreatedDate,IsDeleted, GroupCompareType,Archived
		)
		VALUES
		(
			@CustomerID,@GroupID,@FilterName,@UserID,GetDate(),0,@GroupCompareType,@Archived
		)
		SET @FilterID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Filter
			SET CustomerID=@CustomerID,GroupID=@GroupID,FilterName=@FilterName,
				UpdatedUserID=@UserID,UpdatedDate=GETDATE(), GroupCompareType=@GroupCompareType,Archived = @Archived
		WHERE
			FilterID = @FilterID
	END

	SELECT @FilterID
END