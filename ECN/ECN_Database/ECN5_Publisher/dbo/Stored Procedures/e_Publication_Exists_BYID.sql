CREATE PROCEDURE [dbo].[e_Publication_Exists_BYID]   
@PublicationID int,
@CustomerID int

AS
	if exists(select top 1 PublicationID from Publication where PublicationID = ISNULL(@PublicationID, -1) and customerID = @CustomerID and IsDeleted = 0) select 1 else select 0