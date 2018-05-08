create procedure e_SubGenUserMap_Save
@UserID int,
@ClientID int,
@SubGenUserId int,
@SubGenAccountId int,
@SubGenAccountName varchar(50)
as

if not exists(select UserID from SubGenUserMap with(nolock) where UserID = @UserID and SubGenUserId = @SubGenUserId and ClientID = @ClientID)
	begin
		insert into SubGenUserMap (UserID,ClientID,SubGenUserId,SubGenAccountId,SubGenAccountName)
		values(@UserID,@ClientID,@SubGenUserId,@SubGenAccountId,@SubGenAccountName)
	end
