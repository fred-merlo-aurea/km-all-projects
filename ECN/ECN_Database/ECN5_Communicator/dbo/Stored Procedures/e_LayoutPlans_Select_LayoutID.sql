CREATE PROCEDURE [dbo].[e_LayoutPlans_Select_LayoutID]   
@layoutID int,
@customerID int
AS
	 SELECT lp.LayoutPlanID, lp.LayoutID, lp.EventType, lp.BlastID, lp.Period, lp.Criteria, lp.CustomerID, lp.GroupID,
  isnull(lp.Status,'Y') as Status, l.LayoutName, lr.LayoutName AS 'ResponseName', l.LayoutID AS 'ResponseLayoutID', 
  NULL AS 'GroupName', 
  CASE 
	  WHEN tp.BlastID IS NULL 
	  THEN lp.ActionName 
  ELSE (lp.ActionName +'&nbsp;&nbsp;'+'<sub><img src=/ecn.images/images/icon-no-open.gif alt=''Indicates a NO OPEN Trigger'' border=0></sub>') 
  END AS ActionName,
  CASE
	  WHEN tb.BlastID IS NULL 
	  THEN '' 
  ELSE  '<sub><a href=../blasts/reports.aspx?BlastID='+Convert(varchar,tb.BlastID)+'><img src=/ecn.images/images/icon-reports.gif alt=''View NO OPEN Trigger Reporting'' border=0></a></sub>'
  END AS NOOPRptLnk  
  FROM layoutplans lp 
  JOIN Layout l
   on l.LayoutID = lp.LayoutID and l.IsDeleted = 0 
  JOIN Blast b
   on lp.BlastID = b.BlastID and b.StatusCode <> 'Deleted' 
  JOIN Layout lr 
  on b.LayoutID = lr.LayoutID and lr.IsDeleted = 0  
  LEFT OUTER JOIN TriggerPlans tp 
  on tp.RefTriggerID = lp.BlastID and tp.IsDeleted = 0 
  LEFT JOIN Blast tb 
  on tp.BlastID = tb.BlastID and tb.StatusCode <> 'Deleted' 
  where lp.LayoutID=@layoutID
  AND lp.CustomerID = @customerID   and lp.IsDeleted = 0 ;