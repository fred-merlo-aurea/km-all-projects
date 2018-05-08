CREATE PROCEDURE [dbo].[e_TransformationPubMap_Delete]
@TransformationID int
AS
BEGIN

	set nocount on

	Delete from TransformationPubMap 
	where TransformationPubMapID = @TransformationID;
	Select @TransformationID;
	
END