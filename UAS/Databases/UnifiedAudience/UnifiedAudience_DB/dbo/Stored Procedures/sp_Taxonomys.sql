CREATE  PROCEDURE [dbo].[sp_Taxonomys]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	declare @mastergroupID int;
	
	select @mastergroupID=mastergroupID 
	from [MasterGroups] 
	where [Name]='Master_TOPICS';
	
	select * 
	from ((select a.masterID from (select top 10 mc.masterID as masterID, count(ps.subscriptionID) as subscriptions
		from SubscriberTopicActivity st 
			join pubsubscriptions ps on st.pubsubscriptionID = ps.pubsubscriptionID
			join (select masterID, masterdesc from mastercodesheet where mastergroupID = @mastergroupID) mc on st.TopicCode = mc.masterdesc 
		where TopicCode is not null and DATEPART(MONTH, ActivityDate)=DATEPART(MONTH, GETDATE()) and  DATEPART(YEAR, ActivityDate)=DATEPART(YEAR, GETDATE())
		group by mc.masterID, mc.masterdesc,DATEPART(MONTH, ActivityDate) order by 2 DESC) a
	) b
		inner join (select mc.masterID as masterID, mc.masterdesc as masterdesc, count(ps.subscriptionID) as subscriptions,DATEPART(MONTH, ActivityDate) as monthdt, DATEPART(YEAR, ActivityDate) as yeardt
		from SubscriberTopicActivity st 
			join pubsubscriptions ps on st.pubsubscriptionID = ps.pubsubscriptionID
			join (select masterID, masterdesc from mastercodesheet where mastergroupID = @mastergroupID) mc on st.TopicCode = mc.masterdesc 
		where TopicCode is not null 
		group by mc.masterID, mc.masterdesc,DATEPART(MONTH, ActivityDate),  DATEPART(YEAR, ActivityDate) 
		)  c on b.masterID=c.masterID
	) 
	order by 3,6,5

END