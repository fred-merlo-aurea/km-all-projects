
CREATE PROCEDURE e_ActionMap_Save
@FromActionID int,
@ToActionID int
AS
IF NOT EXISTS(SELECT FromActionID FROM ActionMap With(NoLock) WHERE FromActionID = @FromActionID AND ToActionID = @ToActionID)
	BEGIN
		INSERT INTO ActionMap (FromActionID,ToActionID)
		VALUES(@FromActionID,@ToActionID);
	END
