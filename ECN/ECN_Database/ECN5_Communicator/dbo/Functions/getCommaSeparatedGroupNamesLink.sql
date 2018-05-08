-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[getCommaSeparatedGroupNamesLink]  (
	@BlastGroupID INT
)
RETURNS VARCHAR(8000)
AS
BEGIN

DECLARE @blastIDs varchar(8000)

SELECT @blastIDs = BlastIDs FROM BlastGrouping WHERE BlastGroupID = @blastGroupID

declare @groupName varchar(8000)
SELECT @groupName = COALESCE(@groupName+'<BR>','') +'&nbsp;<a href=../lists/groupeditor.aspx?GroupID='+Convert(varchar,g.groupID)+'><img src=icon-smartForm.gif border=0/></a>&nbsp;<a href=reports.aspx?BlastID='+Convert(varchar,b.BlastID)+'><img src=icon-reports.gif border=0/></a>&nbsp;<a href=rpt_blastreport.aspx?BlastID='+Convert(varchar,b.BlastID)+'><img  src=icon-pdf.gif border=0/></a>&nbsp;&nbsp;&nbsp;&nbsp;'+g.GRoupName 
FROM groups g JOIN Blast b on g.groupID = b.groupID JOIN (SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = b.blastID 

/*
SELECT @groupName = COALESCE(@groupName+'<BR>','') +'&nbsp;&nbsp;&nbsp;<a href=/ecn.communicator/main/lists/groupeditor.aspx?GroupID='+Convert(varchar,g.groupID)+'><img alt="View Profiles" src=/ecn.images/images/icon-smartForm.gif border=0/></a>&nbsp;&nbs
p;&nbsp;<a href=/ecn.communicator/main/blasts/reports.aspx?BlastID='+Convert(varchar,b.BlastID)+'><img alt="View Reporting" src=/ecn.images/images/icon-reports.gif border=0/></a>&nbsp;&nbsp;&nbsp;<a href=/ecn.communicator/main/blasts/rpt_blastreport.aspx?
BlastID='+Convert(varchar,b.BlastID)+'><img alt="View Reporting" src=/ecn.images/images/icon-pdf.gif border=0/></a>&nbsp;&nbsp;&nbsp;&nbsp;'+g.GRoupName FROM groups g JOIN Blasts b on g.groupID = b.groupID JOIN (SELECT ITEMS FROM DBO.fn_split(@blastIDs, ',')) ids ON ids.items = b.blastID 
*/
RETURN @groupName 

END
