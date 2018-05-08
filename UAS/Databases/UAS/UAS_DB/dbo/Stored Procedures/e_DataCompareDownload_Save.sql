create procedure e_DataCompareDownload_Save
@DcDownloadId int,
@DcViewId int,
@WhereClause varchar(max) = null,
@ProfileColumns varchar(max) = null,
@DimensionColumns varchar(max) = null,
@DcTypeCodeId int,
@ProfileCount int = null,
@TotalItemCount int = null,
@TotalBilledCost decimal(19,4) = null,
@TotalThirdPartyCost  decimal(19,4) = null,
@IsPurchased bit = 'false',
@PurchasedByUserId int = null,
@PurchasedDate datetime = null,
@PurchasedCaptcha varchar(50) = null,
@IsBilled bit = 'false',
@BilledDate datetime = null,
@DownloadFileName varchar(100) = null,
@CreatedByUserId int = 0,
@DateCreated datetime = null
as
	begin
		set nocount on
		if(@DcDownloadId > 0)
			begin
				update DataCompareDownload
				set 
					WhereClause = @WhereClause,
					--ProfileColumns = @ProfileColumns,
					--DimensionColumns = @DimensionColumns,
					DcTypeCodeId = @DcTypeCodeId,
					ProfileCount = @ProfileCount,
					TotalItemCount = @TotalItemCount,
					TotalBilledCost = @TotalBilledCost,
					TotalThirdPartyCost = @TotalThirdPartyCost,
					IsPurchased = @IsPurchased,
					PurchasedByUserId = @PurchasedByUserId,
					PurchasedDate = @PurchasedDate,
					PurchasedCaptcha = @PurchasedCaptcha,
					IsBilled = @IsBilled,
					BilledDate = @BilledDate,
					DownloadFileName = @DownloadFileName
				where DcDownloadId = @DcDownloadId

				select @DcDownloadId;
			end
		else
			begin
				if(@DateCreated = null)
					set @DateCreated = getdate();

				insert into DataCompareDownload (DcViewId,WhereClause,DcTypeCodeId,ProfileCount,TotalItemCount,TotalBilledCost,TotalThirdPartyCost,
				 IsPurchased,PurchasedByUserId,PurchasedDate,PurchasedCaptcha,IsBilled,BilledDate,DownloadFileName)--CreatedByUserId,DateCreated,
				values(@DcViewId,@WhereClause,@DcTypeCodeId,@ProfileCount,@TotalItemCount,@TotalBilledCost,@TotalThirdPartyCost,
				 @IsPurchased,@PurchasedByUserId,@PurchasedDate,@PurchasedCaptcha,@IsBilled,@BilledDate,@DownloadFileName);select @@IDENTITY;--,@CreatedByUserId,@DateCreated
			end
	end
go

