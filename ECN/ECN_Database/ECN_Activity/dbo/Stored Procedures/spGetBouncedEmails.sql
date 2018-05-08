CREATE PROCEDURE [dbo].[spGetBouncedEmails] @customerID int, @score int
AS
Begin

	declare @startdt datetime,
			@enddt datetime,
			@MSGroupID int

	set nocount on

	select @msgroupID = groupID from ecn5_communicator..groups where customerID = @customerID and groupname = 'Master Supression'

	set @startdt = convert(varchar(10),dateadd(d, -1, getdate()),101)
	set @enddt = dateadd(s , -1, dateadd(d, 1, @startdt))

	SELECT bab.emailID, bab.blastID, e.EmailAddress, bouncescore
	from	BlastActivityBounces bab with (nolock) join 
			ecn5_communicator..emails e on bab.emailID = e.emailID 
			JOIN BounceCodes bc with (nolock) on bab.BounceCodeID = bc.BounceCodeID
	where 
			bc.BounceCode = 'hardbounce' and 
			BounceTime between @startdt and @enddt and
			e.customerID= @customerID and 
			bouncescore > @score and 
			not exists (select emailID from ecn5_communicator..emailgroups where groupID = @msgroupID and emailID = e.emailID) 
end
