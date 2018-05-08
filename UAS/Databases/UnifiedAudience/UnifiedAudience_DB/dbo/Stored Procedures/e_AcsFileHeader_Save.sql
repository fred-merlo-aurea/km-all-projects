create procedure e_AcsFileHeader_Save
@AcsFileHeaderId int,
@RecordType varchar(1),
@FileVersion varchar(2),
@CustomerID int,
@CreateDate date,
@ShipmentNumber bigint,
@TotalAcsRecordCount int,
@TotalCoaCount int,
@TotalNixieCount int,
@TrdRecordCount int,
@TrdAcsFeeAmount decimal(9,2),
@TrdCoaCount int,
@TrdCoaAcsFeeAmount decimal(9,2),
@TrdNixieCount int,
@TrdNixieAcsFeeAmount decimal(9,2),
@OcdRecordCount int,
@OcdAcsFeeAmount decimal(9,2),
@OcdCoaCount int,
@OcdCoaAcsFeeAmount decimal(9,2),
@OcdNixieCount int,
@OcdNixieAcsFeeAmount decimal(9,2),
@FsRecordCount int,
@FsAcsFeeAmount decimal(9,2),
@FsCoaCount int,
@FsCoaAcsFeeAmount decimal(9,2),
@FsNixieCount int,
@FsNixieAcsFeeAmount decimal(9,2),
@ImpbRecordCount int,
@ImpbAcsFeeAmount decimal(9,2),
@ImpbCoaCount int,
@ImpbCoaAcsFeeAmount decimal(9,2),
@ImpbNixieCount int,
@ImpbNixieAcsFeeAmount decimal(9,2),
@Filler varchar(405),
@EndMarker varchar(1),
@ProcessCode varchar(50)
as
BEGIN

	set nocount on

	IF @AcsFileHeaderId > 0
		BEGIN
			UPDATE AcsFileHeader
			SET 
				RecordType = @RecordType,
				FileVersion = @FileVersion,
				CustomerID = @CustomerID,
				CreateDate = @CreateDate,
				ShipmentNumber = @ShipmentNumber,
				TotalAcsRecordCount = @TotalAcsRecordCount,
				TotalCoaCount = @TotalCoaCount,
				TotalNixieCount = @TotalNixieCount,
				TrdRecordCount = @TrdRecordCount,
				TrdAcsFeeAmount = @TrdAcsFeeAmount,
				TrdCoaCount = @TrdCoaCount,
				TrdCoaAcsFeeAmount = @TrdCoaAcsFeeAmount,
				TrdNixieCount = @TrdNixieCount,
				TrdNixieAcsFeeAmount = @TrdNixieAcsFeeAmount,
				OcdRecordCount = @OcdRecordCount,
				OcdAcsFeeAmount = @OcdAcsFeeAmount,
				OcdCoaCount = @OcdCoaCount,
				OcdCoaAcsFeeAmount = @OcdCoaAcsFeeAmount,
				OcdNixieCount = @OcdNixieCount,
				OcdNixieAcsFeeAmount = @OcdNixieAcsFeeAmount,
				FsRecordCount = @FsRecordCount,
				FsAcsFeeAmount = @FsAcsFeeAmount,
				FsCoaCount = @FsCoaCount,
				FsCoaAcsFeeAmount = @FsCoaAcsFeeAmount,
				FsNixieCount = @FsNixieCount,
				FsNixieAcsFeeAmount = @FsNixieAcsFeeAmount,
				ImpbRecordCount = @ImpbRecordCount,
				ImpbAcsFeeAmount = @ImpbAcsFeeAmount,
				ImpbCoaCount = @ImpbCoaCount,
				ImpbCoaAcsFeeAmount = @ImpbCoaAcsFeeAmount,
				ImpbNixieCount = @ImpbNixieCount,
				ImpbNixieAcsFeeAmount = @ImpbNixieAcsFeeAmount,
				Filler = @Filler,
				EndMarker = @EndMarker,
				ProcessCode = @ProcessCode
			WHERE AcsFileHeaderId = @AcsFileHeaderId;

			SELECT @AcsFileHeaderId;
		END
	ELSE
		BEGIN
			INSERT INTO AcsFileHeader (RecordType,FileVersion,CustomerID,CreateDate,ShipmentNumber,TotalAcsRecordCount,TotalCoaCount,TotalNixieCount,TrdRecordCount,
									   TrdAcsFeeAmount,TrdCoaCount,TrdCoaAcsFeeAmount,TrdNixieCount,TrdNixieAcsFeeAmount,OcdRecordCount,OcdAcsFeeAmount,OcdCoaCount,
									   OcdCoaAcsFeeAmount,OcdNixieCount,OcdNixieAcsFeeAmount,FsRecordCount,FsAcsFeeAmount,FsCoaCount,FsCoaAcsFeeAmount,FsNixieCount,
									   FsNixieAcsFeeAmount,ImpbRecordCount,ImpbAcsFeeAmount,ImpbCoaCount,ImpbCoaAcsFeeAmount,ImpbNixieCount,ImpbNixieAcsFeeAmount,
									   Filler,EndMarker,ProcessCode)
			VALUES(@RecordType,@FileVersion,@CustomerID,@CreateDate,@ShipmentNumber,@TotalAcsRecordCount,@TotalCoaCount,@TotalNixieCount,@TrdRecordCount,
				   @TrdAcsFeeAmount,@TrdCoaCount,@TrdCoaAcsFeeAmount,@TrdNixieCount,@TrdNixieAcsFeeAmount,@OcdRecordCount,@OcdAcsFeeAmount,@OcdCoaCount,
				   @OcdCoaAcsFeeAmount,@OcdNixieCount,@OcdNixieAcsFeeAmount,@FsRecordCount,@FsAcsFeeAmount,@FsCoaCount,@FsCoaAcsFeeAmount,@FsNixieCount,
				   @FsNixieAcsFeeAmount,@ImpbRecordCount,@ImpbAcsFeeAmount,@ImpbCoaCount,@ImpbCoaAcsFeeAmount,@ImpbNixieCount,@ImpbNixieAcsFeeAmount,
				   @Filler,@EndMarker,@ProcessCode);SELECT @@IDENTITY;
		END

END
go