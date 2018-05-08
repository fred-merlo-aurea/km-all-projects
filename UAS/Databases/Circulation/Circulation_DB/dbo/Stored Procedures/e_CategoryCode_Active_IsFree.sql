CREATE PROCEDURE e_CategoryCode_Active_IsFree
@IsFree bit
AS
	SELECT cc.*
	FROM CategoryCode cc With(NoLock)
	JOIN CategoryCodeType t With(NoLock) ON cc.CategoryCodeTypeID = t.CategoryCodeTypeID
	WHERE t.IsActive = 'true' AND t.IsFree = @IsFree
	AND cc.IsActive = 'true'
