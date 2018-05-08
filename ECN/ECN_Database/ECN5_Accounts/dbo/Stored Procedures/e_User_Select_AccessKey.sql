CREATE PROCEDURE [dbo].[e_User_Select_AccessKey]
@AccessKey varchar(500)
AS
Begin

SELECT *
FROM 
	[Users] WITH(NOLOCK) 
WHERE 
	AccessKey = @AccessKey and
	IsDeleted = 0 and 
	ActiveFlag = 'Y'
	
End
