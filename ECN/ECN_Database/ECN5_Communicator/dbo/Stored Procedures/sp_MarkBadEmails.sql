CREATE proc [dbo].[sp_MarkBadEmails]
(
	@GroupID int
)
as
Begin
	Update	emailgroups
	set		SubscribeTypeCode = 'D',
			LastChanged = getdate() 
	where	emailgroupID in (
		SELECT eg.emailGroupID  FROM emails e join emailgroups eg on e.emailID = eg.emailID 
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
