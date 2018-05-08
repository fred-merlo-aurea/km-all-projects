CREATE PROCEDURE [dbo].[e_UserDataMask_Save]
@MaskUserID int,
@MaskField varchar(100),
@UserID int
AS
BEGIN

	SET NOCOUNT ON
	
	insert into UserDataMask (
		UserID,
		MaskField,
		CreatedUserID,
		CreatedDate) 
	values (
		@MaskUserID,
		@MaskField,
		@UserID,
		GETDATE())

	Select @@IDENTITY;
			
END
