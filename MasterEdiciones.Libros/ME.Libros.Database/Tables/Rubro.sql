﻿CREATE TABLE [dbo].[Rubro] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (80)  NOT NULL,
    [Descripcion] NVARCHAR (256) NULL,
    [FechaAlta]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Rubro] PRIMARY KEY CLUSTERED ([Id] ASC)
);

