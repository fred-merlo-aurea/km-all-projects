CREATE TABLE [dbo].[Access] (
    [AccessID]        INT          IDENTITY (1, 1) NOT NULL,
    [AccessName]      VARCHAR (50) NOT NULL,
    [AccessCode]      VARCHAR (20) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Access_IsActive] DEFAULT ('true') NOT NULL,
    [DateCreated]     DATETIME     CONSTRAINT [DF_Access_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]     DATETIME     NULL,
    [CreatedByUserID] INT          NOT NULL,
    [UpdatedByUserID] INT          NULL,
    CONSTRAINT [PK_Access] PRIMARY KEY CLUSTERED ([AccessID] ASC)
);

