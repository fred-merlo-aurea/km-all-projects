CREATE PROCEDURE [dbo].[e_FilterSegmentation_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM FilterSegmentation With(NoLock)

END
