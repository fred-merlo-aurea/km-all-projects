CREATE PROCEDURE e_AcsShippingDetail_Save
@AcsShippingDetailId int,
@ClientId int,
@CustomerNumber int,
@AcsDate date,
@ShipmentNumber bigint,
@AcsTypeId int,
@AcsId int,
@AcsName varchar(250),
@ProductCode varchar(100),
@Description varchar(250),
@Quantity int,
@UnitCost decimal(8,2),
@TotalCost decimal(12,2),
@DateCreated datetime,
@IsBilled bit,
@BilledDate datetime,
@BilledByUserID int,
@ProcessCode varchar(50)
AS

IF @AcsShippingDetailId > 0
	BEGIN		
		UPDATE AcsShippingDetail
		SET ClientId = @ClientId,
			CustomerNumber = @CustomerNumber,
			AcsDate = @AcsDate,
			ShipmentNumber = @ShipmentNumber,
			AcsTypeId = @AcsTypeId,
			AcsId = @AcsId,
			AcsName = @AcsName,
			ProductCode = @ProductCode,
			Description = @Description,
			Quantity = @Quantity,
			UnitCost = @UnitCost,
			TotalCost = @TotalCost,
			IsBilled = @IsBilled,
			BilledDate = @BilledDate,
			BilledByUserID = @BilledByUserID,
			ProcessCode = @ProcessCode
		WHERE AcsShippingDetailId = @AcsShippingDetailId;
		
		SELECT @AcsShippingDetailId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO AcsShippingDetail (ClientId,CustomerNumber,AcsDate,ShipmentNumber,AcsTypeId,AcsId,AcsName,ProductCode,Description,Quantity,UnitCost,TotalCost,DateCreated,IsBilled,BilledDate,BilledByUserID,ProcessCode)
		VALUES(@ClientId,@CustomerNumber,@AcsDate,@ShipmentNumber,@AcsTypeId,@AcsId,@AcsName,@ProductCode,@Description,@Quantity,@UnitCost,@TotalCost,@DateCreated,@IsBilled,@BilledDate,@BilledByUserID,@ProcessCode);SELECT @@IDENTITY;
	END
go