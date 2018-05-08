CREATE  PROC [dbo].[e_Group_Count_MasterSuppression] 
(
	@GroupID int = NULL
)
AS 
BEGIN
	if exists (SELECT TOP 1 GroupID FROM ecn5_communicator..Groups WHERE GroupID = @GroupID and MasterSupression = 1) select 1 else select 0
END