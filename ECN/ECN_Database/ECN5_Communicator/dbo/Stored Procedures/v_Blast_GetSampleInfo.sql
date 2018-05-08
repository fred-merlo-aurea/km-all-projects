CREATE PROCEDURE [dbo].[v_Blast_GetSampleInfo] 
(
@SampleID int,
@CustomerID int
)
AS

Select 
	b.BlastID, b.EmailFrom, b.EmailFromName, b.ReplyTo, isnull(b.codeID,0) as CodeID, isnull(b.filterID,0) as FilterID, 
	isnull(b.GroupID,0) as GroupID, isnull(AddOptOuts_to_MS,0) as mastersuppress , isnull(BlastSuppression,'') as BlastSuppression, 
	GroupName, isnull(FilterName,'') as FilterName, isnull(s.ABWinnerType,'') as ABWinnerType
from 
	Blast b with (NOLOCK) 
	join Groups g with (NOLOCK) on b.GroupID = g.GroupID 
	left outer join Filter f with (NOLOCK) on b.FilterID = f.FilterID 
	join ecn5_communicator..Sample s on b.SampleID = s.SampleID
where 
	b.SampleID = @SampleID and
	b.CustomerID = @CustomerID and
	b.BlastType = 'Sample' and
	b.StatusCode <> 'deleted'

