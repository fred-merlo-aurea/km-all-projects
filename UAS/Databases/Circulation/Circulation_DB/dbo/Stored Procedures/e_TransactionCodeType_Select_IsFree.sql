CREATE PROCEDURE e_TransactionCodeType_Select_IsFree
@IsFree bit
AS
	SELECT *
	FROM TransactionCodeType With(NoLock)
	WHERE IsFree = @IsFree
