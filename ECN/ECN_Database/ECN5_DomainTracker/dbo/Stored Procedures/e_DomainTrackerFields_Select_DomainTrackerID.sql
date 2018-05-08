CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Select_DomainTrackerID]
@DomainTrackerID INT

AS

SET NOCOUNT ON

SELECT 
	*
FROM 
	DomainTrackerFields dtf WITH(NOLOCK)
WHERE 
	DomainTrackerID = @DomainTrackerID 
	AND dtf.IsDeleted = 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerFields_Select_DomainTrackerID] TO [ecn5]
    AS [dbo];

