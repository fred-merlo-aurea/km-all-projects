CREATE PROCEDURE [dbo].[e_WaveMailingDetail_Select_IssueID]
@IssueID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT wmd.* 
	FROM WaveMailingDetail wmd With(NoLock)
		JOIN WaveMailing wm With(NoLock) ON wmd.WaveMailingID = wm.WaveMailingID
	WHERE wm.IssueID = @IssueID
	ORDER BY wmd.DateCreated ASC

END