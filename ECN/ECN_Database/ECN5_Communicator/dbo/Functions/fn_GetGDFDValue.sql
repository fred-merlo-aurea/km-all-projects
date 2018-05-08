CREATE FUNCTION [dbo].[fn_GetGDFDValue]
(
	@DataValue varchar(500),
	@SystemValue varchar(500)
	
)
RETURNS varchar(500)

AS
BEGIN
	if @SystemValue = 'CurrentDate'
	BEGIN
		RETURN Convert(varchar(500), GETDATE(),120)
	END
	else
	BEGIN
		Return(@DataValue)
	END
	RETURN @DataValue
END
