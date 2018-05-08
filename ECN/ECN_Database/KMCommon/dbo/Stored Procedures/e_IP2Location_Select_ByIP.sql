CREATE PROCEDURE [dbo].[e_IP2Location_Select_ByIP]
@IP BIGINT
AS
BEGIN
	SELECT 
		TOP 1 * 
	FROM 
		IP2Location WITH (NOLOCK)
	WHERE 
		@IP >= IpStart AND
		@IP <= IpEnd
END