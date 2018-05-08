﻿CREATE TABLE [dbo].[AcsFileHeader]
(
	AcsFileHeaderId int identity(1,1) Primary Key,
	ClientId int,
	RecordType varchar(1),
	FileVersion varchar(2),
	CustomerID int,
	CreateDate date,
	ShipmentNumber bigint,
	TotalAcsRecordCount int,
	TotalCoaCount int,
	TotalNixieCount int,
	TrdRecordCount int,
	TrdAcsFeeAmount decimal(9,2),
	TrdCoaCount int,
	TrdCoaAcsFeeAmount decimal(9,2),
	TrdNixieCount int,
	TrdNixieAcsFeeAmount decimal(9,2),
	OcdRecordCount int,
	OcdAcsFeeAmount decimal(9,2),
	OcdCoaCount int,
	OcdCoaAcsFeeAmount decimal(9,2),
	OcdNixieCount int,
	OcdNixieAcsFeeAmount decimal(9,2),
	FsRecordCount int,
	FsAcsFeeAmount decimal(9,2),
	FsCoaCount int,
	FsCoaAcsFeeAmount decimal(9,2),
	FsNixieCount int,
	FsNixieAcsFeeAmount decimal(9,2),
	ImpbRecordCount int,
	ImpbAcsFeeAmount decimal(9,2),
	ImpbCoaCount int,
	ImpbCoaAcsFeeAmount decimal(9,2),
	ImpbNixieCount int,
	ImpbNixieAcsFeeAmount decimal(9,2),
	Filler varchar(405),
	EndMarker varchar(1),
	ProcessCode varchar(50)
)
