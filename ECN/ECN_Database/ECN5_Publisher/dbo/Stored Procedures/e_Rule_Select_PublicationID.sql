CREATE PROCEDURE [dbo].[e_Rule_Select_PublicationID]   
@PublicationID int

AS
		
		select r.*, pb.CustomerID  from rules r join 
		edition e on r.editionID = e.editionID join
		Publication pb on pb.PublicationID = e.PublicationID
		where r.publicationID = @PublicationID