CREATE PROCEDURE e_TransactionCode_Save
@TransactionCodeID int,
@TransactionCodeName nvarchar(50),
@TransactionCodeTypeID int,
@TransactionCodeValue nchar(10)
AS
BEGIN

	set nocount on

	IF @TransactionCodeID > 0
		BEGIN
			UPDATE TransactionCode
			SET TransactionCodeTypeID = @TransactionCodeTypeID,
				TransactionCodeName = @TransactionCodeName,
				TransactionCodeValue = @TransactionCodeValue
			WHERE TransactionCodeID = @TransactionCodeID;

			SELECT @TransactionCodeID;
		END
	ELSE
		IF NOT EXISTS(SELECT TransactionCodeID FROM TransactionCode WHERE TransactionCodeTypeID = @TransactionCodeTypeID AND TransactionCodeValue = @TransactionCodeValue)
		BEGIN
			INSERT INTO TransactionCode (TransactionCodeTypeID,TransactionCodeName,TransactionCodeValue)
			VALUES(@TransactionCodeTypeID,@TransactionCodeName,@TransactionCodeValue);SELECT @@IDENTITY;
		END

END