CREATE PROCEDURE [dbo].[e_Subscriber_ClearWaveMailingInfo]
@WaveMailingID int
AS
	UPDATE Subscriber
	SET WaveMailingID = 0, IsInActiveWaveMailing = 0
	WHERE WaveMailingID = @WaveMailingID
