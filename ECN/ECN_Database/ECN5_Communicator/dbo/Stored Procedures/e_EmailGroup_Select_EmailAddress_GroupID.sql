CREATE  PROC [dbo].[e_EmailGroup_Select_EmailAddress_GroupID] 
(
    @EmailAddress varchar(255) = NULL,
    @GroupID int = NULL
)
AS 
BEGIN
	select 
		eg.*, e.CustomerID 
	from 
		Emails e with (nolock)
		join [EmailGroups] eg with (nolock) on e.EmailID = eg.EmailID and eg.GroupID = @GroupID
		join [Groups] g  with (nolock) on eg.GroupID=g.GroupID and e.CustomerID = g.CustomerID
	where 
		e.EmailAddress=@EmailAddress
	
END
