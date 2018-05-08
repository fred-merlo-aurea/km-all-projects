CREATE proc [dbo].[e_EmailGroup_MarkBadEmails]
(
	@GroupID int
)
as
Begin
	Update	[EmailGroups]
	set		[EmailGroups].SubscribeTypeCode = 'D',
			[EmailGroups].LastChanged = getdate() 
	where	emailgroupID in (
		SELECT eg.emailGroupID  FROM [Emails] e WITH (NOLOCK) join [EmailGroups] eg WITH (NOLOCK) on e.emailID = eg.emailID 
		WHERE groupID = @GroupID and 
		NOT 
		 ( 
			  CHARINDEX ( ' ',LTRIM(RTRIM([emailAddress]))) = 0  
		 AND       LEFT(LTRIM([emailAddress]),1) <> '@'  
		 AND       RIGHT(RTRIM([emailAddress]),1) <> '.'  
		 AND       CHARINDEX ( '.',[emailAddress],CHARINDEX ( '@',[emailAddress])) - CHARINDEX ( '@',[emailAddress]) > 1  
		 AND       LEN(LTRIM(RTRIM([emailAddress]))) - LEN(REPLACE(LTRIM(RTRIM([emailAddress])),'@','')) = 1  
		 AND       CHARINDEX ( '.',REVERSE(LTRIM(RTRIM([emailAddress])))) >= 3  
		 AND       ( CHARINDEX ('.@',[emailAddress]) = 0 AND CHARINDEX ( '..',[emailAddress]) = 0))  )

	select @@ROWCOUNT
End