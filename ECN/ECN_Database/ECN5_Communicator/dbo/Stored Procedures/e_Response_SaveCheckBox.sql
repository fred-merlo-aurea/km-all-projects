CREATE PROCEDURE [dbo].[e_Response_SaveCheckBox] 
	@EmailID int,
	@GroupDataFieldsID int,
	@Answer varchar(100),
	@UserID int
AS
BEGIN
	INSERT INTO EmailDataValues(EmailID, GroupDatafieldsID, CreatedDate, CreatedUserID, DataValue)
	VALUES(@EmailID, @GroupDataFieldsID, GETDATE(), @UserID, @Answer)
END