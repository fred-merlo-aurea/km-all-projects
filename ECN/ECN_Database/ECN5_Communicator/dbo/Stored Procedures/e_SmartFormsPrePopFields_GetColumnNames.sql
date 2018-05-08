CREATE  PROC [dbo].[e_SmartFormsPrePopFields_GetColumnNames] 
(
@SFID int = NULL,
@GroupID int = NULL
)
AS 
BEGIN
	SELECT 
		c.Name as 'Name'
	FROM 
		SYSCOLUMNS c 
		INNER JOIN SYSOBJECTS o ON o.id = c.id 
	WHERE 
		o.Name = 'Emails' AND 
		c.Name NOT IN ( SELECT ProfileFieldName 
						FROM SmartFormsPrePopFields WITH (NOLOCK) 
						WHERE sfid = @SFID and IsDeleted = 0) 
	UNION 
	SELECT 
		'user_'+ShortName as 'Name'
	FROM 
		GroupDatafields WITH (NOLOCK)  
	WHERE 
		GroupID= @GroupID AND 
		IsDeleted = 0 AND
		'user_'+ShortName NOT IN (	SELECT ProfileFieldName 
									FROM SmartFormsPrePopFields WITH (NOLOCK) 
									where sfid = @SFID and IsDeleted = 0)
	ORDER BY 1
	
END