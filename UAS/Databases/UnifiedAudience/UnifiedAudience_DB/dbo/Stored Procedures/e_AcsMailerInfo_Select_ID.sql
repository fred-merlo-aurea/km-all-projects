CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Select_ID]
@AcsMailerInfoId int
AS
BEGIN

	set nocount on

	SELECT *
	FROM AcsMailerInfo With(NoLock)
	WHERE AcsMailerInfoId = @AcsMailerInfoId

END