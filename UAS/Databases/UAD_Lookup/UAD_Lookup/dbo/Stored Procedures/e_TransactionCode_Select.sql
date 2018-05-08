CREATE PROCEDURE e_TransactionCode_Select
AS
BEGIN

	set nocount on

	SELECT * FROM TransactionCode With(NoLock)

END