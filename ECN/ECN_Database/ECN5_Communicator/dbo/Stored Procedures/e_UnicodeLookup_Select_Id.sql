CREATE PROCEDURE [dbo].[e_UnicodeLookup_Select_Id]
	@Id int
AS
	Select * from UnicodeLookup ul with(nolock)
	where ul.Id = @Id
