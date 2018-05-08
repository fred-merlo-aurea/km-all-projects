﻿CREATE PROCEDURE [dbo].[e_FilterGroup_Select_FilterGroupID]   
@FilterGroupID int = NULL
AS
	SELECT fg.*, f.CustomerID
	FROM FilterGroup fg with (nolock) 
		join Filter f with (nolock) on fg.FilterID = f.FilterID 
	WHERE 
		fg.FilterGroupID = @FilterGroupID and 
		f.IsDeleted = 0 and
		fg.IsDeleted = 0
