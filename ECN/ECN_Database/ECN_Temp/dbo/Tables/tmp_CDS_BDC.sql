﻿CREATE TABLE [dbo].[tmp_CDS_BDC] (
    [PUB]       VARCHAR (50)  NULL,
    [SUB]       VARCHAR (50)  NULL,
    [INM]       VARCHAR (50)  NULL,
    [TTL]       VARCHAR (50)  NULL,
    [CNM]       VARCHAR (50)  NULL,
    [AD1]       VARCHAR (50)  NULL,
    [AD2]       VARCHAR (50)  NULL,
    [CTY]       VARCHAR (50)  NULL,
    [ST]        VARCHAR (50)  NULL,
    [ZIP]       VARCHAR (50)  NULL,
    [CNT]       VARCHAR (50)  NULL,
    [PHN]       VARCHAR (50)  NULL,
    [FAX]       VARCHAR (50)  NULL,
    [CEL]       VARCHAR (50)  NULL,
    [EML]       VARCHAR (50)  NOT NULL,
    [RF1]       VARCHAR (50)  NULL,
    [CRD]       VARCHAR (50)  NULL,
    [UPD]       VARCHAR (50)  NULL,
    [VDT]       VARCHAR (50)  NULL,
    [ED]        VARCHAR (50)  NULL,
    [NMS]       VARCHAR (50)  NULL,
    [PRM]       VARCHAR (50)  NULL,
    [DMO001]    VARCHAR (50)  NULL,
    [DMO003]    VARCHAR (50)  NULL,
    [DMO005]    VARCHAR (50)  NULL,
    [DMO007]    VARCHAR (50)  NULL,
    [DMO009]    VARCHAR (50)  NULL,
    [DMO011]    VARCHAR (50)  NULL,
    [DMO013]    VARCHAR (50)  NULL,
    [DMO015]    VARCHAR (50)  NULL,
    [DMO017]    VARCHAR (50)  NULL,
    [DMO019]    VARCHAR (50)  NULL,
    [DMO021]    VARCHAR (50)  NULL,
    [DMO023]    VARCHAR (50)  NULL,
    [DMO025]    VARCHAR (50)  NULL,
    [DMO027]    VARCHAR (50)  NULL,
    [DMO029]    VARCHAR (50)  NULL,
    [DMO031]    VARCHAR (50)  NULL,
    [DMO033]    VARCHAR (50)  NULL,
    [DMO035]    VARCHAR (50)  NULL,
    [DMO037]    VARCHAR (50)  NULL,
    [DMO047]    VARCHAR (50)  NULL,
    [DMO059]    VARCHAR (50)  NULL,
    [DMO099]    VARCHAR (50)  NULL,
    [DMO101]    VARCHAR (50)  NULL,
    [DMO103]    VARCHAR (50)  NULL,
    [DMO105]    VARCHAR (50)  NULL,
    [DMO119]    VARCHAR (50)  NULL,
    [DMO121]    VARCHAR (50)  NULL,
    [DMO123]    VARCHAR (50)  NULL,
    [DMO129]    VARCHAR (50)  NULL,
    [DMO131]    VARCHAR (50)  NULL,
    [DMO133]    VARCHAR (50)  NULL,
    [DMO201]    VARCHAR (50)  NULL,
    [DMO203]    VARCHAR (50)  NULL,
    [DMO205]    VARCHAR (50)  NULL,
    [DMO207]    VARCHAR (50)  NULL,
    [DMO209]    VARCHAR (50)  NULL,
    [DMO211]    VARCHAR (50)  NULL,
    [firstname] VARCHAR (500) NULL,
    [Lastname]  VARCHAR (500) NULL,
    [forzip]    VARCHAR (50)  NULL,
    [CatID]     VARCHAR (50)  NULL,
    [Xact]      VARCHAR (50)  NULL,
    [Demo7]     VARCHAR (1)   NULL,
    CONSTRAINT [PK_tmp_CDS_BDC] PRIMARY KEY CLUSTERED ([EML] ASC)
);
