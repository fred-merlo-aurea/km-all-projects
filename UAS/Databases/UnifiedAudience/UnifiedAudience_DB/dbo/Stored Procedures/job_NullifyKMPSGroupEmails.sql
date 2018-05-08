create procedure job_NullifyKMPSGroupEmails
@ProcessCode varchar(50)
as
BEGIN

	SET NOCOUNT ON

	update sf
	set Email = null
	from SubscriberFinal sf
	where sf.ProcessCode = @ProcessCode
		and sf.Email like '%kmpsgroup.com%'

END
go