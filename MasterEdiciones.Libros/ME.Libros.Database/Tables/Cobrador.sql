﻿CREATE TABLE [dbo].[Cobrador] (
    [Id]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [Nombre]             NVARCHAR (100)  NOT NULL,
    [Apellido]           NVARCHAR (100)  NOT NULL,
    [Dni]                BIGINT          NOT NULL,
    [Direccion]          NVARCHAR (100)  NULL,
    [TelefonoFijo]       NVARCHAR (MAX)  NULL,
    [Celular]            NVARCHAR (MAX)  NULL,
    [Email]              NVARCHAR (MAX)  NULL,
    [PorcentajeComision] DECIMAL (18, 2) NOT NULL,
    [FechaAlta]          DATETIME        NOT NULL,
    [Localidad_Id]       BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.Cobrador] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Cobrador_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[Cobrador]([Localidad_Id] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Dni]
    ON [dbo].[Cobrador]([Dni] ASC);

