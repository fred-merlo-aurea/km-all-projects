CREATE PROCEDURE [dbo].[e_LayoutPlan_Exists_GroupID]
	@GroupID int,
	@Criteria varchar(50)
AS
	if exists(
			select top 1 * 
			from LayoutPlans lp with(nolock)
			where lp.GroupID = @GroupID and (lp.Status = 'Y' or (lp.Status = 'N' and isnull(lp.IsDeleted,0) = 1)) and lp.Criteria = @Criteria
			)
			select 1
	else
		select 0
