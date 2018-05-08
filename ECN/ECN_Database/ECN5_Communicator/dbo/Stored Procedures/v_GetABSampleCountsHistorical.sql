
CREATE PROCEDURE [dbo].[v_GetABSampleCountsHistorical] 
(  
	@SampleId int
)
as

Begin

SET NOCOUNT ON 

DECLARE
--	@SampleId int = 4967,
	@date datetime ,
	@BlastIDA INT,
	@BlastIDB INT,
	@BlastIDChampion INT,

	@ClicksA INT,
	@ClicksB INT,
	@OpensA INT,
	@OpensB INT,
	@BouncesA INT,
	@BouncesB INT,
	@BlastIDWinning INT,
	@SendToNonWinner INT = 1,
	@Reason VARCHAR(30) = 'Clicks'


SELECT  @Date = (SELECT sendtime from Blast ba where b.SampleID = ba.sampleId and blasttype = 'champion'),
		@BlastIDA = (SELECT MIN(BlastId) from Blast ba where b.SampleID = ba.sampleId and blasttype != 'champion') ,
		@BlastIDB	= (SELECT MAX(BlastId) from Blast ba where b.SampleID = ba.sampleId and blasttype != 'champion') ,
		@BlastIDChampion = (SELECT MIN(BlastId) from Blast ba where b.SampleID = ba.sampleId and blasttype = 'champion') 
from
	Blast b 
	--ChampionBlasts cb on cb.BlastID = b.BlastID  
where 
	b.StatusCode = 'sent'
	and b.sampleId = 	@SampleID 


	create table #Result (
		BlastID int, 
		BounceCount int, 
		Delivered int,
		OpenCount int, 
		ClickCount int, 
		SendTotal int,
		OpenPercent DECIMAL(12,8), 
		ClickPercent DECIMAL(12,8), 
		Winner BIT)  

	INSERT INTO #Result(BlastID)  values (@BlastIDA) ,(@BlastIDB)
		
	UPDATE #Result SET BounceCount = (select ISNULL(count(distinct (emailID)),0) from ECN_Activity..BlastActivityBounces with (nolock) where #Result.BlastID = BlastActivityBounces.BlastID and bouncetime < @Date)
	UPDATE #Result SET OpenCount = (select ISNULL(count(distinct (emailID)),0) from ECN_Activity..BlastActivityOpens with (nolock) where #Result.BlastID = BlastActivityOpens.BlastID and opentime < @Date)
	UPDATE #Result SET ClickCount = (select ISNULL(count(distinct (emailID)),0) from ECN_Activity..BlastActivityClicks with (nolock) where #Result.BlastID = BlastActivityClicks.BlastID and clicktime < @Date)   
	UPDATE #Result SET SendTotal = (select sendtotal from ECN5_Communicator..Blast b with (nolock) where #Result.BlastID = b.BlastID)   

SELECT 	@ClicksA = ClickCount,
		@OpensA = OpenCOunt,
		@BouncesA = BounceCount
FROM
	#Result
WHERE
	BlastId= @BlastIDA

SELECT 	@ClicksB = ClickCount,
		@OpensB = OpenCOunt,
		@BouncesB = BounceCount
FROM
	#Result
WHERE
	BlastId= @BlastIDB



	update #Result
	set Delivered  = SendTotal - BounceCount,
		OpenPercent = CONVERT(DECIMAL(12,8),OpenCount) / (SendTotal - BounceCount),
		ClickPercent = CONVERT(DECIMAL(12,8),ClickCount) / (SendTotal - BounceCount)
	where
		(SendTotal - BounceCount) > 0
	
	update #Result
	set Winner = 1
	where BlastID in (select top 1 blastID from #Result order by (OpenPercent+ClickPercent) desc, BlastID asc)
	

--select * from #Result

SET @BlastIDWinning = (SELECT BlastId FROM #Result WHERE Winner =1)

Insert into ChampionAudit (
	AuditTime,
	SampleID,
	BlastIDA,
	BlastIDB,
	BlastIDChampion,
	ClicksA,
	ClicksB,
	OpensA,
	OpensB,
	BouncesA,
	BouncesB,
	BlastIDWinning,
	SendToNonWinner,
	Reason)
Values
	(
	GETDATE(),
	@SampleID,
	@BlastIDA,
	@BlastIDB,
	@BlastIDChampion,

	@ClicksA,
	@ClicksB,
	@OpensA,
	@OpensB,
	@BouncesA,
	@BouncesB,
	@BlastIDWinning,
	@SendToNonWinner,
	@Reason
	)
drop table #Result

END