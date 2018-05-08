CREATE PROCEDURE e_TransactionCode_Active_IsFree
@IsFree bit
AS
	SELECT tc.*
	FROM TransactionCode tc With(NoLock)
	JOIN TransactionCodeType t With(NoLock) ON tc.TransactionCodeTypeID = t.TransactionCodeTypeID
	WHERE t.IsActive = 'true' AND t.IsFree = @IsFree
	AND tc.IsActive = 'true'