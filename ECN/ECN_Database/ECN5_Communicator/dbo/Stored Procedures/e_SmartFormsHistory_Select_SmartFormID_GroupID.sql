﻿CREATE PROCEDURE [dbo].[e_SmartFormsHistory_Select_SmartFormID_GroupID]   
@GroupID INT = NULL,
@SmartFormID INT = NULL
AS
	SELECT sfh.*, g.CustomerID
	FROM 
		SmartFormsHistory sfh with (nolock)
		JOIN [Groups] g WITH (NOLOCK) ON sfh.GroupID = g.GroupID
	WHERE 
		sfh.GroupID = @GroupID AND 
		sfh.SmartFormID = @SmartFormID AND
		sfh.IsDeleted = 0