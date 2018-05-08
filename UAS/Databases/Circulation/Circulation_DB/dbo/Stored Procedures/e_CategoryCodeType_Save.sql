CREATE PROCEDURE e_CategoryCodeType_Save
@CategoryCodeTypeID int,
@CategoryCodeTypeName nvarchar(50)
AS
IF @CategoryCodeTypeID > 0
	BEGIN
		UPDATE CategoryCodeType
		SET CategoryCodeTypeName = @CategoryCodeTypeName
		WHERE CategoryCodeTypeID = @CategoryCodeTypeID;

		SELECT @CategoryCodeTypeID;
	END
ELSE
	BEGIN
		IF NOT EXISTS (Select CategoryCodeTypeID FROM CategoryCodeType WHERE CategoryCodeTypeName = @CategoryCodeTypeName)
			BEGIN
				INSERT INTO CategoryCodeType (CategoryCodeTypeName)
				VALUES(@CategoryCodeTypeName);SELECT @@IDENTITY;
			END
	END
