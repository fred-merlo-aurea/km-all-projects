create PROCEDURE [dbo].[e_FilterPenetrationReportsDetails_Save]
(
	@ReportID int,
	@FilterID int
)
AS
BEGIN

	SET NOCOUNT ON
	
	Begin
		insert into FilterPenetrationReportsDetails(ReportID, FilterID) values(@ReportID, @FilterID);
		select @@IDENTITY;
	End
END