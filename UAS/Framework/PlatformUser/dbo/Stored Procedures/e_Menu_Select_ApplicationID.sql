CREATE PROCEDURE [dbo].[e_Menu_Select_ApplicationID]
@ApplicationID int
AS
	select *
	from Menu
	where ApplicationID = @ApplicationID
