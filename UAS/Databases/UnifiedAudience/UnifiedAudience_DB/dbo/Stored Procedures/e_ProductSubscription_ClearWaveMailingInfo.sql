CREATE PROCEDURE [dbo].[e_ProductSubscription_ClearWaveMailingInfo]
@WaveMailingID int
AS
BEGIN

	set nocount on

	UPDATE PubSubscriptions
	SET WaveMailingID = 0, IsInActiveWaveMailing = 0
	WHERE WaveMailingID = @WaveMailingID

END