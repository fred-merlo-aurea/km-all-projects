CREATE PROCEDURE [dbo].[e_DataCompareDownloadFilterDetail_Select_DcFilterGroupID]
@dcFilterGroupID int
AS
	begin
		set nocount on
		select d.*
		from DataCompareDownloadFilterDetail d with(nolock)
		where d.dcFilterGroupID = @dcFilterGroupID
	end
