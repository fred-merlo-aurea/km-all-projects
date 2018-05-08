CREATE PROCEDURE [dbo].[e_GetGroups_User] 
	@CustomerID int, 
	@userID int
AS     
BEGIN     
	if (select count(u.groupID) from usergroups u where UserID = @userID) > 0
	Begin		
		select g.* from Groups g join usergroups ug on ug.GroupID = g.GroupID where UserID = @userID  order by GroupName
	End
	Else
	Begin		
		select * from Groups where CustomerID = @customerID order by GroupName
	END 
End
