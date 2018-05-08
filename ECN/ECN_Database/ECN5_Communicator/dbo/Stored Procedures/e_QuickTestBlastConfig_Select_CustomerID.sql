CREATE PROCEDURE [dbo].[e_QuickTestBlastConfig_Select_CustomerID]
@CustomerID int
AS
	SELECT *
	FROM QuickTestBlastConfig WITH (NOLOCK)
	WHERE CustomerID = @CustomerID
