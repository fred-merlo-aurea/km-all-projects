CREATE PROCEDURE [dbo].[job_Telemarketing_MatchSequenceUpdateIGrpNo_ProcessCode]
	@ProcessCode varchar(50),
	@FileType varchar(50)
as
BEGIN

	SET NOCOUNT ON

	--Update Subscriber Final record IGrp_No Column in match on Sequence, Pub, FName, LName
	Update SF
		set SF.IGrp_No = S.IGrp_No
		from SubscriberFinal SF 
			join Pubs PUB WITH(NOLOCK) on PUB.PubCode = SF.PubCode 
			join PubSubscriptions PS WITH(NOLOCK) on PS.SequenceID = SF.Sequence and PS.PubID = PUB.PubID
			join Subscriptions S WITH(NOLOCK) on PS.SubscriptionID = S.SubscriptionID
		where SF.ProcessCode = @ProcessCode and ISNULL(SF.Sequence,0) > 0
			and (PS.FirstNAME = SF.FNAME and PS.LastNAME = SF.LNAME)

END