CREATE PROCEDURE [dbo].[e_EmailDirect_Select_EmailDirectID]
	@EmailDirectID int
AS
	Select * from EmailDirect ed with(nolock) where ed.EmailDirectID = @EmailDirectID