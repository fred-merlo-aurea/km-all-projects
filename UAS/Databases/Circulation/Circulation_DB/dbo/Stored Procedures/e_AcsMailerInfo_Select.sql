CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Select]
AS
	SELECT * FROM AcsMailerInfo With(NoLock)
GO