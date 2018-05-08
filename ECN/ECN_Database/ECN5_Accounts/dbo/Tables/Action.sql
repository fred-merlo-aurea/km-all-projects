CREATE TABLE [dbo].[Action] (
    [ActionID]      INT          IDENTITY (1, 1) NOT NULL,
    [ProductID]     INT          NULL,
    [DisplayName]   VARCHAR (50) NULL,
    [ActionCode]    VARCHAR (50) NULL,
    [DisplayOrder]  INT          CONSTRAINT [DF_Actions_DisplayOrder] DEFAULT (0) NULL,
    [CreatedDate]   DATETIME     NULL,
    [CreatedUserID] INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_Action_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED ([ActionID] ASC) WITH (FILLFACTOR = 80)
);

