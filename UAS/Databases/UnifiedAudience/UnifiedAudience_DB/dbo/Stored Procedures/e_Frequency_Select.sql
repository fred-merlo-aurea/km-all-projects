CREATE PROCEDURE [dbo].[e_Frequency_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM Frequency With(NoLock)

END