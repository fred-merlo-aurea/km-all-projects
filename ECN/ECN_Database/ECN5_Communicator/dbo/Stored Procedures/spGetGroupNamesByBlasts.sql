CREATE PROCEDURE [dbo].[spGetGroupNamesByBlasts]  
	@BlastIDs varchar(8000)
AS
BEGIN		
	SELECT '&nbsp;<a href=../lists/groupeditor.aspx?GroupID='+Convert(varchar,g.groupID)+'><img src=icon-smartForm.gif border=0/></a>&nbsp;<a href=reports.aspx?BlastID='+Convert(varchar,b.BlastID)+'><img src=icon-reports.gif border=0/></a>&nbsp;<a href=rpt_blastreport.aspx?BlastID='+Convert(varchar,b.BlastID)+'><img  src=icon-pdf.gif border=0/></a>' + COALESCE(g.groupName+'<BR>','') +'&nbsp;&nbsp;&nbsp;&nbsp;' as 'Groups' 
	FROM groups g JOIN [BLAST] b on g.groupID = b.groupID JOIN (SELECT ITEMS FROM DBO.fn_split(@BlastIDs, ',')) ids ON ids.items = b.blastID
END
