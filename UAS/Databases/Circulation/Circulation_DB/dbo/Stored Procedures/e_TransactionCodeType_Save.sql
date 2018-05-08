CREATE PROCEDURE e_TransactionCodeType_Save
@TransactionCodeTypeID int,
@TransactionCodeTypeName nvarchar(50)
AS
IF @TransactionCodeTypeID > 0
	BEGIN
		UPDATE TransactionCodeType
		SET TransactionCodeTypeName = @TransactionCodeTypeName
		WHERE TransactionCodeTypeID = @TransactionCodeTypeID;

		SELECT @TransactionCodeTypeID;
	END
ELSE
	BEGIN
		IF NOT EXISTS (Select TransactionCodeTypeID FROM TransactionCodeType WHERE TransactionCodeTypeName = @TransactionCodeTypeName)
			BEGIN
				INSERT INTO TransactionCodeType (TransactionCodeTypeName)
				VALUES(@TransactionCodeTypeName);SELECT @@IDENTITY;
			END
	END
