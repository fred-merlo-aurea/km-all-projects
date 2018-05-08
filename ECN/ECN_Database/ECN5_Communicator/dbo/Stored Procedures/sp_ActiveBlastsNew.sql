CREATE PROCEDURE [dbo].[sp_ActiveBlastsNew]
AS
BEGIN
--select getdate() as 'Time Now'


if exists (select top 1 * from [BLAST] b with (nolock) join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
 where b.SendTime < getDate() and b.StatusCode in ('pending') and ISNULL(blastconfigID,0) = 0 )
Begin
	select ' NO MTA ', * from [BLAST] b with (nolock) join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
 where b.SendTime < getDate() and b.StatusCode in ('pending') and ISNULL(blastconfigID,0) = 0
End

if exists (select top 1 * from [BLAST] with (nolock) where SendTime < getDate() and StatusCode in ('error'))
Begin
	select ' ERROR ', * from [BLAST] with (nolock) where SendTime < getDate() and StatusCode in ('error')
End

if exists (select top 1 * from [BLAST] b with (nolock) left outer join MTACustomer c with (nolock) on b.CustomerID = c.CustomerID
 where b.SendTime < getDate() and b.StatusCode in ('pending') and c.CustomerID is null )
Begin
	select ' NO VIRTUAL MTA ', * from [BLAST] b with (nolock) join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
 where b.SendTime < getDate() and b.StatusCode in ('pending') and c.CustomerID is null
End

if exists (select top 1 * from Blastsingles with (nolock) where processed = 'n' and sendtime < getDate())
	Select * from Blastsingles with (nolock) where processed = 'n' and sendtime < getDate()

if exists (select top 1 * from [BLAST] b with (nolock)
	join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
	join BlastConfig bc with (nolock) on c.BlastConfigID = bc.BlastConfigID
where	b.StatusCode in ('active'))
select b.BlastID, c.basechannelID, b.CustomerID, c.CustomerName, b.BlastEngineID, bc.MTAName, b.EmailFrom,b.EmailSubject, b.SendTime, b.StartTime, b.AttemptTotal, b.SendTotal, b.StatusCode, b.BlastType,
	b.CodeID, b.LayoutID, b.GroupID, b.FilterID, b.Spinlock, b.TestBlast, b.BlastFrequency, b.RefBlastID, b.BlastSuppression, b.FinishTime, b.SuccessTotal, b.CreatedUserID, b.BlastScheduleID, bp.BlastPlanID
from [BLAST] b with (nolock)
	join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
	join BlastConfig bc with (nolock) on c.BlastConfigID = bc.BlastConfigID	
	left join BlastPlans bp with (nolock) on b.BlastID = bp.BlastID
where	b.StatusCode in ('active')
order by b.SendTime asc, b.BlastID asc


if exists (select top 1 * from [BLAST] b with (nolock)
	join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
	join BlastConfig bc with (nolock) on c.BlastConfigID = bc.BlastConfigID
where	b.SendTime < getDate() and b.StatusCode in ('pending'))
select b.BlastID, b.CustomerID, c.CustomerName, b.BlastEngineID, bc.MTAName, b.EmailFrom, b.EmailSubject, b.SendTime, b.StartTime, b.AttemptTotal, b.SendTotal,  b.SendBytes, b.StatusCode, b.BlastType,
	b.CodeID, b.LayoutID, b.GroupID, b.FilterID, b.Spinlock, b.TestBlast, b.BlastFrequency, b.RefBlastID, b.BlastSuppression, b.FinishTime, b.SuccessTotal, b.CreatedUserID, b.BlastScheduleID, bp.BlastPlanID
from [BLAST] b with (nolock)
	join [ECN5_ACCOUNTS].[DBO].[CUSTOMER] c with (nolock) on b.CustomerID = c.CustomerID
	join BlastConfig bc with (nolock) on c.BlastConfigID = bc.BlastConfigID	
	left join BlastPlans bp with (nolock) on b.BlastID = bp.BlastID
where	b.SendTime < getDate() and b.StatusCode in ('pending')
order by b.SendTime asc, b.BlastID asc
END
