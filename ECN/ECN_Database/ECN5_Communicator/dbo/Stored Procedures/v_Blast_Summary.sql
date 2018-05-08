CREATE proc [dbo].[v_Blast_Summary] 
(
	@blastID int
)
as
Begin
	Set nocount on
	 SELECT 
                 'GroupName' = CASE 
                 WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN g.GroupName 
                 ELSE '< GROUP DELETED >' 
                 END, 
                 'GrpNavigateURL' = CASE 
                 WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN '../lists/groupeditor.aspx?GroupID='+CONVERT(VARCHAR,g.GroupID) 
                 ELSE '' 
                 END, 
                 'FilterName' = CASE 
                 WHEN b.SmartSegmentID = 1 THEN 'UnClicked for BlastID (' + Convert(varchar,b.refblastID) + ')' 
                 WHEN b.SmartSegmentID = 3 THEN 'UnOpened for BlastID (' + Convert(varchar,b.refblastID) + ')' 
                 WHEN b.SmartSegmentID = 4 THEN 'Suppressed for BlastID (' + Convert(varchar,b.refblastID) + ')' 
                 WHEN b.SmartSegmentID = 5 THEN 'Opened for BlastID (' + Convert(varchar,b.refblastID) + ')' 
                 WHEN b.SmartSegmentID = 6 THEN 'Clicked for BlastID (' + Convert(varchar,b.refblastID) + ')' 
				 WHEN b.SmartSegmentID = 7 THEN 'Sent for BlastID (' + Convert(varchar, b.refBlastID) + ')'
				 WHEN b.SmartSegmentID = 8 THEN 'Not Sent for BlastID (' + Convert(varchar, b.refBlastID) + ')'
                 WHEN isnull(b.FilterID,0) <= 0 THEN '< NO FILTER >' 
                 WHEN f.FilterID <> 0 THEN f.FilterName 
                 ELSE '< FILTER DELETED >' 
                 END, 
                 'FltrNavigateURL' = CASE 
                 WHEN f.FilterID <> 0 THEN '../lists/filters.aspx?FilterID='+CONVERT(VARCHAR,f.FilterID)+'&action=editFilter' 
                 ELSE '' 
                 END, 
                 'LayoutName' = CASE 
                 WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName 
                 ELSE '< CAMPAIGN DELETED >' 
                 END, 
                 'LytNavigateURL' = CASE 
                 WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN '../content/layouteditor.aspx?LayoutID='+CONVERT(VARCHAR,l.LayoutID)  
                 ELSE '' 
                 END, 
                 b.EmailSubject, b.BlastSuppression, b.EmailFromName, b.EmailFrom, b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal, l.LayoutID 
                 FROM Blast b WITH (NOLOCK) LEFT OUTER JOIN Groups g WITH (NOLOCK) ON b.groupID = g.groupID LEFT OUTER JOIN Filter f WITH (NOLOCK) ON b.filterID = f.filterID 
                 LEFT OUTER JOIN Layout l WITH (NOLOCK) on b.LayoutID = l.LayoutID  
                 WHERE b.BlastID =  @blastID

 
End