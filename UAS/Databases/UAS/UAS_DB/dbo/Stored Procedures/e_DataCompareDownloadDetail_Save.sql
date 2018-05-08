CREATE PROCEDURE [dbo].[e_DataCompareDownloadDetail_Save]
@DcDownloadId int,
@SubscriptionIds XML = null
AS
	begin
		set nocount on

		insert into DataCompareDownloadDetail(DcDownloadId, SubscriptionId)
        select @DcDownloadId, T.C.value('.', 'int') from @SubscriptionIDs.nodes('/SubcriptionIDs/SubcriptionID') as T(C)

	end