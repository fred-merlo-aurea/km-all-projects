CREATE PROCEDURE e_TransactionCodeType_Select_IsFree
@IsFree bit
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransactionCodeType With(NoLock)
	WHERE IsFree = @IsFree

END