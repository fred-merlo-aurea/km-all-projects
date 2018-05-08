CREATE TABLE [dbo].[DomainTrackerValue] (
    [DomainTrackerValueID]    INT            IDENTITY (1, 1) NOT NULL,
    [DomainTrackerFieldsID]   INT            NOT NULL,
    [DomainTrackerActivityID] INT            NOT NULL,
    [Value]                   VARCHAR (8000) NULL,
    [CreatedDate]             DATETIME       NULL,
    [CreatedUserID]           INT            NULL,
    [UpdatedDate]             DATETIME       NULL,
    [UpdatedUserID]           INT            NULL,
    [IsDeleted]               BIT            NULL,
    CONSTRAINT [PK_DomainTrackerValue] PRIMARY KEY CLUSTERED ([DomainTrackerValueID] ASC),
    CONSTRAINT [FK_DomainTrackerValueDomainTrackerActivityID] FOREIGN KEY ([DomainTrackerActivityID]) REFERENCES [dbo].[DomainTrackerActivity] ([DomainTrackerActivityID]),
    CONSTRAINT [FK_DomainTrackerValueDomainTrackerFieldsID] FOREIGN KEY ([DomainTrackerFieldsID]) REFERENCES [dbo].[DomainTrackerFields] ([DomainTrackerFieldsID])
);




GO
CREATE NONCLUSTERED INDEX [IDX_DomainTrackerValueDomainTrackerFieldsID]
    ON [dbo].[DomainTrackerValue]([DomainTrackerFieldsID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_DomainTrackerValueDomainTrackerActivityID]
    ON [dbo].[DomainTrackerValue]([DomainTrackerActivityID] ASC);

