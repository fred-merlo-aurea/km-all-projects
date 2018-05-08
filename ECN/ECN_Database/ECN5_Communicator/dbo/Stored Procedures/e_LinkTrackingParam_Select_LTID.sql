CREATE PROCEDURE [dbo].[e_LinkTrackingParam_Select_LTID]
@LTID int
AS
	SELECT *
	FROM LinkTrackingParam WITH (NOLOCK)
	WHERE LTID = @LTID AND IsActive = 1