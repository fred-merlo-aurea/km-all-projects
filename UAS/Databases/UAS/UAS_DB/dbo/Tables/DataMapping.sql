CREATE TABLE [dbo].[DataMapping] (
    [DataMappingID]   INT           IDENTITY (1, 1) NOT NULL,
    [FieldMapping]    INT           NOT NULL,
    [IncomingValue]   VARCHAR (100) NOT NULL,
    [MAFValue]        VARCHAR (100) NULL,
    [Ignore]          BIT           CONSTRAINT [DF_DataMapping_Ignore] DEFAULT ((0)) NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT      NULL,
    CONSTRAINT [PK_DataMapping] PRIMARY KEY CLUSTERED ([DataMappingID] ASC),
    CONSTRAINT [FK_DataMapping_FieldMapping] FOREIGN KEY ([FieldMapping]) REFERENCES [dbo].[FieldMapping] ([FieldMappingID])
);

