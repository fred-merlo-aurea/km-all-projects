CREATE TABLE [dbo].[EmailQueue] (
    [EmailQueueID] INT             IDENTITY (1, 1) NOT NULL,
    [BlastID]      INT             NOT NULL,
    [EmailID]      INT             NOT NULL,
    [FromAddress]  VARCHAR (800)   NOT NULL,
    [ToAddress]    VARCHAR (100)   NOT NULL,
    [MessageBytes] VARBINARY (MAX) NOT NULL,
    [Processed]    BIT             CONSTRAINT [DF_EmailQueue_Processed] DEFAULT ((0)) NOT NULL,
    [CreatedDate]  DATETIME        CONSTRAINT [DF_EmailQueue_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED ([EmailQueueID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_EmailQueue_BlastID_EmailID]
    ON [dbo].[EmailQueue]([BlastID] ASC, [EmailID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmailQueue_BlastID]
    ON [dbo].[EmailQueue]([BlastID] ASC);

