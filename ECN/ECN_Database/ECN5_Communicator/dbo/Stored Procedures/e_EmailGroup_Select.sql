﻿CREATE  PROC [dbo].[e_EmailGroup_Select] 
(
    @EmailID int = NULL,
    @GroupID int = NULL
)
AS 
BEGIN
	select eg.*, g.CustomerID 
	from [EmailGroups] eg with (nolock)  
	join [Groups] g  with (nolock) 
	on eg.GroupID=g.GroupID 
	where eg.EmailID=@EmailID and
	eg.GroupID=@GroupID
	
END