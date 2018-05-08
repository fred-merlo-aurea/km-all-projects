CREATE PROCEDURE [dbo].[e_WaveMailingDetail_Select_IssueID]
@IssueID int
AS
	SELECT * FROM WaveMailingDetail wmd With(NoLock)
	JOIN WaveMailing wm With(NoLock) ON wmd.WaveMailingID = wm.WaveMailingID
	WHERE wm.IssueID = @IssueID