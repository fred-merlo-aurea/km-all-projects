CREATE  PROC [dbo].[e_EmailGroup_Exists_EmailAddress_GroupID] 
(
    @EmailAddress varchar(255) = NULL,
    @GroupID int = NULL,
    @CustomerID int = NULL
)
AS 
BEGIN
	if exists (
				select top 1 eg.EmailGroupID 
				from 
					[EmailGroups] eg with (nolock)  
					join [Groups] g  with (nolock) on eg.GroupID=g.GroupID 
					join [Emails] e  with (nolock) on eg.EmailID=e.EmailID 
				where 
					e.EmailAddress=@EmailAddress and
					eg.GroupID=@GroupID and
					g.CustomerID = @CustomerID
				) select 1 else select 0
	
END
