create proc [dbo].[sp_EmailWizardRate] 
(
	@ChannelID int,
	@GroupID int
)
as
Begin

	declare @EmailCount int,
		@TotalAmount decimal(18,2)

	set @TotalAmount = 0

	select	@EmailCount = count(e.EmailID) FROM [ECN5_COMMUNICATOR].[DBO].Emails e join [ECN5_COMMUNICATOR].[DBO].EmailGroups eg on e.EmailID=eg.EmailID 
	WHERE 	eg.GroupID = @GroupID and SubscribeTypeCode='S'

	select 	@TotalAmount = BaseFee + (@EmailCount * convert(decimal(18,4),EmailRate)) 
	from 	wizard_EmailRates
	where 
			@EmailCount between EmailRangeStart and EmailRangeEnd and 
			baseChannelID = @ChannelID

	Select @TotalAmount

End
