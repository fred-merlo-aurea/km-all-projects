CREATE  PROC [dbo].[e_Group_Count_DigitalEditions] 
(
	@GroupID int = NULL
)
AS 
BEGIN
	if exists (SELECT TOP 1 GroupID FROM ecn5_publisher..[PUBLICATION] WHERE GroupID = @GroupID and IsDeleted=0) select 1 else select 0
END