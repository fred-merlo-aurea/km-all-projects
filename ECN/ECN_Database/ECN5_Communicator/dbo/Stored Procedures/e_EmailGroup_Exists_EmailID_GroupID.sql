CREATE  PROC [dbo].[e_EmailGroup_Exists_EmailID_GroupID] 
(
    @EmailID int = NULL,
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
				where 
					eg.EmailID = @EmailID and
					eg.GroupID=@GroupID and
					g.CustomerID = @CustomerID
				) select 1 else select 0
	
END
