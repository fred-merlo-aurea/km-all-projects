﻿CREATE TABLE [dbo].[Menu] (
    [MenuID]           INT           IDENTITY (1, 1) NOT NULL,
    [ApplicationID]    INT           NOT NULL,
    [IsServiceFeature] BIT           CONSTRAINT [DF_Menu_IsServiceFeature] DEFAULT ((0)) NOT NULL,
    [ServiceFeatureID] INT           CONSTRAINT [DF_Menu_ServiceFeatureID] DEFAULT ((-1)) NOT NULL,
    [MenuName]         VARCHAR (50)  NOT NULL,
    [Description]      VARCHAR (500) NULL,
    [IsParent]         BIT           CONSTRAINT [DF_Menu_IsParent] DEFAULT ((1)) NOT NULL,
    [ParentMenuID]     INT           NOT NULL,
    [URL]              VARCHAR (250) NULL,
    [IsActive]         BIT           CONSTRAINT [DF_Menu_IsActive] DEFAULT ((1)) NOT NULL,
    [MenuOrder]        INT           CONSTRAINT [DF_Menu_MenuOrder] DEFAULT ((0)) NOT NULL,
    [HasFeatures]      BIT           CONSTRAINT [DF_Menu_HasFeatures] DEFAULT ((0)) NOT NULL,
    [ImagePath]        VARCHAR (250) NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT           NULL,
    [ServiceID] INT NULL, 
    [IsClientGroupService] BIT NULL, 
    [IsSysAdmin] BIT NULL, 
    [IsChannelAdmin] BIT NULL, 
    [IsCustomerAdmin] BIT NULL, 
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([MenuID] ASC)
);

