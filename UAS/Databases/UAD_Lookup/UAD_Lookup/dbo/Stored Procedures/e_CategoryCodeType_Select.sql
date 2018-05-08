CREATE PROCEDURE e_CategoryCodeType_Select
AS

BEGIN

	set nocount on

	SELECT * FROM CategoryCodeType With(NoLock)

END