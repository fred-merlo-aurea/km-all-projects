CREATE TABLE [dbo].[ConversionLinks] (
    [LinkID]        INT            IDENTITY (1, 1) NOT NULL,
    [LayoutID]      INT            NULL,
    [LinkURL]       VARCHAR (2048) NULL,
    [LinkParams]    VARCHAR (255)  NULL,
    [LinkName]      VARCHAR (255)  NULL,
    [IsActive]      CHAR (1)       NULL,
    [SortOrder]     INT            NULL,
    [CreatedDate]   DATETIME       CONSTRAINT [DF_ConversionLinks_DateAdded] DEFAULT (getdate()) NULL,
    [UpdatedDate]   DATETIME       CONSTRAINT [DF_ConversionLinks_DateUpdated] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT            NULL,
    [IsDeleted]     BIT            CONSTRAINT [DF_ConversionLinks_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID] INT            NULL,
    CONSTRAINT [PK_ConversionLinks] PRIMARY KEY CLUSTERED ([LinkID] ASC) WITH (FILLFACTOR = 80)
);

