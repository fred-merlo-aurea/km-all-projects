create procedure e_AdHocDimensionGroup_Select_ClientID_SourceFileID
@ClientID int,
@SourceFileID int
as
BEGIN

	set nocount on

	select *
	from AdHocDimensionGroup with(nolock)
	where ClientID = @ClientID
	and SourceFileID = @SourceFileID

END
go