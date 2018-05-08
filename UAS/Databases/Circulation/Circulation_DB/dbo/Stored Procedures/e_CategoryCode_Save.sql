
CREATE PROCEDURE e_CategoryCode_Save
@CategoryCodeID int,
@CategoryCodeName nvarchar(50),
@CategoryCodeTypeID int,
@CategoryCodeValue nchar(10)
AS
IF @CategoryCodeID > 0
	BEGIN
		UPDATE CategoryCode
		SET CategoryCodeTypeID = @CategoryCodeTypeID,
			CategoryCodeName = @CategoryCodeName,
			CategoryCodeValue = @CategoryCodeValue
		WHERE CategoryCodeID = @CategoryCodeID;

		SELECT @CategoryCodeID;
	END
ELSE
	IF NOT EXISTS(SELECT CategoryCodeID FROM CategoryCode WHERE CategoryCodeTypeID = @CategoryCodeTypeID AND CategoryCodeValue = @CategoryCodeValue)
	BEGIN
		INSERT INTO CategoryCode (CategoryCodeTypeID,CategoryCodeName,CategoryCodeValue)
		VALUES(@CategoryCodeTypeID,@CategoryCodeName,@CategoryCodeValue);SELECT @@IDENTITY;
	END
