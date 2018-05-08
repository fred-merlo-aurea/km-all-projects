CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_MergeAnonActivity]
	@AnonEmail varchar(255),
	@ActualEmail varchar(255),
	@BaseChannelID int
AS
	declare @AnonProfileID int, @ActualProfileID int

	select @AnonProfileID = ProfileID from DomainTrackerUserProfile dtup with(nolock) where dtup.BaseChannelID = @BaseChannelID and dtup.EmailAddress = @AnonEmail
	select @ActualProfileID = ProfileID from DomainTrackerUserProfile dtup with(nolock) where dtup.BaseChannelID = @BaseChannelID and dtup.EmailAddress = @ActualEmail

	if @AnonProfileID > 0 and @ActualProfileID > 0
	BEGIN
		update DomainTrackerActivity
		set ProfileID = @ActualProfileID
		where ProfileID = @AnonProfileID

		declare @currentDate datetime = GETDATE()

		update DomainTrackerUserProfile 
		set ConvertedDate = @currentDate
		where ProfileID = @ActualProfileID

		update DomainTrackerUserProfile 
		set ConvertedDate = @currentDate
		where ProfileID = @AnonProfileID

		delete from DomainTrackerUserProfile
		where ProfileID = @AnonProfileID

	END