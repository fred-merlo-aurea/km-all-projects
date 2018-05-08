
CREATE PROCEDURE e_ActionMap_Delete
@FromActionID int,
@ToActionID int
AS
DELETE ActionMap WHERE FromActionID = @FromActionID AND ToActionID = @ToActionID
