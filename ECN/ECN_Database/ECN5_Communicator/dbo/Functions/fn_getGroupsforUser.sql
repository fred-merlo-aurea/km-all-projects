--************************************** --      
-- Name: Split Function -- Description:Enables SQL Server to perform the Split Function in stored procedures/views/functions 
-- By: Sunil 
-- Inputs:A STRING that you would like to 
--   split down into individual elements, based 
--   on the DELIMITER specified -- 
-- Returns:a table with a row for each i 
--     item found between the delimiter you specify 
-- 
CREATE FUNCTION [dbo].[fn_getGroupsforUser](@CustomerID int, @userID int) 
RETURNS 
		@Group TABLE (GroupID int) 
AS     

BEGIN     
	if (select count(u.groupID) from usergroups u with(nolock) join Groups g with(nolock) on u.groupid = g.groupid and g.customerid = @CustomerID where UserID = @userID and u.IsDeleted = 0) > 0
	Begin
		insert into @Group
		select distinct u.groupID 
		from usergroups u  with(nolock)
		join Groups g with(nolock) on u.GroupID = g.GroupID
		where UserID = @userID  and g.CustomerID = @CustomerID and u.IsDeleted = 0
	End
	Else
	Begin
		insert into @Group
		select distinct groupID from [Groups] with(nolock) where CustomerID = @customerID
	END 
	
	RETURN 

End
