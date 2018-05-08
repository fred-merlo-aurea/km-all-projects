CREATE proc [dbo].[spUpdateMasterSuppressedinNeebo]
as
Begin
	update dataset_new
	set IsMasterSuppressed= 1
	where EmailAddress in (
			select EmailAddress from ECN5_COMMUNICATOR..Emails e with (NOLOCK) join ECN5_COMMUNICATOR..EmailGroups eg with (NOLOCK) on e.EmailID = eg.EmailID where groupID = 2000
			union 
			select EmailAddress from ECN5_COMMUNICATOR.dbo.ChannelMasterSuppressionList with (NOLOCK) where BaseChannelID = 25
			)
		
End

