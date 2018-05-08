CREATE TABLE [dbo].[CompanyGroupMasterValues] (
    [CompanyGroupMasterValuesID] INT              IDENTITY (1, 1) NOT NULL,
    [MasterGroupID]              INT              NULL,
    [CGRP_NO]                    UNIQUEIDENTIFIER NULL,
    [CombinedValues]             VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_CompanyGroupMasterValues] PRIMARY KEY CLUSTERED ([CompanyGroupMasterValuesID] ASC)
);



