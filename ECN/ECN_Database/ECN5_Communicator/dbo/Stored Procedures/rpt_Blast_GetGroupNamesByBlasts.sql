CREATE PROCEDURE [dbo].[rpt_Blast_GetGroupNamesByBlasts]  
	@CampaignItemID int,
	@CustomerID int
	--set @CustomerID = 1
	--select * from CampaignItemBlast where CampaignItemID = 11479
	--set @CampaignItemID = 11479
AS
BEGIN		
	--select '&nbsp;<a href=../lists/groupeditor.aspx?GroupID='+Convert(varchar,g.groupID)+'><img src=icon-smartForm.gif border=0/></a>&nbsp;<a href=reports.aspx?BlastID='+Convert(varchar,cib.BlastID)+'><img src=icon-reports.gif border=0/></a>&nbsp;<a href=rpt_blastreport.aspx?BlastID='+Convert(varchar,cib.BlastID)+'><img  src=icon-pdf.gif border=0/></a>' + COALESCE(g.groupName+'<BR>','') +'&nbsp;&nbsp;&nbsp;&nbsp;' as 'Groups' 
	--from 
	--	Campaign c with (nolock)
	--	join CampaignItem ci with (nolock) on c.CampaignID = ci.CampaignID and ci.IsDeleted = 0
	--	join CampaignItemBlast cib with (nolock) on ci.CampaignItemID = cib.CampaignItemID and cib.IsDeleted = 0 and IsNull(cib.BlastID, 0) != 0
	--	join Groups g with (nolock) on cib.GroupID = g.GroupID and g.CustomerID = @CustomerID
	--where
	--	ci.CampaignItemID = @CampaignItemID and
	--	c.IsDeleted = 0 and
	--	c.CustomerID = @CustomerID
SELECT '&nbsp;<a href=/ecn.communicator.mvc/Subscriber/Index/'+Convert(varchar,g.groupID)+'>
		<img src=icon-smartForm.gif border=0/></a>&nbsp;<a href=reports.aspx?BlastID='+Convert(varchar,b.BlastID)+'>
		<img src=icon-reports.gif border=0/></a>&nbsp;<a href=rpt_blastreport.aspx?BlastID='+Convert(varchar,b.BlastID)+'>
		<img  src=icon-pdf.gif border=0/>
		</a>' + g.groupName 
		+	case when SUM(ISNULL(cibf.SmartSegmentID,0)) = 0 AND SUM(ISNULL(cibf.FilterID,0)) = 0 
				then '' else  STUFF((SELECT case when se2.SmartSegmentID is not null 
													then '  Smart - ' + ss2.SmartSegmentName + ' ' + se2.RefBlastIDs + '<BR>'
												 when se2.FilterID is not null 
													then  '  ' + f2.FilterName + '<br />'
												end
									FROM CampaignItemBlastFilter se2
									left outer Join SmartSegment ss2 on se2.SmartSegmentID = ss2.SmartSegmentID
									left outer join Filter f2 on se2.FilterID = f2.FilterID and ISNULL(f2.IsDeleted,0) = 0
									WHERE cibf.CampaignItemBlastID = se2.CampaignItemBlastID and ISNULL(se2.IsDeleted,0) = 0
									FOR XML PATH(''),root('MyString'),type).value('/MyString[1]','varchar(max)'),1,1,'') END + '<BR>&nbsp;&nbsp;&nbsp;&nbsp;' as 'Groups'
												
												
		,'Layout' = CASE WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN '<a href=../content/layouteditor.aspx?LayoutID='+CONVERT(VARCHAR,l.LayoutID)+'>'+l.LayoutName+'</a>'
						ELSE '< CAMPAIGN DELETED >'
						END,  '<NO FILTER>' AS 'FilterName'
						,
		b.EmailSubject, b.EmailFromName, b.EmailFrom, MIN(b.SendTime) AS 'SendTime', MAX(b.FinishTime) AS 'FinishTime', SUM(b.SendTotal) AS 'SendTotal', l.LayoutID 
	FROM 
		[Blast] b WITH (NOLOCK) 
		JOIN CampaignItemBlast cib WITH (NOLOCK) ON b.BlastID = cib.BlastID and cib.IsDeleted = 0
		left outer join CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID and ISNULL(cibf.IsDeleted,0) = 0
		left outer join SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID
		LEFT OUTER JOIN [Groups] g WITH (NOLOCK) ON b.groupID = g.groupID 
		LEFT OUTER JOIN [Filter] f WITH (NOLOCK)  ON cibf.filterID = f.filterID AND f.IsDeleted = 0
		LEFT OUTER JOIN [Layout] l WITH (NOLOCK) ON b.LayoutID = l.LayoutID AND l.IsDeleted = 0
	WHERE
		cib.CampaignItemID = @CampaignItemID AND
		b.CustomerID = @CustomerID AND
		b.StatusCode <> 'Deleted' and b.TestBlast = 'N'
	GROUP BY 		
		l.LayoutName, l.LayoutID, b.EmailSubject, b.EmailFromName, b.EmailFrom, g.GroupName, g.GroupID, b.BlastID,cibf.CampaignItemBlastID
END
