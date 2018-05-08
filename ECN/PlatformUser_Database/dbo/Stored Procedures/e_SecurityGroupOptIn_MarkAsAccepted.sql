
CREATE PROCEDURE e_SecurityGroupOptIn_MarkAsAccepted
	@SecurityGroupOptInID int
AS
BEGIN
	UPDATE SecurityGroupOptIn
	SET HasAccepted = 1, DateAccepted = GETDATE()
	WHERE SecurityGroupOptInID = @SecurityGroupOptInID
END