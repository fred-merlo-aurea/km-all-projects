CREATE PROCEDURE e_SourceFile_Select_ClientID_IsDeleted
@ClientID int,
@IsDeleted bit = 'false'
AS
	SELECT *
	FROM SourceFile With(NoLock)
	WHERE ClientID = @ClientID AND IsDeleted = @IsDeleted
