CREATE PROCEDURE [dbo].[e_LinkTracking_Check_Domain]   
@LTID int,
@Domain varchar(200),
@CustomerID int
AS

if exists(Select LinkTrackingDomainID from LinkTrackingDomain where LTID=@LTID and IsDeleted=0 and CustomerID=@CustomerID)
BEGIN	
	if exists(
				SELECT ltd.LinkTrackingDomainID 
				FROM 
					CampaignItemLinkTracking cilt WITH (NOLOCK)
					JOIN CampaignItem ci WITH (NOLOCK) on cilt.CampaignItemID = ci.CampaignItemID
					JOIN LinkTrackingParam ltp WITH (NOLOCK) ON ltp.LTPID = cilt.LTPID 
					JOIN LinkTracking lt WITH (NOLOCK) ON lt.LTID = lt.LTID 
					JOIN LinkTrackingDomain ltd WITH (NOLOCK) ON lt.LTID = ltd.LTID 
				WHERE ci.IsDeleted = 0 and 
					  lt.IsActive=1 and
					  ltd.Domain=@Domain and
					  lt.LTID=@LTID and
					  ltd.IsDeleted=0 and 
					  CustomerID=@CustomerID
	    )
	    BEGIN	 
	       SELECT 1
	    END
	ELSE
		BEGIN
			Select 0
	    END
END
ELSE
BEGIN
	Select 1
END