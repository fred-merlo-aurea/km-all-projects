CREATE PROCEDURE [dbo].[e_Edition_Select_PublicationCode]   
@publicationcode varchar(50)

AS
	select top 1  e.*, p.CustomerID 
	from 
		Edition e join 
		Publication p on e.PublicationID = P.PublicationID 
	where 
		p.PublicationCode = @publicationcode and 
		e.IsDeleted=0 and 
		p.IsDeleted=0 and Status = 'Active'
	order by EditionID desc

