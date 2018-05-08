
CREATE PROCEDURE [dbo].[e_Subscription_Select_SequenceID]
@SequenceID int
AS
	SELECT * FROM Subscription With(NoLock) WHERE SequenceID = @SequenceID

