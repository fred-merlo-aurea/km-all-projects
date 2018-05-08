CREATE PROCEDURE [dbo].[e_AcsMailerInfo_Select_ID]
@AcsMailerInfoId int
AS
	SELECT *
	FROM AcsMailerInfo With(NoLock)
	WHERE AcsMailerInfoId = @AcsMailerInfoId
