create procedure job_CodeSheet_AddNoValue
as
BEGIN   

	SET NOCOUNT ON 

	insert into CodeSheet (PubID,ResponseGroup,Responsevalue,Responsedesc,ResponseGroupID,DateCreated,CreatedByUserID,DisplayOrder,IsActive,IsOther)
	select rg.PubID,rg.ResponseGroupName,'xxNVxx','No Value',rg.ResponseGroupID,getdate(),1,99,'true','false'
	from ResponseGroups rg with(nolock)
	where ResponseGroupID not in
	(
		select ResponseGroupID 
		from CodeSheet
		where Responsevalue = 'xxNVxx'
	)
	and rg.ResponseGroupName != 'PUBCODE'

END
go