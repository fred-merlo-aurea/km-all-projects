CREATE proc [dbo].[sp_getbouncedemails_InitialCleanup]
@customerID int,
@bouncescore int
as
Begin
	select distinct emailID, (select max(blastID) from emailactivitylog eal where eal.emailID = e.emailID and actiontypecode = 'bounce') as BlastID,  
	bouncescore from emails e where customerID = @customerID and bouncescore > @bouncescore
end
