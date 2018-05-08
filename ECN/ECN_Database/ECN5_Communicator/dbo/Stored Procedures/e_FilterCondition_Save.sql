CREATE  PROC [dbo].[e_FilterCondition_Save] 
(
	@FilterConditionID int = NULL,
	@FilterGroupID int = NULL,
	@SortOrder int = NULL,
	@Field varchar(100) = NULL,
	@FieldType varchar(50) = NULL,
	@Comparator varchar(100) = NULL,
	@CompareValue varchar(500) = NULL,
	@NotComparator int = NULL,
	@DatePart varchar(10) = NULL,
	@UserID int = null
)
AS 
BEGIN
	IF @FilterConditionID is NULL or @FilterConditionID <= 0
	BEGIN
		INSERT INTO FilterCondition
		(
			FilterGroupID,SortOrder,Field,FieldType,Comparator,CompareValue,NotComparator,[DatePart],CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@FilterGroupID,@SortOrder,@Field,@FieldType,@Comparator,@CompareValue,@NotComparator,@DatePart,@UserID,GetDate(),0
		)
		SET @FilterConditionID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE FilterCondition
			SET FilterGroupID=@FilterGroupID,SortOrder=@SortOrder,Field=@Field,FieldType=@FieldType,
				Comparator=@Comparator,CompareValue=@CompareValue,NotComparator=@NotComparator,[DatePart]=@DatePart,
				UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			FilterConditionID = @FilterConditionID
	END

	SELECT @FilterConditionID
END