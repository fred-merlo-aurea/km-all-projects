CREATE  PROC [dbo].[e_Group_Count_PubNewsletters] 
(
	@GroupID int = NULL
)
AS 
BEGIN
	if exists (SELECT TOP 1 ECNGroupID FROM KMPSJointForms..PubNewsletters WHERE ECNGroupID = @GroupID) select 1 else select 0
END