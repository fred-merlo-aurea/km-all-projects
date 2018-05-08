CREATE PROCEDURE [dbo].[e_QuickTestBlastConfig_Select_KMDefaultConfig]
AS
	SELECT *
	FROM QuickTestBlastConfig WITH (NOLOCK)
	WHERE IsDefault = 1
