CREATE TABLE [dbo].[Staff] (
    [StaffID]           INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]     INT           NOT NULL,
    [FirstName]         VARCHAR (50)  NOT NULL,
    [LastName]          VARCHAR (50)  NOT NULL,
    [Email]             VARCHAR (100) NOT NULL,
    [Roles]             SMALLINT      NOT NULL,
    [UserID]            INT           CONSTRAINT [DF_Staff_UserID] DEFAULT (0) NOT NULL,
    [LicenseUpdateFlag] BIT           NULL,
    [FeatureUpdateFlag] BIT           CONSTRAINT [DF_Staff_FeatureUpdateFlag] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED ([StaffID] ASC) WITH (FILLFACTOR = 80)
);

