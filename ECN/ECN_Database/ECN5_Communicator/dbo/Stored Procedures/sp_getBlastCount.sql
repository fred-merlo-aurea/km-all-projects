CREATE proc [dbo].[sp_getBlastCount] (
	@customerID int,
	@groupIDs varchar(4000), 
	@suppGroupIDs varchar(4000)
)
as 
begin 
	declare @mastSuppressionGroupID int
	select @mastSuppressionGroupID = GroupID from Groups where GroupName = 'Master Supression' and CustomerID = @customerID
	if len(@suppGroupIDs) > 0 
		set @suppGroupIDs = @suppGroupIDs+','+Convert(varchar,@mastSuppressionGroupID)
	else	
		set @suppGroupIDs = Convert(varchar,@mastSuppressionGroupID)

		exec('select count(eg.emailID) from '+ 
				' emailgroups eg left outer join emailgroups eg1 on eg.emailID = eg1.emailID and eg1.groupID in('+@suppGroupIDs+') '+ 
				' where eg.groupID in ('+@groupIDs+') and eg.subscribeTypeCode = ''S'' and eg1.emailgroupID is null')
end
