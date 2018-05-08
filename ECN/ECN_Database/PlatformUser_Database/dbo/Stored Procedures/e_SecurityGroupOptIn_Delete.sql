
CREATE PROCEDURE e_SecurityGroupOptIn_Delete
	@SecurityGroupOptInID int
AS
BEGIN
	UPDATE SecurityGroupOptIn
	SET IsDeleted = 1
	WHERE SecurityGroupOptInID = @SecurityGroupOptInID
END