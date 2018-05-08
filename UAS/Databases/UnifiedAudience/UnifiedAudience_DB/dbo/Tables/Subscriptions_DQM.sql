﻿CREATE TABLE [dbo].[Subscriptions_DQM] (
    [DQMID]                   INT              IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]          INT              NOT NULL,
    [ZZ_MAF_ACTION]           VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_STD]        VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_MATCH1]     VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_MATCH2]     VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_MATCH3]     VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_MATCH4]     VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_MATCH5]     VARCHAR (1024)   NULL,
    [ZZ_PAR_FNAME_MATCH6]     VARCHAR (1024)   NULL,
    [ZZ_PAR_LNAME_STD]        VARCHAR (1024)   NULL,
    [ZZ_PAR_TITLE_STD]        VARCHAR (1024)   NULL,
    [ZZ_PAR_COMPANY_STD]      VARCHAR (1024)   NULL,
    [ZZ_PAR_COMPANY2]         VARCHAR (1024)   NULL,
    [ZZ_PAR_COMPANY_MATCH1]   VARCHAR (1024)   NULL,
    [ZZ_PAR_COMPANY_MATCH2]   VARCHAR (1024)   NULL,
    [ZZ_PAR_ADDRESS_STD]      VARCHAR (1024)   NULL,
    [ZZ_PAR_MAILSTOP_STD]     VARCHAR (1024)   NULL,
    [ZZ_PAR_CITY_STD]         VARCHAR (1024)   NULL,
    [ZZ_PAR_STATE_STD]        VARCHAR (1024)   NULL,
    [ZZ_PAR_ZIP_STD]          VARCHAR (1024)   NULL,
    [ZZ_PAR_PLUS4_STD]        VARCHAR (1024)   NULL,
    [ZZ_PAR_FORZIP_STD]       VARCHAR (1024)   NULL,
    [ZZ_PAR_POSTCODE]         VARCHAR (1024)   NULL,
    [ZZ_PAR_EMAIL_STD]        VARCHAR (1024)   NULL,
    [ZZ_PAR_USCAN_PHONE]      VARCHAR (1024)   NULL,
    [ZZ_PAR_INTL_PHONE]       VARCHAR (1024)   NULL,
    [ZZ_PAR_PRIMARY_NUMBER]   VARCHAR (1024)   NULL,
    [ZZ_PAR_PRIMARY_PREFIX]   VARCHAR (1024)   NULL,
    [ZZ_PAR_PRIMARY_STREET]   VARCHAR (1024)   NULL,
    [ZZ_PAR_PRIMARY_TYPE]     VARCHAR (1024)   NULL,
    [ZZ_PAR_PRIMARY_POSTFIX]  VARCHAR (1024)   NULL,
    [ZZ_PAR_RR_BOX]           VARCHAR (1024)   NULL,
    [ZZ_PAR_RR_NUMBER]        VARCHAR (1024)   NULL,
    [ZZ_PAR_UNIT_DESCRIPTION] VARCHAR (1024)   NULL,
    [ZZ_PAR_UNIT_NUMBER]      VARCHAR (1024)   NULL,
    [ZZ_PAR_POBOX]            VARCHAR (1024)   NULL,
    [IGRP_NO]                 UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Subscriptions_DQM] PRIMARY KEY CLUSTERED ([DQMID] ASC) WITH (FILLFACTOR = 70)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_Subscriptions_DQM_IGRP_NO]
    ON [dbo].[Subscriptions_DQM]([IGRP_NO] ASC) WITH (FILLFACTOR = 70);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_Subscriptions_DQM_SubscriptionID]
    ON [dbo].[Subscriptions_DQM]([SubscriptionID] ASC) WITH (FILLFACTOR = 70);
GO

