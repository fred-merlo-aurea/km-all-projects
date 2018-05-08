

CREATE PROCEDURE [dbo].[e_History_Select_Active_User_BatchID]
@UserID int,
@PublicationID int
AS

	SELECT * FROM History h With(NoLock)
	JOIN Batch b With(NoLock) ON h.BatchID = b.BatchID
	where b.IsActive = 1 and h.BatchCountItem >= 100 
	and b.UserID = @UserID
	and h.PublicationID = @PublicationID
	
