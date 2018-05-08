CREATE PROCEDURE [dbo].[e_LandingPageAssignContent_Save]   
@LPACID int = null,
@Display varchar(MAX),
@LPAID int,
@LPOID int,
@UserID int,
@IsDeleted bit,
@SortOrder int

AS
if(@LPACID is null)
BEGIN
	insert into LandingPageAssignContent(LPAID, LPOID, Display, CreatedUserID, CreatedDate, IsDeleted, SortOrder)
	values (@LPAID, @LPOID, @Display, @UserID, GETDATE() ,0, @SortOrder)
	END
	else
	BEGIN
		update LandingPageAssignContent
		set Display = @Display, UpdatedUserID = @UserID, UpdatedDate = GETDATE(), IsDeleted = @IsDeleted, SortOrder = @SortOrder
		where LPACID = @LPACID
	END
	SELECT @@IDENTITY