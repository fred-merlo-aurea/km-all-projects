CREATE proc [dbo].[e_EmailGroup_DeleteBadEmails]
(
	@GroupID int,
	@UserID int
)
as
Begin
	DELETE From	[EmailGroups]
	WHERE groupID = @GroupID and SubscribeTypeCode = 'D'

	select @@ROWCOUNT
End
