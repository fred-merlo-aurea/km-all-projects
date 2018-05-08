CREATE proc [dbo].[sp_DELogin]
(
	@EditionID int,
	@username varchar(500),
	@pwd varchar(500)
)
as
Begin

	declare	@groupID int,
			@username_gdfID int,
			@pwd_gdfID int,
			@emailID int

	select @groupID = GroupID from ecn5_publisher..[EDITION] ed join ecn5_publisher..[PUBLICATION] m on ed.PublicationID = m.PublicationID where editionID = @EditionID and status='Active'
	set @username_gdfID = 0
	set @pwd_gdfID = 0

	select @username_gdfID = groupdatafieldsID from groupdatafields where groupID = @groupID and shortname = 'username'
	select @pwd_gdfID = groupdatafieldsID from groupdatafields where groupID = @groupID and shortname = 'pwd'

	--select @username_gdfID, @pwd_gdfID

	if @username_gdfID > 0  and @pwd_gdfID > 0
	Begin
		select @emailID = EmailID from emaildatavalues where groupdatafieldsID = @username_gdfID and datavalue = @username 

		if @emailID > 0 and exists (select emailid from emaildatavalues where groupdatafieldsID = @pwd_gdfID and emailID = @emailID and datavalue = @pwd)
		Begin
			select @emailID
			return
		End
	End

	select 0
End
