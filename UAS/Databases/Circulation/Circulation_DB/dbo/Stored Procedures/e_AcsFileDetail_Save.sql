﻿create procedure e_AcsFileDetail_Save
@AcsFileDetailId int,
@ClientId int,
@RecordType varchar(1),
@FileVersion varchar(2),
@SequenceNumber int,
@AcsMailerId varchar(9), 
@KeylineSequenceSerialNumber varchar(16),
@MoveEffectiveDate date, 
@MoveType varchar(1), 
@DeliverabilityCode varchar(1), 
@UspsSiteID int,
@LastName varchar(20),
@FirstName varchar(15),
@Prefix varchar(6),
@Suffix varchar(6),
@OldAddressType varchar(1),
@OldUrb varchar(28),
@OldPrimaryNumber varchar(10),
@OldPreDirectional varchar(2),
@OldStreetName varchar(28),
@OldSuffix varchar(4),
@OldPostDirectional varchar(2),
@OldUnitDesignator varchar(4),
@OldSecondaryNumber varchar(10),
@OldCity varchar(28),
@OldStateAbbreviation varchar(2),
@OldZipCode varchar(5),
@NewAddressType varchar(1), 
@NewPmb varchar(8),
@NewUrb varchar(28),
@NewPrimaryNumber varchar(10),
@NewPreDirectional varchar(2),
@NewStreetName varchar(28),
@NewSuffix varchar(4),
@NewPostDirectional varchar(2),
@NewUnitDesignator varchar(4),
@NewSecondaryNumber varchar(10),
@NewCity varchar(28),
@NewStateAbbreviation varchar(2),
@NewZipCode varchar(5),
@Hyphen varchar(1),
@NewPlus4Code varchar(4),
@NewDeliveryPoint varchar(2),
@NewAbbreviatedCityName varchar(13),
@NewAddressLabel varchar(66),
@FeeNotification varchar(1), 
@NotificationType varchar(1), 
@IntelligentMailBarcode varchar(31), 
@IntelligentMailPackageBarcode varchar(35), 
@IdTag varchar(16), 
@HardcopyToElectronicFlag varchar(1),
@TypeOfAcs varchar(1), 
@FulfillmentDate date,
@ProcessingType varchar(1), 
@CaptureType varchar(1),
@MadeAvailableDate date,
@ShapeOfMail varchar(1), 
@MailActionCode varchar(1),
@NixieFlag varchar(1), 
@ProductCode1 int,
@ProductCodeFee1 decimal(4,2),
@ProductCode2 int,
@ProductCodeFee2 decimal(4,2),
@ProductCode3 int,
@ProductCodeFee3 decimal(4,2),
@ProductCode4 int,
@ProductCodeFee4 decimal(4,2),
@ProductCode5 int,
@ProductCodeFee5 decimal(4,2),
@ProductCode6 int,
@ProductCodeFee6 decimal(4,2),
@Filler varchar(405),
@EndMarker varchar(1),
@ProductCode varchar(50),
@OldAddress1 varchar(100),
@OldAddress2 varchar(100),
@OldAddress3 varchar(100),
@NewAddress1 varchar(100),
@NewAddress2 varchar(100),
@NewAddress3 varchar(100),
@SequenceID int,
@TransactionCodeValue int,
@CategoryCodeValue int,
@IsIgnored bit,
@AcsActionId int,
@CreatedDate date,
@CreatedTime time(7),
@ProcessCode varchar(50)
as
	IF @AcsFileDetailId > 0
		BEGIN
			UPDATE AcsFileDetail
			SET ClientId = @ClientId,
				RecordType = @RecordType,
				FileVersion = @FileVersion,
				SequenceNumber = @SequenceNumber,
				AcsMailerId = @AcsMailerId,
				KeylineSequenceSerialNumber = @KeylineSequenceSerialNumber,
				MoveEffectiveDate = @MoveEffectiveDate,
				MoveType = @MoveType,
				DeliverabilityCode = @DeliverabilityCode,
				UspsSiteID = @UspsSiteID,
				LastName = @LastName,
				FirstName = @FirstName,
				Prefix = @Prefix,
				Suffix = @Suffix,
				OldAddressType = @OldAddressType,
				OldUrb = @OldUrb,
				OldPrimaryNumber = @OldPrimaryNumber,
				OldPreDirectional = @OldPreDirectional,
				OldStreetName = @OldStreetName,
				OldSuffix = @OldSuffix,
				OldPostDirectional = @OldPostDirectional,
				OldUnitDesignator = @OldUnitDesignator,
				OldSecondaryNumber = @OldSecondaryNumber,
				OldCity = @OldCity,
				OldStateAbbreviation = @OldStateAbbreviation,
				OldZipCode = @OldZipCode,
				NewAddressType = @NewAddressType,
				NewPmb = @NewPmb,
				NewUrb = @NewUrb,
				NewPrimaryNumber = @NewPrimaryNumber,
				NewPreDirectional = @NewPreDirectional,
				NewStreetName = @NewStreetName,
				NewSuffix = @NewSuffix,
				NewPostDirectional = @NewPostDirectional,
				NewUnitDesignator = @NewUnitDesignator,
				NewSecondaryNumber = @NewSecondaryNumber,
				NewCity = @NewCity,
				NewStateAbbreviation = @NewStateAbbreviation,
				NewZipCode = @NewZipCode,
				Hyphen = @Hyphen,
				NewPlus4Code = @NewPlus4Code,
				NewDeliveryPoint = @NewDeliveryPoint,
				NewAbbreviatedCityName = @NewAbbreviatedCityName,
				NewAddressLabel = @NewAddressLabel,
				FeeNotification = @FeeNotification,
				NotificationType = @NotificationType,
				IntelligentMailBarcode = @IntelligentMailBarcode,
				IntelligentMailPackageBarcode = @IntelligentMailPackageBarcode,
				IdTag = @IdTag,
				HardcopyToElectronicFlag = @HardcopyToElectronicFlag,
				TypeOfAcs = @TypeOfAcs,
				FulfillmentDate = @FulfillmentDate,
				ProcessingType = @ProcessingType,
				CaptureType = @CaptureType,
				MadeAvailableDate = @MadeAvailableDate,
				ShapeOfMail = @ShapeOfMail,
				MailActionCode = @MailActionCode,
				NixieFlag = @NixieFlag,
				ProductCode1 = @ProductCode1,
				ProductCodeFee1 = @ProductCodeFee1,
				ProductCode2 = @ProductCode2,
				ProductCodeFee2 = @ProductCodeFee2,
				ProductCode3 = @ProductCode3,
				ProductCodeFee3 = @ProductCodeFee3,
				ProductCode4 = @ProductCode4,
				ProductCodeFee4 = @ProductCodeFee4,
				ProductCode5 = @ProductCode5,
				ProductCodeFee5 = @ProductCodeFee5,
				ProductCode6 = @ProductCode6,
				ProductCodeFee6 = @ProductCodeFee6,
				Filler = @Filler,
				EndMarker = @EndMarker,
				ProductCode = @ProductCode,
				OldAddress1 = @OldAddress1,
				OldAddress2 = @OldAddress2,
				OldAddress3 = @OldAddress3,
				NewAddress1 = @NewAddress1,
				NewAddress2 = @NewAddress2,
				NewAddress3 = @NewAddress3,
				SequenceID = @SequenceID,
				TransactionCodeValue = @TransactionCodeValue,
				CategoryCodeValue = @CategoryCodeValue,
				IsIgnored = @IsIgnored,
				AcsActionId = @AcsActionId,
				ProcessCode = @ProcessCode
			WHERE AcsFileDetailId = @AcsFileDetailId;

			SELECT @AcsFileDetailId;
		END
	ELSE
		BEGIN
			INSERT INTO AcsFileDetail (ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,
									   UspsSiteID,LastName,FirstName,Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,
									   OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,
									   NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,NewZipCode,Hyphen,NewPlus4Code,
									   NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,
									   IdTag,HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,
									   ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,
									   ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,
									   ProductCode,OldAddress1,OldAddress2,OldAddress3,NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,AcsActionId,CreatedDate,CreatedTime,ProcessCode)
			VALUES(@ClientId,@RecordType,@FileVersion,@SequenceNumber,@AcsMailerId,@KeylineSequenceSerialNumber,@MoveEffectiveDate,@MoveType,@DeliverabilityCode,
				   @UspsSiteID,@LastName,@FirstName,@Prefix,@Suffix,@OldAddressType,@OldUrb,@OldPrimaryNumber,@OldPreDirectional,@OldStreetName,@OldSuffix,@OldPostDirectional,
				   @OldUnitDesignator,@OldSecondaryNumber,@OldCity,@OldStateAbbreviation,@OldZipCode,@NewAddressType,@NewPmb,@NewUrb,@NewPrimaryNumber,@NewPreDirectional,
				   @NewStreetName,@NewSuffix,@NewPostDirectional,@NewUnitDesignator,@NewSecondaryNumber,@NewCity,@NewStateAbbreviation,@NewZipCode,@Hyphen,@NewPlus4Code,
				   @NewDeliveryPoint,@NewAbbreviatedCityName,@NewAddressLabel,@FeeNotification,@NotificationType,@IntelligentMailBarcode,@IntelligentMailPackageBarcode,
				   @IdTag,@HardcopyToElectronicFlag,@TypeOfAcs,@FulfillmentDate,@ProcessingType,@CaptureType,@MadeAvailableDate,@ShapeOfMail,@MailActionCode,@NixieFlag,
				   @ProductCode1,@ProductCodeFee1,@ProductCode2,@ProductCodeFee2,@ProductCode3,@ProductCodeFee3,@ProductCode4,@ProductCodeFee4,@ProductCode5,
				   @ProductCodeFee5,@ProductCode6,@ProductCodeFee6,@Filler,@EndMarker,
				   @ProductCode,@OldAddress1,@OldAddress2,@OldAddress3,@NewAddress1,@NewAddress2,@NewAddress3,@SequenceID,@TransactionCodeValue,@CategoryCodeValue,@IsIgnored,@AcsActionId,@CreatedDate,@CreatedTime,@ProcessCode);SELECT @@IDENTITY;
		END
go

