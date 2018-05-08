CREATE TABLE [dbo].[AutoResponders] (
    [AutoResponderID] INT           IDENTITY (1, 1) NOT NULL,
    [MailServer]      VARCHAR (255) NULL,
    [AccountName]     VARCHAR (255) NULL,
    [AccountPasswd]   VARCHAR (255) NULL,
    [ForwardTo]       VARCHAR (255) NULL,
    [BlastID]         INT           NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_AutoResponders_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]   INT           NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_AutoResponders_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]     DATETIME      NULL,
    [UpdatedUserID]   INT           NULL,
    CONSTRAINT [PK_AutoResponders] PRIMARY KEY CLUSTERED ([AutoResponderID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[AutoResponders] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[AutoResponders] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AutoResponders] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[AutoResponders] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AutoResponders] TO [reader]
    AS [dbo];

