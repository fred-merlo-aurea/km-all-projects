-- 2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE PROCEDURE [dbo].[e_Group_Select_GetMasterSuppression]  

@CustomerID int

AS

SET NOCOUNT ON

SELECT 
	* 
FROM 
	Groups WITH(NOLOCK) 
WHERE 
	CustomerID = @CustomerID 
	AND MasterSupression = 1