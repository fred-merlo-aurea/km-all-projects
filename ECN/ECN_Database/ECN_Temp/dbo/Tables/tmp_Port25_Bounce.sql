CREATE TABLE [dbo].[tmp_Port25_Bounce] (
    [ID]            INT           NOT NULL,
    [EmailID]       INT           NULL,
    [BlastID]       INT           NULL,
    [BounceMessage] VARCHAR (260) NULL,
    [BounceCode]    VARCHAR (50)  NULL,
    [TimeQueued]    DATETIME      NULL,
    [IsExists]      BIT           CONSTRAINT [DF_tmp_Port25_Bounce_IsExists] DEFAULT ((0)) NOT NULL,
    [Processed]     BIT           CONSTRAINT [DF_tmp_Port25_Bounce_Processed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tmp_Port25_Bounce] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_tmp_Port25_Bounce_blastID_EmailID]
    ON [dbo].[tmp_Port25_Bounce]([BlastID] ASC, [EmailID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_tmp_Port25_Bounce]
    ON [dbo].[tmp_Port25_Bounce]([BounceMessage] ASC);

