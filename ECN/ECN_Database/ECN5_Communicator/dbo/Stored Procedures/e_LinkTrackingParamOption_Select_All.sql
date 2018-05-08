CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_Select_All]

AS
	SELECT *
	FROM LinkTrackingParamOption WITH (NOLOCK)
	WHERE IsActive = 1 and IsDeleted = 0