CREATE PROCEDURE [dbo].[e_EmailStatus_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM EmailStatus With(NoLock)

END
GO