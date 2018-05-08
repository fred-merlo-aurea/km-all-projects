CREATE PROCEDURE e_PublicationSequence_Select_NextSeqID_PublicationID_UserID
@PublicationID int,
@UserID int
AS
BEGIN

	set nocount on

	DECLARE @NextID int = (SELECT MAX(SequenceID) + 1 FROM PublicationSequence With(NoLock)	WHERE PublicationID = @PublicationID)
	
	INSERT INTO PublicationSequence (PublicationID,SequenceID,DateCreated,CreatedByUserID)
	VALUES(@PublicationID,@NextID,GETDATE(),@UserID);
	
	SELECT @NextID 

END