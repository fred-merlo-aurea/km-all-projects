CREATE PROCEDURE [dbo].[MovedToActivity_sp_GetBouncedEmails] @customerID int, @score int
AS
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_GetBouncedEmails', GETDATE())
	declare @startdt datetime,
			@enddt datetime,
			@MSGroupID int

	set nocount on

	select @msgroupID = groupID from groups where customerID = @customerID and groupname = 'Master Supression'

	set @startdt = convert(varchar(10),dateadd(d, -1, getdate()),101)
	set @enddt = dateadd(s , -1, dateadd(d, 1, @startdt))

	SELECT eal.emailID, eal.blastID, e.EmailAddress, bouncescore
	from	EmailActivityLog eal join 
			emails e on eal.emailID = e.emailID 
	where 
			eal.actiontypecode ='bounce' and 
			actionvalue = 'hardbounce' and 
			eal.actiondate between @startdt and @enddt and
			Processed='Y' and 
			e.customerID= @customerID and 
			bouncescore > @score and 
			not exists (select emailID from emailgroups where groupID = @msgroupID and emailID = e.emailID) 
end
