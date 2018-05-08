CREATE PROCEDURE e_CategoryCodeType_Select_IsFree
@IsFree bit
AS
BEGIN

	set nocount on

	SELECT *
	FROM CategoryCodeType With(NoLock)
	WHERE IsFree = @IsFree

END