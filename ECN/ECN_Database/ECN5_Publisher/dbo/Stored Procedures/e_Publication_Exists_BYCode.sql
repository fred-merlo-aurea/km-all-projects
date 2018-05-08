CREATE PROCEDURE [dbo].[e_Publication_Exists_BYCode]   
@PublicationID int,
@PublicationCode varchar(100)

AS
	if exists(select top 1 PublicationID from Publication where PublicationCode = @PublicationCode  and IsDeleted = 0 and PublicationID <> ISNULL(@PublicationID, -1) and len(@PublicationCode) > 0) select 1 else select 0