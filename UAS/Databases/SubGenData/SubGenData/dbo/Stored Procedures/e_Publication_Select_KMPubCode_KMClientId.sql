create procedure e_Publication_Select_KMPubCode_KMClientId
 @kmPubCode varchar(50),
 @kmClientId int
 as
 BEGIN

	set nocount on

	select p.*
	from Publication p with(nolock)
	join Account a with(nolock) on p.account_id = a.account_id
	where p.KMPubcode = @kmPubCode and a.KMClientId = @kmClientId

END
go