CREATE  PROC [dbo].[e_Group_Count_JointForms] 
(
	@GroupID int = NULL
)
AS 
BEGIN
	if exists ( select Top 1 ECNDefaultGroupID from KMPSJointForms..Publications WITH (NOLOCK) where ECNDefaultGroupID = @GroupID)
	BEGIN
	 select 1 
	END
	else if exists ( select Top 1 ECNGroupID from KMPSJointForms..PubNewsletters WITH (NOLOCK) where ECNGroupID = @GroupID)
	BEGIN
	 select 1 
	END
	else 
	BEGIN
		select 0
	END
END
