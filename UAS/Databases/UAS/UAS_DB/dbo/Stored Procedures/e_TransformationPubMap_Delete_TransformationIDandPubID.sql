CREATE PROCEDURE [dbo].[e_TransformationPubMap_Delete_TransformationIDandPubID]
@TransformationID int,
@PubID int
AS
BEGIN

	set nocount on

	Delete from TransformationPubMap 
	where TransformationID = @TransformationID and PubID = @PubID;
	Select @TransformationID;
	
END