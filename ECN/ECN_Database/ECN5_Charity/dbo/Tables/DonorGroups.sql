CREATE TABLE [dbo].[DonorGroups] (
    [DonorGroupID] INT IDENTITY (1, 1) NOT NULL,
    [CustomerID]   INT NOT NULL,
    [GroupID]      INT NULL,
    CONSTRAINT [PK_DonorGroups] PRIMARY KEY CLUSTERED ([DonorGroupID] ASC) WITH (FILLFACTOR = 80)
);

