CREATE TABLE [dbo].[IncomingData] (
    [PUBCODE]                 VARCHAR (15)   NULL,
    [SEQUENCE]                FLOAT (53)     NULL,
    [CAT]                     FLOAT (53)     NULL,
    [XACT]                    FLOAT (53)     NULL,
    [XACTDATE]                SMALLDATETIME  NULL,
    [FNAME]                   VARCHAR (100)  NULL,
    [LNAME]                   VARCHAR (100)  NULL,
    [TITLE]                   VARCHAR (100)  NULL,
    [COMPANY]                 VARCHAR (100)  NULL,
    [ADDRESS]                 VARCHAR (255)  NULL,
    [MAILSTOP]                VARCHAR (255)  NULL,
    [CITY]                    VARCHAR (50)   NULL,
    [STATE]                   VARCHAR (50)   NULL,
    [ZIP]                     VARCHAR (15)   NULL,
    [PLUS4]                   VARCHAR (10)   NULL,
    [FORZIP]                  VARCHAR (15)   NULL,
    [COUNTY]                  VARCHAR (20)   NULL,
    [COUNTRY]                 VARCHAR (100)  NULL,
    [CTRY]                    VARCHAR (3)    NULL,
    [PHONE]                   VARCHAR (100)  NULL,
    [FAX]                     VARCHAR (100)  NULL,
    [EMAIL]                   VARCHAR (100)  NULL,
    [QDATE]                   SMALLDATETIME  NULL,
    [BUSINESS]                VARCHAR (1024) NULL,
    [FUNCTION]                VARCHAR (1024) NULL,
    [DEMO6]                   VARCHAR (10)   NULL,
    [DEMO7]                   VARCHAR (2)    NULL,
    [DEMO31]                  VARCHAR (1)    NULL,
    [DEMO32]                  VARCHAR (1)    NULL,
    [DEMO33]                  VARCHAR (1)    NULL,
    [DEMO34]                  VARCHAR (2)    NULL,
    [DEMO35]                  VARCHAR (10)   NULL,
    [DEMO36]                  VARCHAR (10)   NULL,
    [IGRP_CNT]                FLOAT (53)     NULL,
    [IGRP_RANK]               VARCHAR (2)    NULL,
    [TECHNIQUES]              VARCHAR (200)  NULL,
    [IVTSUB01]                VARCHAR (60)   NULL,
    [ADDRESS3]                VARCHAR (50)   NULL,
    [DEPARTMENT]              VARCHAR (30)   NULL,
    [PHONEEXT]                VARCHAR (8)    NULL,
    [WEBSITE]                 VARCHAR (50)   NULL,
    [SIC]                     VARCHAR (8)    NULL,
    [SOURCECODE]              VARCHAR (8)    NULL,
    [CATCODE]                 VARCHAR (55)   NULL,
    [INDYCODE]                VARCHAR (55)   NULL,
    [TITLE_CODE]              VARCHAR (4)    NULL,
    [MAIL]                    VARCHAR (1)    NULL,
    [CONF_ID]                 VARCHAR (10)   NULL,
    [LYFLAG]                  VARCHAR (1)    NULL,
    [REGCODE]                 VARCHAR (45)   NULL,
    [DEMO1]                   VARCHAR (165)  NULL,
    [DEMO4]                   VARCHAR (165)  NULL,
    [DEMO10]                  VARCHAR (165)  NULL,
    [PRDCDE]                  VARCHAR (1024) NULL,
    [HDUHEAR]                 VARCHAR (100)  NULL,
    [TOPICS]                  VARCHAR (1024) NULL,
    [EVENT_ID]                VARCHAR (1024) NULL,
    [FUNCTTLCDE]              VARCHAR (20)   NULL,
    [DEMO3]                   VARCHAR (50)   NULL,
    [DEMO5]                   VARCHAR (50)   NULL,
    [CLASS_CODE]              VARCHAR (20)   NULL,
    [FLDW]                    VARCHAR (100)  NULL,
    [SUB04ANS]                VARCHAR (1024) NULL,
    [PROCESS_CONTROL]         VARCHAR (20)   NULL,
    [PRODUCT_SERVICES]        VARCHAR (20)   NULL,
    [THERAPEUTIC_AREAS]       VARCHAR (50)   NULL,
    [PRODUCTS2]               VARCHAR (10)   NULL,
    [ZZ_MAF_ACTION]           VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_STD]        VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_MATCH1]     VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_MATCH2]     VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_MATCH3]     VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_MATCH4]     VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_MATCH5]     VARCHAR (1024) NULL,
    [ZZ_PAR_FNAME_MATCH6]     VARCHAR (1024) NULL,
    [ZZ_PAR_LNAME_STD]        VARCHAR (1024) NULL,
    [ZZ_PAR_TITLE_STD]        VARCHAR (1024) NULL,
    [ZZ_PAR_COMPANY_STD]      VARCHAR (1024) NULL,
    [ZZ_PAR_COMPANY2]         VARCHAR (1024) NULL,
    [ZZ_PAR_COMPANY_MATCH1]   VARCHAR (1024) NULL,
    [ZZ_PAR_COMPANY_MATCH2]   VARCHAR (1024) NULL,
    [ZZ_PAR_ADDRESS_STD]      VARCHAR (1024) NULL,
    [ZZ_PAR_MAILSTOP_STD]     VARCHAR (1024) NULL,
    [ZZ_PAR_CITY_STD]         VARCHAR (1024) NULL,
    [ZZ_PAR_STATE_STD]        VARCHAR (1024) NULL,
    [ZZ_PAR_ZIP_STD]          VARCHAR (1024) NULL,
    [ZZ_PAR_PLUS4_STD]        VARCHAR (1024) NULL,
    [ZZ_PAR_FORZIP_STD]       VARCHAR (1024) NULL,
    [ZZ_PAR_POSTCODE]         VARCHAR (1024) NULL,
    [ZZ_PAR_EMAIL_STD]        VARCHAR (1024) NULL,
    [ZZ_PAR_USCAN_PHONE]      VARCHAR (1024) NULL,
    [ZZ_PAR_INTL_PHONE]       VARCHAR (1024) NULL,
    [ZZ_PAR_PRIMARY_NUMBER]   VARCHAR (1024) NULL,
    [ZZ_PAR_PRIMARY_PREFIX]   VARCHAR (1024) NULL,
    [ZZ_PAR_PRIMARY_STREET]   VARCHAR (1024) NULL,
    [ZZ_PAR_PRIMARY_TYPE]     VARCHAR (1024) NULL,
    [ZZ_PAR_PRIMARY_POSTFIX]  VARCHAR (1024) NULL,
    [ZZ_PAR_RR_BOX]           VARCHAR (1024) NULL,
    [ZZ_PAR_RR_NUMBER]        VARCHAR (1024) NULL,
    [ZZ_PAR_UNIT_DESCRIPTION] VARCHAR (1024) NULL,
    [ZZ_PAR_UNIT_NUMBER]      VARCHAR (1024) NULL,
    [ZZ_PAR_POBOX]            VARCHAR (1024) NULL,
    [QSOURCE]                 VARCHAR (1)    NULL,
    [PAR3C]                   VARCHAR (1)    NULL,
    [EMAILSTATUS]             VARCHAR (256)  NULL,
    [IGRP_NO]                 VARCHAR (36)   NOT NULL,
    [PRODUCT_BRAND]           VARCHAR (1024) NULL,
    [PRODUCT_TYPE]            VARCHAR (1024) NULL,
    [REGCODE1]                VARCHAR (1024) NULL,
    [TYPE]                    VARCHAR (255)  NULL,
    [PUB_QUAL]                VARCHAR (1024) NULL,
    [PHEX_MBA]                VARCHAR (500)  NULL,
    [Gender]                  CHAR (1)       NULL
);


GO

GO

GO
CREATE NONCLUSTERED INDEX [IX_IncomingData_Pubcode]
    ON [dbo].[IncomingData]([PUBCODE] ASC)
    INCLUDE([IGRP_NO]);


GO
CREATE NONCLUSTERED INDEX [IX_IncomingData_IGRP_NO]
    ON [dbo].[IncomingData]([IGRP_NO] ASC);


GO
GRANT ALTER
    ON OBJECT::[dbo].[IncomingData] TO [webuser]
    AS [dbo];

