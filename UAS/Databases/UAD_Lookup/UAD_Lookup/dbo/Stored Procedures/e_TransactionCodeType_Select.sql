
CREATE PROCEDURE e_TransactionCodeType_Select
AS
BEGIN

	set nocount on

	SELECT * FROM TransactionCodeType With(NoLock)

END