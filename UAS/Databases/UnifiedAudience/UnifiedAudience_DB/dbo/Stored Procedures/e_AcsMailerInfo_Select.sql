CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Select]
AS
BEGIN

	set nocount on

	SELECT * FROM AcsMailerInfo With(NoLock)

END
GO