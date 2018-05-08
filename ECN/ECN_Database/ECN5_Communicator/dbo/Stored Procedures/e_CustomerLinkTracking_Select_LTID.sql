CREATE PROCEDURE [dbo].[e_CustomerLinkTracking_Select_LTID]   
@LTID int
AS
	SELECT clt.*
	FROM 
		CustomerLinkTracking clt WITH (NOLOCK)
		JOIN LinkTrackingParam ltp WITH (NOLOCK) ON clt.LTPID = ltp.LTPID
		JOIN LinkTracking lt WITH (NOLOCK) ON ltp.LTID = lt.LTID
	WHERE 
		lt.LTID = @LTID and 
		clt.IsActive = 1 and
		ltp.IsActive = 1 and
		lt.IsActive = 1