create  PROCEDURE [dbo].[sp_Adhoc_SelectAll]   
AS  
BEGIN
	
	SET NOCOUNT ON


	Select * 
	from adhoc WITH(NOLOCK)
	order by sortorder

End