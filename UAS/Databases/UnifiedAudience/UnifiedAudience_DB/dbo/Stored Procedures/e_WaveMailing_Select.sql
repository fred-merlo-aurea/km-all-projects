CREATE PROCEDURE [dbo].[e_WaveMailing_Select]
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT * 
	FROM WaveMailing With(NoLock)

END