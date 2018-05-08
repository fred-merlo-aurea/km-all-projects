CREATE proc [dbo].[rpt_BlastReport] (@blastID int)
as
Begin
DECLARE @FilterString varchar(1000)
SELECT @FilterString = rTrim(STUFF((SELECT Case WHEN x.SmartSegmentID is not null THEN x.SmartSegmentName + ' for blasts(' + x.refBlastIDs + '), '
									WHEN x.FilterID is not null THEN x.FilterName + ', ' END
					FROM
					(
					SELECT cibf.SmartSegmentID, cibf.FilterID, cibf.RefBlastIDs, ss.SmartSegmentName, f.FilterName fROM CampaignItemBlast cib with(nolock)
					LEFT OUTER JOIN CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID and cibf.IsDeleted = 0
					LEFT OUTER JOIN SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID
					LEFT OUTER JOIN Filter f with(nolock) on cibf.FilterID = f.FilterID
					WHERE cib.BlastID = @blastID) x for XML Path(''))
					, 1,0,''))
SELECT @FilterString = SUBSTRING(@FilterString, 1, len(@FilterString) - 1)

declare @SuppFilterString varchar(MAX)
declare @SuppGroup varchar(MAX)

select @SuppFilterString = rTrim(STUFF((SELECT Case WHEN x.SmartSegmentID is not null THEN x.SmartSegmentName + ' for blasts(' + x.refBlastIDs + '), '
									WHEN x.FilterID is not null THEN x.FilterName + ', ' END
					FROM
					(
					SELECT cibf.SmartSegmentID, cibf.FilterID, cibf.RefBlastIDs, ss.SmartSegmentName, f.FilterName 
					fROM CampaignItemSuppression cis with(nolock)
						join CampaignItemBlast cib with(nolock) on cis.CampaignItemID = cib.CampaignItemID
						LEFT OUTER JOIN CampaignItemBlastFilter cibf with(nolock) on cis.CampaignItemSuppressionID = cibf.CampaignItemSuppressionID and cibf.IsDeleted = 0
						LEFT OUTER JOIN SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID
						LEFT OUTER JOIN Filter f with(nolock) on cibf.FilterID = f.FilterID
					WHERE cib.BlastID = @blastID and cis.IsDeleted = 0) x for XML Path(''))
					, 1,0,''))

SELECT @SuppFilterString = SUBSTRING(@SuppFilterString, 1, len(@SuppFilterString) - 1)

select @SuppGroup = rTrim(STUFF((SELECT x.GroupName + ', '
					FROM
					(
					SELECT g.GroupName
					fROM CampaignItemSuppression cis with(nolock)
						join CampaignItemBlast cib with(nolock) on cis.CampaignItemID = cib.CampaignItemID
						join Groups g with(nolock) on cis.GroupID = g.GroupID
					WHERE cib.BlastID = @blastID and cis.IsDeleted = 0) x for XML Path(''))
					, 1,0,''))
					
select @SuppGroup = SUBSTRING(@SuppGroup, 1, len(@SuppGroup) - 1)


SELECT 
	b.BlastID, b.EmailSubject, b.EmailFromName,  b.EmailFrom,
	CASE WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN g.GroupName ELSE '< GROUP DELETED >' END as 'GroupName' , 
	CASE WHEN LEN(ISNULL(@FilterString,'')) = 0 THEN '' ELSE @FilterString END as 'FilterName', 
	CASE WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName ELSE '< CAMPAIGN DELETED >' END as 'LayoutName', 
	CASE WHEN LEN(ISNULL(@SuppFilterString,'')) = 0 then '' ELSE @SuppFilterString END as 'SuppressionGroupFilters',
	CASE WHEN LEN(@SuppGroup) = 0 THEN '< NO SUPPRESSION GROUP >' ELSE @SuppGroup END as 'SuppressionGroups',
	b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal,
	l.SetupCost, l.OutboundCost, l.InboundCost, l.DesignCost, l.OtherCost 
	
FROM 
		[BLAST] b WITH (NOLOCK) LEFT OUTER JOIN 
		Groups g WITH (NOLOCK) ON b.groupID = g.groupID LEFT OUTER JOIN 
		[LAYOUT] l WITH (NOLOCK) on b.LayoutID = l.LayoutID
WHERE b.BlastID = @BlastID

end