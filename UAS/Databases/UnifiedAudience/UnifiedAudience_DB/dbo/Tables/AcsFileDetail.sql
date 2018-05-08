﻿CREATE TABLE [dbo].[AcsFileDetail] (
    [AcsFileDetailId]               INT            IDENTITY (1, 1) NOT NULL,
    [RecordType]                    VARCHAR (1)    NULL,
    [FileVersion]                   VARCHAR (2)    NULL,
    [SequenceNumber]                INT            NULL,
    [AcsMailerId]                   VARCHAR (9)    NULL,
    [KeylineSequenceSerialNumber]   VARCHAR (16)   NULL,
    [MoveEffectiveDate]             DATE           NULL,
    [MoveType]                      VARCHAR (1)    NULL,
    [DeliverabilityCode]            VARCHAR (1)    NULL,
    [UspsSiteID]                    INT            NULL,
    [LastName]                      VARCHAR (20)   NULL,
    [FirstName]                     VARCHAR (15)   NULL,
    [Prefix]                        VARCHAR (6)    NULL,
    [Suffix]                        VARCHAR (6)    NULL,
    [OldAddressType]                VARCHAR (1)    NULL,
    [OldUrb]                        VARCHAR (28)   NULL,
    [OldPrimaryNumber]              VARCHAR (10)   NULL,
    [OldPreDirectional]             VARCHAR (2)    NULL,
    [OldStreetName]                 VARCHAR (28)   NULL,
    [OldSuffix]                     VARCHAR (4)    NULL,
    [OldPostDirectional]            VARCHAR (2)    NULL,
    [OldUnitDesignator]             VARCHAR (4)    NULL,
    [OldSecondaryNumber]            VARCHAR (10)   NULL,
    [OldCity]                       VARCHAR (28)   NULL,
    [OldStateAbbreviation]          VARCHAR (2)    NULL,
    [OldZipCode]                    VARCHAR (5)    NULL,
    [NewAddressType]                VARCHAR (1)    NULL,
    [NewPmb]                        VARCHAR (8)    NULL,
    [NewUrb]                        VARCHAR (28)   NULL,
    [NewPrimaryNumber]              VARCHAR (10)   NULL,
    [NewPreDirectional]             VARCHAR (2)    NULL,
    [NewStreetName]                 VARCHAR (28)   NULL,
    [NewSuffix]                     VARCHAR (4)    NULL,
    [NewPostDirectional]            VARCHAR (2)    NULL,
    [NewUnitDesignator]             VARCHAR (4)    NULL,
    [NewSecondaryNumber]            VARCHAR (10)   NULL,
    [NewCity]                       VARCHAR (28)   NULL,
    [NewStateAbbreviation]          VARCHAR (2)    NULL,
    [NewZipCode]                    VARCHAR (5)    NULL,
    [Hyphen]                        VARCHAR (1)    NULL,
    [NewPlus4Code]                  VARCHAR (4)    NULL,
    [NewDeliveryPoint]              VARCHAR (2)    NULL,
    [NewAbbreviatedCityName]        VARCHAR (13)   NULL,
    [NewAddressLabel]               VARCHAR (66)   NULL,
    [FeeNotification]               VARCHAR (1)    NULL,
    [NotificationType]              VARCHAR (1)    NULL,
    [IntelligentMailBarcode]        VARCHAR (31)   NULL,
    [IntelligentMailPackageBarcode] VARCHAR (35)   NULL,
    [IdTag]                         VARCHAR (16)   NULL,
    [HardcopyToElectronicFlag]      VARCHAR (1)    NULL,
    [TypeOfAcs]                     VARCHAR (1)    NULL,
    [FulfillmentDate]               DATE           NULL,
    [ProcessingType]                VARCHAR (1)    NULL,
    [CaptureType]                   VARCHAR (1)    NULL,
    [MadeAvailableDate]             DATE           NULL,
    [ShapeOfMail]                   VARCHAR (1)    NULL,
    [MailActionCode]                VARCHAR (1)    NULL,
    [NixieFlag]                     VARCHAR (1)    NULL,
    [ProductCode1]                  INT            NULL,
    [ProductCodeFee1]               DECIMAL (4, 2) NULL,
    [ProductCode2]                  INT            NULL,
    [ProductCodeFee2]               DECIMAL (4, 2) NULL,
    [ProductCode3]                  INT            NULL,
    [ProductCodeFee3]               DECIMAL (4, 2) NULL,
    [ProductCode4]                  INT            NULL,
    [ProductCodeFee4]               DECIMAL (4, 2) NULL,
    [ProductCode5]                  INT            NULL,
    [ProductCodeFee5]               DECIMAL (4, 2) NULL,
    [ProductCode6]                  INT            NULL,
    [ProductCodeFee6]               DECIMAL (4, 2) NULL,
    [Filler]                        VARCHAR (405)  NULL,
    [EndMarker]                     VARCHAR (1)    NULL,
    [ProductCode]                   VARCHAR (50)   NULL,
    [OldAddress1]                   VARCHAR (100)  NULL,
    [OldAddress2]                   VARCHAR (100)  NULL,
    [OldAddress3]                   VARCHAR (100)  NULL,
    [NewAddress1]                   VARCHAR (100)  NULL,
    [NewAddress2]                   VARCHAR (100)  NULL,
    [NewAddress3]                   VARCHAR (100)  NULL,
    [SequenceID]                    INT            NULL,
    [TransactionCodeValue]          INT            NULL,
    [CategoryCodeValue]             INT            NULL,
    [IsIgnored]                     BIT            DEFAULT ('false') NULL,
    [AcsActionId]                   INT            NOT NULL,
    [CreatedDate]                   DATE           NULL,
    [CreatedTime]                   TIME (7)       NULL,
    [ProcessCode]                   VARCHAR (50)   NULL,
    CONSTRAINT [PK_AcsFileDetail_AcsFileDetailId] PRIMARY KEY CLUSTERED ([AcsFileDetailId] ASC) WITH (FILLFACTOR = 90)
);