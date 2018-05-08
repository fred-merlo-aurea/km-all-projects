create procedure job_MasterCodeSheet_AddNoValue
as
BEGIN

	SET NOCOUNT ON

	insert into Mastercodesheet (MasterValue,MasterDesc,MasterGroupID,MasterDesc1,EnableSearching,SortOrder,DateCreated,CreatedByUserID)
	select 'xxNVxx','No Value',mg.MasterGroupID,'No Value','false',99,getdate(),1
	from MasterGroups mg with(nolock)
	where MasterGroupID not in
	(
		select MasterGroupID 
		from Mastercodesheet
		where MasterValue = 'xxNVxx'
	)
	and mg.Name != 'PUBCODE'

END
go