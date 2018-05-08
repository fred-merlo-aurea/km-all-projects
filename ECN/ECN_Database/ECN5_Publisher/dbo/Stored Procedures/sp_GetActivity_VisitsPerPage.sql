CREATE Proc [dbo].[sp_GetActivity_VisitsPerPage]
(
	@EditionID int,
	@BlastID int
)
as
Begin
	--SUMMARY
	select	PageNumber, 
			count(distinct EmailID) + count(distinct case when isnull(EmailID,'') = '' then IPaddress end) as 'Unique', 
			count(EAID) as total from editionactivitylog 
	join page on editionactivitylog.pageID = page.PageID
	where editionactivitylog.editionID = @EditionID   and ActionTypecode='Visit' and blastID = (case when @blastID = -1 then blastID else @blastID end)
	group by PageNumber
	order by PageNumber asc, 3 desc

End
