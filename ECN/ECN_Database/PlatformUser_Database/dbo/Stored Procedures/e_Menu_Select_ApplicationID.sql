CREATE PROCEDURE [dbo].[e_Menu_Select_ApplicationID]
@ApplicationID int,
@IsActive bit
AS
	select *
	from Menu
	where ApplicationID = @ApplicationID
	and IsActive = @IsActive
GO

