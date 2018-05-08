CREATE TABLE [dbo].[CompanyGroupDetails] (
    [CompanyGroupDetailsID] INT              IDENTITY (1, 1) NOT NULL,
    [CGRP_NO]               UNIQUEIDENTIFIER NULL,
    [MasterId]              INT              NULL,
    CONSTRAINT [PK_CompanyGroupDetails] PRIMARY KEY CLUSTERED ([CompanyGroupDetailsID] ASC)
);



