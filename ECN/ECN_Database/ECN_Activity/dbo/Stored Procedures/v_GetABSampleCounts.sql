


--this is to get counts from A/B for audit
CREATE PROCEDURE [dbo].[v_GetABSampleCounts] 
(  
	@BlastA int,
	@BlastB int
)
as
Begin

	create table #Result (BlastID int, BounceCount int, OpenCount int, ClickCount int)

	INSERT INTO #Result(BlastID) SELECT @BlastA
	INSERT INTO #Result(BlastID) SELECT @BlastB
	
	UPDATE #Result SET BounceCount = (select ISNULL(count(distinct (emailID)),0) from BlastActivityBounces with (nolock) where #Result.BlastID = BlastActivityBounces.BlastID)
	UPDATE #Result SET OpenCount = (select ISNULL(count(distinct (emailID)),0) from BlastActivityOpens with (nolock) where #Result.BlastID = BlastActivityOpens.BlastID)
	UPDATE #Result SET ClickCount = (select ISNULL(count(distinct (emailID)),0) from BlastActivityClicks with (nolock) where #Result.BlastID = BlastActivityClicks.BlastID)   
	 
	select * from #Result
	drop table #Result
END