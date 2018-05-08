CREATE TABLE [dbo].[Sample] (
    [SampleID]       INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]     INT           NULL,
    [SampleName]     VARCHAR (255) NULL,
    [BlastID]        INT           NULL,
    [CreatedUserID]  INT           NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_Sample_CREATEDDATE] DEFAULT (getdate()) NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_Sample_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]    DATETIME      NULL,
    [UpdatedUserID]  INT           NULL,
    [WinningBlastID] INT           NULL,
    [ABWinnerType] VARCHAR(10) NULL , 
    [DidNotReceiveAB] BIT NULL, 
    [DeliveredOrOpened] VARCHAR(50) NULL, 
    CONSTRAINT [PK_Samples] PRIMARY KEY CLUSTERED ([SampleID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Sample] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Sample] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sample] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Sample] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sample] TO [reader]
    AS [dbo];

