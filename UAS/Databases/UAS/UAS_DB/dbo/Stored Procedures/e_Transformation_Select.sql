CREATE PROCEDURE [dbo].[e_Transformation_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM Transformation With(NoLock)
	WHERE IsActive = 'true'

END