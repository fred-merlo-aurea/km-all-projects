CREATE PROCEDURE [dbo].[e_Publication_Select_PublicationID]   
@PublicationID int
AS
	select * from Publication where PublicationID = @PublicationID and IsDeleted=0