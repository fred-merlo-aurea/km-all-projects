CREATE PROCEDURE [dbo].[e_LinkTracking_Select_All]
AS
	SELECT *
	FROM LinkTracking WITH (NOLOCK)
	WHERE IsActive = 1