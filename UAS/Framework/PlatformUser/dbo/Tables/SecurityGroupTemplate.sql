CREATE TABLE [dbo].[SecurityGroupTemplate] (
    [SecurityGroupTemplateID] INT          IDENTITY (1, 1) NOT NULL,
    [SecurityGroupName]       VARCHAR (50) NOT NULL,
    [IsActive]                BIT          NOT NULL,
    [DateCreated]             DATETIME     CONSTRAINT [DF_SecurityGroupTemplate_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]             DATETIME     NULL,
    [CreatedByUserID]         INT          NOT NULL,
    [UpdatedByUserID]         INT          NULL,
    CONSTRAINT [PK_SecurityGroupTemplate] PRIMARY KEY CLUSTERED ([SecurityGroupTemplateID] ASC)
);

