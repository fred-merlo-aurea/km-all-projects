CREATE PROCEDURE [dbo].[e_CrossTabReport_Select]
AS
BEGIN

	SET NOCOUNT ON

	select cr.*, p.PubName, 
		(case when IsNumeric(cr.Row) = 1 then case when cr.PubID > 0 then rgr.DisplayName else mgr.DisplayName end else cr.Row end) as RowDisplayName, (case when IsNumeric(cr.[Column]) = 1 then case when cr.PubID > 0 then rgc.DisplayName else mgc.DisplayName end else cr.[Column] end) as ColumnDisplayName
	from CrossTabReport cr with(nolock)   
		left outer join Pubs p with(nolock) on p.PubID = cr.PubID
		left outer join MasterGroups mgr  with(nolock) on (mgr.MasterGroupID = case when IsNumeric(cr.Row) = 1 then cr.Row end and cr.PubID = 0)
		left outer join MasterGroups mgc  with(nolock) on (mgc.MasterGroupID =  case when IsNumeric(cr.[Column]) = 1 then cr.[Column] end and cr.PubID = 0)
		left outer join ResponseGroups rgr  with(nolock) on (rgr.ResponseGroupID =  case when IsNumeric(cr.Row) = 1 then cr.Row end and cr.PubID > 0)
		left outer join ResponseGroups rgc  with(nolock) on (rgc.ResponseGroupID =  case when IsNumeric(cr.[Column]) = 1 then cr.[Column] end and cr.PubID > 0)		
	Where cr.IsDeleted = 0		
	order by  CrossTabReportName

END