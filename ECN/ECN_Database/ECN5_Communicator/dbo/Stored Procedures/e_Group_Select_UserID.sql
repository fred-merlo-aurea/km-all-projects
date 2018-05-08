CREATE PROCEDURE [dbo].[e_Group_Select_UserID] 
	@CustomerID int, 
	@UserID int
AS     
BEGIN     
	if (select count(u.groupID) from usergroups u WITH (NOLOCK) where UserID = @userID) > 0
	Begin		
		select g.* from [Groups] g WITH (NOLOCK) join usergroups ug WITH (NOLOCK) on ug.GroupID = g.GroupID where UserID = @UserID order by GroupName
	End
	Else
	Begin		
		select * from [Groups] WITH (NOLOCK) where CustomerID = @customerID  order by GroupName
	END 
End