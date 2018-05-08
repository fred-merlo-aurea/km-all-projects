CREATE PROCEDURE [dbo].[e_FilterCategory_Select]	
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	from FilterCategory With(NoLock)

END