CREATE TABLE [dbo].[BlastEnvelopes] (
    [BlastEnvelopeID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]      INT           NOT NULL,
    [FromName]        VARCHAR (100) NULL,
    [FromEmail]       VARCHAR (100) NULL,
    [CreatedUserID]   INT           NOT NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_BlastEnvelopes_DateAdded] DEFAULT (getdate()) NOT NULL,
    [UpdatedUserID]   INT           NOT NULL,
    [UpdatedDate]     DATETIME      CONSTRAINT [DF_BlastEnvelopes_DateModified] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_BlastEnvelopes_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_BlastEnvelopes] PRIMARY KEY CLUSTERED ([BlastEnvelopeID] ASC) WITH (FILLFACTOR = 100, STATISTICS_NORECOMPUTE = ON)
);

