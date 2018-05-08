create proc [dbo].[e_UserTracking_Select_All]
as
BEGIN
	
	SET NOCOUNT ON
	
	
	select * 
	from UserTracking with(nolock) 

End