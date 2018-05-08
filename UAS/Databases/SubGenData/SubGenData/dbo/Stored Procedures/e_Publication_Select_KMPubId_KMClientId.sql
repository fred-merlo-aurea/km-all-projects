create procedure e_Publication_Select_KMPubId_KMClientId
 @kmPubId int,
 @kmClientId int
 as
 BEGIN

	set nocount on

	select p.*
	from Publication p with(nolock)
	join Account a with(nolock) on p.account_id = a.account_id
	where p.KMPubID = @kmPubId and a.KMClientId = @kmClientId

END
go