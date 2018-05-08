CREATE PROCEDURE [dbo].[v_CampaignItemLinkTracking_GetParamInfo]   
@BlastID int,
@LTID int
AS
--declare @BlastID int
--set @BlastID = 1568963
	SELECT cilt.*, ltp.DisplayName, case when ltpo.ColumnName is null then ltp.DisplayName else ltpo.ColumnName end as ColumnName, lt.LTID, lt.DisplayName
	FROM 
		CampaignItemLinkTracking cilt WITH (NOLOCK)
		JOIN LinkTrackingParam ltp WITH (NOLOCK) ON cilt.LTPID = ltp.LTPID
		JOIN LinkTracking lt WITH (NOLOCK) ON ltp.LTID = lt.LTID
		LEFT OUTER JOIN LinkTrackingParamOption ltpo WITH (NOLOCK) ON cilt.LTPOID = ltpo.LTPOID
		LEFT OUTER JOIN CampaignItemBlast cib WITH (NOLOCK) ON cilt.CampaignItemID = cib.CampaignItemID and cib.BlastID = @BlastID
		LEFT OUTER JOIN CampaignItemTestBlast citb WITH (NOLOCK) ON cilt.CampaignItemID = citb.CampaignItemID and citb.BlastID = @BlastID
	WHERE
		(cib.BlastID = @BlastID or citb.BlastID = @BlastID)
		and lt.LTID=@LTID and lt.IsActive=1