CREATE proc  [dbo].[sp_ChannelMasterSuppression_Update]
(
	@basechannelID int,
	@groupIDs varchar(4000)
)
as
Begin
	declare @Groups table  (GroupID int)

	insert into @groups
	select items from fn_split(@groupIDs, ',')

	insert into ChannelMasterSuppressionList (BaseChannelID, EmailAddress)
	select	distinct @basechannelID, e.emailaddress
	from	emailgroups eg join @groups tg on eg.groupID = tg.groupID join
			emails e on eg.emailID = e.emailID left outer join
			ChannelMasterSuppressionList cms on cms.emailaddress = e.emailaddress AND basechannelID = @basechannelID
	where cms.emailaddress is null and 
			(	
				Convert(varchar(10), eg.CreatedOn, 101) = convert(varchar(10),dateadd(dd, -1, getdate()), 101) or
				Convert(varchar(10), eg.LastChanged, 101) = convert(varchar(10),dateadd(dd, -1, getdate()), 101)
			)

	select @@ROWCOUNT
		
End