CREATE PROCEDURE [dbo].[e_WaveMailing_Select]
AS
	SELECT * FROM WaveMailing With(NoLock)
