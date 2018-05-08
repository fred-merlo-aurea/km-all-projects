CREATE PROCEDURE [dbo].[e_LayoutPlans_Select_SmartFormID]
	@SmartFormID int,
	@CustomerID int
AS
	SELECT lp.LayoutPlanID, lp.LayoutID, lp.EventType, lp.BlastID, lp.Period, lp.Criteria, lp.CustomerID, lp.GroupID,
  isnull(lp.Status,'Y') as Status, g.GroupName, gr.GroupName AS 'ResponseName', g.GroupID AS 'ResponseGroupID', 
  NULL AS 'LayoutName', 
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
  JOIN Groups g
   on g.GroupID = lp.GroupID
  LEFT OUTER JOIN Blast b
   on lp.BlastID = b.BlastID and b.StatusCode <> 'Deleted' 
  LEFT OUTER JOIN groups gr 
  on b.GroupID = gr.GroupID
  LEFT OUTER JOIN TriggerPlans tp 
  on tp.RefTriggerID = lp.BlastID and tp.IsDeleted = 0 
  LEFT JOIN Blast tb 
  on tp.BlastID = tb.BlastID and tb.StatusCode <> 'Deleted' 
  where lp.SmartFormID = @SmartFormID  AND lp.CustomerID = @customerID   and lp.IsDeleted = 0 ;