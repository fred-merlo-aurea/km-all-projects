CREATE TABLE [dbo].[Code] (
    [CodeId]          INT           IDENTITY (1, 1) NOT NULL,
    [CodeTypeId]      INT           NOT NULL,
    [CodeName]        VARCHAR (50)  NOT NULL,
    [CodeValue]       VARCHAR (50)  NULL,
    [DisplayName]     VARCHAR (250) NOT NULL,
    [CodeDescription] VARCHAR (MAX) NULL,
    [DisplayOrder]    INT           NULL,
    [HasChildren]     BIT           DEFAULT ('false') NOT NULL,
    [ParentCodeId]    INT           NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    CONSTRAINT [PK_Code] PRIMARY KEY CLUSTERED ([CodeId] ASC)
)