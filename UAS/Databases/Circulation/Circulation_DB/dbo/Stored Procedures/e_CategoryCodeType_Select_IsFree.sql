CREATE PROCEDURE e_CategoryCodeType_Select_IsFree
@IsFree bit
AS
	SELECT *
	FROM CategoryCodeType With(NoLock)
	WHERE IsFree = @IsFree
