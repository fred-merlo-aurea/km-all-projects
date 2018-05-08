
CREATE PROCEDURE e_Action_Save
@ActionID int,
@ActionTypeID int,
@CategoryCodeID int,
@TransactionCodeID int,
@Note nvarchar(max)
AS
IF @ActionID > 0
	BEGIN
		UPDATE Action
		SET ActionTypeID = @ActionTypeID,
			CategoryCodeID = @CategoryCodeID,
			TransactionCodeID = @TransactionCodeID,
			Note = @Note
		WHERE ActionID = @ActionID;

		SELECT @ActionID;
	END
ELSE
	BEGIN
		INSERT INTO Action (ActionTypeID,CategoryCodeID,TransactionCodeID,Note)
		VALUES(@ActionTypeID,@CategoryCodeID,@TransactionCodeID,@Note);SELECT @@IDENTITY;
	END
