CREATE PROCEDURE [dbo].[e_SecurityGroupServiceMap_Select]
@IsEnabled bit = true
AS
IF @IsEnabled IS NULL
	SELECT * FROM SecurityGroupServicMap m
ELSE
	SELECT * FROM SecurityGroupServicMap m WHERE @IsEnabled = @IsEnabled