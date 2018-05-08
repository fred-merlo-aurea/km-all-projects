CREATE PROCEDURE [dbo].[e_DataCompareView_Save]
@DcViewId int,
@DcRunId int,
@DcTargetCodeId int,
@DcTargetIdUad int = null,
@UadNetCount int = 0,
@MatchedCount int = 0,
@NoDataCount int = 0,
@Cost int = 0,
@DateCreated datetime,
@DateUpdated datetime = null,
@CreatedByUserID int,
@UpdatedByUserID int = null,
@IsBillable bit = 'true',
@Notes varchar(max) = null,
@PaymentStatusId int = null,
@PaidDate datetime = null,
@DcTypeCodeId int = 0
AS
	begin
		set nocount on
		if @DcViewId > 0
			begin
			
				UPDATE DataCompareView
				SET DcTargetCodeId = @DcTargetCodeId,
					DcTargetIdUad = @DcTargetIdUad,
					UadNetCount = @UadNetCount,
					MatchedCount = @MatchedCount,
					NoDataCount = @NoDataCount,
					Cost = @Cost,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID,
					IsBillable = @IsBillable,
				    Notes = @Notes,
					PaymentStatusId = @PaymentStatusId,
					PaidDate = @PaidDate,
					DcTypeCodeId = @DcTypeCodeId
				WHERE DcViewId = @DcViewId;
		
				SELECT @DcViewId;
			end
		else
			begin
				IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
				INSERT INTO DataCompareView (DcRunId,DcTargetCodeId,DcTargetIdUad,UadNetCount,MatchedCount,NoDataCount,Cost,DateCreated,CreatedByUserID,IsBillable,Notes,PaymentStatusId,PaidDate,DcTypeCodeId)
				VALUES(@DcRunId,@DcTargetCodeId,@DcTargetIdUad,@UadNetCount,@MatchedCount,@NoDataCount,@Cost,@DateCreated,@CreatedByUserID,@IsBillable,@Notes,@PaymentStatusId,@PaidDate,@DcTypeCodeId);SELECT @@IDENTITY;
			end
	end
go
