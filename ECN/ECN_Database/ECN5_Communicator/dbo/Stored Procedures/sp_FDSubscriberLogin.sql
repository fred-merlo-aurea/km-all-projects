CREATE PROCEDURE [dbo].[sp_FDSubscriberLogin](
	@GroupID int,
	@EmailAddress varchar(255) = null,
	@UDFID int = null,
	@UDFValue varchar(500) = null,
	@Password varchar(25) = null,
	@User1 varchar(255) = null,
	@User2 varchar(255) = null,
	@User3 varchar(255) = null,
	@User4 varchar(255) = null,
	@User5 varchar(255) = null,
	@User6 varchar(255) = null
	)
	as   
BEGIN   
	DECLARE @EmailID int = 0
	If (@EmailAddress is not null)
	Begin
		  --EmailAddress and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.EmailAddress = @EmailAddress and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --EmailAddress alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.EmailAddress = @EmailAddress and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@UDFID is not null)
	Begin
		  --UDF and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
					  join EmailDataValues edv with (nolock) on edv.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  edv.DataValue = @UDFValue and
					  edv.GroupDatafieldsID = @UDFID and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --UDF alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
					  join EmailDataValues edv with (nolock) on edv.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  edv.DataValue = @UDFValue and
					  edv.GroupDatafieldsID = @UDFID and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@User1 is not null)
	Begin
		  --User1 and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User1 = @User1 and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --User1 alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User1 = @User1 and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@User2 is not null)
	Begin
		  --User2 and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User2 = @User2 and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --User2 alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User2 = @User2 and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@User3 is not null)
	Begin
		  --User3 and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User3 = @User3 and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --User3 alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User3 = @User3 and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@User4 is not null)
	Begin
		  --User4 and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User4 = @User4 and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --User4 alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User4 = @User4 and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@User5 is not null)
	Begin
		  --User5 and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User5 = @User5 and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --User5 alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User5 = @User5 and
					  eg.SubscribeTypeCode = 'S'
		  End
	End
	Else If (@User6 is not null)
	Begin
		  --User6 and Password
		  If (@Password is not null)
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User6 = @User6 and
					  e.[Password] = @Password COLLATE SQL_Latin1_General_CP1_CS_AS and
					  eg.SubscribeTypeCode = 'S'
		  End
		  --User6 alone
		  Else
		  Begin
				Select @EmailID = e.EmailID
				From
					  Emails e with (nolock)
					  join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID
				Where
					  eg.GroupID = @GroupID and
					  e.User6 = @User6 and
					  eg.SubscribeTypeCode = 'S'
		  End
	End

	Select @EmailID
END