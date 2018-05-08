create procedure e_AdHocDimensionGroup_Select_ClientID_SourceFileID_AdHocDimensionGroupName
@ClientID int,
@SourceFileID int,
@AdHocDimensionGroupName varchar(50)
as
BEGIN

	set nocount on

	select *
	from AdHocDimensionGroup with(nolock)
	where ClientID = @ClientID
	and SourceFileID = @SourceFileID
	and AdHocDimensionGroupName = @AdHocDimensionGroupName 

END
go