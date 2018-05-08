CREATE PROCEDURE [dbo].[e_SmartFormsHistory_GetGroupID]   
@SFID INT,
@CustomerID INT
AS
	SELECT sfh.GroupID
	FROM 
		SmartFormsHistory sfh WITH (NOLOCK)
		join Groups g WITH (NOLOCK) ON sfh.GroupID = g.GroupID
	WHERE
		sfh.SmartFormID = @SFID AND
		sfh.IsDeleted = 0 AND
		g.CustomerID = @CustomerID