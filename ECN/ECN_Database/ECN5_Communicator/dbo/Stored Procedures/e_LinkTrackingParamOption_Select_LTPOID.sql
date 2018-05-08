CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_Select_LTPOID]
@LTPOID int
AS
	SELECT *
	FROM LinkTrackingParamOption WITH (NOLOCK)
	WHERE LTPOID = @LTPOID AND IsActive = 1 and IsDeleted = 0