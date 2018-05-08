CREATE PROCEDURE [dbo].[e_SourceFile_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM SourceFile With(NoLock)

END