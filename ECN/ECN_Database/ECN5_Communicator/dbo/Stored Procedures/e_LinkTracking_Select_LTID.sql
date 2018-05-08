CREATE PROCEDURE [dbo].[e_LinkTracking_Select_LTID]   
@LTID int
AS
	SELECT *
	FROM LinkTracking WITH (NOLOCK)
	WHERE LTID = @LTID and IsActive = 1