CREATE PROCEDURE [dbo].[e_GatewayValue_Delete_LoginOrCapture]
	@GatewayID int,
	@Login bit = null,
	@Capture bit = null
AS
	if(@Login is not null)
	BEGIN
		Update GatewayValue
		set IsDeleted = 1
		where GatewayID = @GatewayID and IsLoginValidator = 1
	END
	ELSE if (@Capture is not null)
	BEGIN
		Update GatewayValue
		set IsDeleted = 1
		where GatewayID = @GatewayID and IsCaptureValue = 1
	END