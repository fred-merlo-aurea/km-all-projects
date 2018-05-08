CREATE TABLE [dbo].[SimplyAudio_GiftCodes] (
    [GiftCodeID]    INT          IDENTITY (1, 1) NOT NULL,
    [GiftCodeValue] VARCHAR (50) CONSTRAINT [DF_SimplyAudio_GiftCodes_GiftCodeValue] DEFAULT ('') NULL,
    [Processed]     CHAR (1)     CONSTRAINT [DF_SimplyAudio_GiftCodes_Processed] DEFAULT ('N') NULL,
    [DateAdded]     DATETIME     CONSTRAINT [DF_SimplyAudio_GiftCodes_DateAdded] DEFAULT (getdate()) NULL,
    [DateUpdated]   DATETIME     NULL,
    CONSTRAINT [PK_SimplyAudio_GiftCodes] PRIMARY KEY CLUSTERED ([GiftCodeID] ASC) WITH (FILLFACTOR = 80)
);

