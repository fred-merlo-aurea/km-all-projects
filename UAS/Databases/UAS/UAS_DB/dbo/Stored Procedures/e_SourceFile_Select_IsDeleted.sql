CREATE PROCEDURE e_SourceFile_Select_IsDeleted
@IsDeleted bit = 'false'
AS
BEGIN

	set nocount on

	SELECT *
	FROM SourceFile With(NoLock)
	WHERE IsDeleted = @IsDeleted

END