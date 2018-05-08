﻿CREATE PROCEDURE [dbo].[e_SmartFormsPrePopFields_Select_SFID]   
@SFID INT = NULL
AS
	SELECT sfppf.*, g.CustomerID
	FROM 
		SmartFormsPrePopFields sfppf with (nolock)
		JOIN SmartFormsHistory sfh WITH (NOLOCK) ON sfppf.SFID = sfh.SmartFormID
		JOIN [Groups] g WITH (NOLOCK) ON sfh.GroupID = g.GroupID
	WHERE 
		sfppf.SFID = @SFID AND 
		sfppf.IsDeleted = 0 AND
		sfh.IsDeleted = 0
