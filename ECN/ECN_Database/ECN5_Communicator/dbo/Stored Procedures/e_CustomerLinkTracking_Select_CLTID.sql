CREATE PROCEDURE [dbo].[e_CustomerLinkTracking_Select_CLTID]   
@CLTID int
AS
	SELECT *
	FROM CustomerLinkTracking WITH (NOLOCK)
	WHERE CLTID = @CLTID and IsActive = 1