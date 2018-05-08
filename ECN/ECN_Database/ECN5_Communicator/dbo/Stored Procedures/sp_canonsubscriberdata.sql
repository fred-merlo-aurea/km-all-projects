CREATE proc [dbo].[sp_canonsubscriberdata]
(
	@emailID int,
	@groupID int
)
as
Begin
	select CASE WHEN e.emailaddress LIKE '%CRWI.KMPSGROUP.COM' THEN '' ELSE e.emailaddress END as emailaddress, e.firstname, e.lastname, e.company, e.title, e.address, e.city, e.state, e.zip, e.country, e.voice, e.mobile, e.fax,
			(select top 1 datavalue from emaildatavalues where emailID = @emailID and groupdatafieldsID in (select groupdatafieldsID from groupdatafields where groupID = @groupID and shortname='mailstop' )) as mailstop,
			(select top 1 datavalue from emaildatavalues where emailID = @emailID and groupdatafieldsID in (select groupdatafieldsID from groupdatafields where groupID = @groupID and shortname='SubscriberID' )) as SubscriberID
	from emails e join emailgroups eg on e.emailID = eg.emailID
	where groupID = @groupID and e.emailID = @emailID
end
