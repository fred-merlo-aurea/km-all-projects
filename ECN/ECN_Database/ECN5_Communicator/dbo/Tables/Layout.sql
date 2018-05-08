CREATE TABLE [dbo].[Layout] (
    [LayoutID]       INT           IDENTITY (1, 1) NOT NULL,
    [TemplateID]     INT           NULL,
    [CustomerID]     INT           NULL,
    [FolderID]       INT           CONSTRAINT [DF_Layouts_FolderID] DEFAULT (0) NULL,
    [LayoutName]     VARCHAR (50)  NULL,
    [ContentSlot1]   INT           NULL,
    [ContentSlot2]   INT           NULL,
    [ContentSlot3]   INT           NULL,
    [ContentSlot4]   INT           NULL,
    [ContentSlot5]   INT           NULL,
    [ContentSlot6]   INT           NULL,
    [ContentSlot7]   INT           NULL,
    [ContentSlot8]   INT           NULL,
    [ContentSlot9]   INT           NULL,
    [CreatedUserID]  INT           NULL,
    [UpdatedDate]    DATETIME      CONSTRAINT [DF_Layout_UpdatedDate] DEFAULT (getdate()) NULL,
    [TableOptions]   VARCHAR (255) NULL,
    [DisplayAddress] VARCHAR (255) NULL,
    [SetupCost]      VARCHAR (50)  CONSTRAINT [DF_Layouts_SetupFees] DEFAULT (0) NULL,
    [OutboundCost]   VARCHAR (50)  CONSTRAINT [DF_Layouts_OutboundCost] DEFAULT (0) NULL,
    [InboundCost]    VARCHAR (50)  CONSTRAINT [DF_Layouts_InboundCost] DEFAULT (0) NULL,
    [DesignCost]     VARCHAR (50)  CONSTRAINT [DF_Layouts_DesignCost] DEFAULT (0) NULL,
    [OtherCost]      VARCHAR (50)  CONSTRAINT [DF_Layouts_OtherCost] DEFAULT (0) NULL,
    [MessageTypeID]  INT           NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_Layout_CREATEDDATE] DEFAULT (getdate()) NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_Layout_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID]  INT           NULL,
    [Archived] BIT NULL, 
    CONSTRAINT [Layouts_PK] PRIMARY KEY CLUSTERED ([LayoutID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Layouts_TemplateID] FOREIGN KEY ([TemplateID]) REFERENCES [dbo].[Template] ([TemplateID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Layouts_CustomerID_1]
    ON [dbo].[Layout]([CustomerID] ASC)
    INCLUDE([ContentSlot1], [ContentSlot2], [ContentSlot3], [ContentSlot4], [ContentSlot5], [ContentSlot6], [ContentSlot7], [ContentSlot8], [ContentSlot9]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_Layouts_FolderID]
    ON [dbo].[Layout]([FolderID] ASC)
    INCLUDE([LayoutID]) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Layout] TO [reader]
    AS [dbo];

