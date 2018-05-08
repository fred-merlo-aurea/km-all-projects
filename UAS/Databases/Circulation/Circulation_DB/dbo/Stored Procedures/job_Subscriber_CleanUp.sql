CREATE PROCEDURE [dbo].[job_Subscriber_CleanUp]

AS
BEGIN

	SET NOCOUNT ON;
	-- This proc is used to delete subscriber and subscription records that are NEWLY created but a system shut down occured.
	-- System shut down as in, the users computer froze or the users computer lost power

	-- Create temp table
	create table #SubscriberForDelete(SubscriberID Int, SubscriptionID int)
	
	-- Insert ID's that are to be deleted
	insert into #SubscriberForDelete
	select SubscriberID,SubscriptionID 
	from Circulation..Subscription
	where ActionID_Current <= 0 and CreatedByUserID in (
		select distinct ual.UserID
		from UAS..[Service] s with(nolock)
				join UAS..ClientGroupServiceMap cgsm with(nolock) on s.ServiceID = cgsm.ServiceID
				join UAS..ClientGroupUserMap cgum with(nolock) on cgsm.ClientGroupID = cgum.ClientGroupID
				join UAS..UserAuthorizationLog ual with(nolock) on cgum.UserID = ual.UserID
		where s.ServiceID = 1 
				and s.ServiceCode = 'FUL' 
				and CONVERT(Date,logoutdate) = CONVERT(Date,GETDATE())
				and CONVERT(TIME,LogOutTime) >= Convert(TIME,DATEADD(hour, -1, SYSDATETIMEOFFSET())) )
	
	-- Begin Delete
	delete s from Subscriber s join #SubscriberForDelete sd on s.SubscriberID = sd.SubscriberID
	delete ss from Subscription ss join #SubscriberForDelete sd on ss.SubscriptionID = sd.SubscriptionID
	
	drop table #SubscriberForDelete
	
	-- Delete records that have actionId_current equal or less than 0 and is 24 hours or older
	delete s from Subscriber s join Subscription ss on s.SubscriberID = ss.SubscriberID where ss.ActionID_Current <= 0 and ss.DateCreated < DATEADD(hour,-24,getdate())
	delete from subscription where ActionID_Current <= 0 and DateCreated < DATEADD(hour,-24,getdate())

END