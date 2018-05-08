CREATE PROCEDURE e_SourceFile_Select_IsDeleted
@IsDeleted bit = 'false'
AS
	SELECT *
	FROM SourceFile With(NoLock)
	WHERE IsDeleted = @IsDeleted
