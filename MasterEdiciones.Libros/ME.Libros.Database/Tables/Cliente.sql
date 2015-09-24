CREATE TABLE [dbo].[Cliente] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Codigo]          BIGINT         NOT NULL,
    [Nombre]          NVARCHAR (100) NOT NULL,
    [Apellido]        NVARCHAR (100) NOT NULL,
    [Dni]             BIGINT         NULL,
    [Cuil]            NVARCHAR (13)  NULL,
    [FechaNacimiento] DATETIME       NULL,
    [Direccion]       NVARCHAR (200) NOT NULL,
    [Numero]          NVARCHAR (MAX) NULL,
    [Comentario]      NVARCHAR (250) NULL,
    [TelefonoFijo]    NVARCHAR (11)  NULL,
    [Celular]         NVARCHAR (11)  NULL,
    [Celular2]        NVARCHAR (11)  NULL,
    [Email]           NVARCHAR (100) NULL,
    [FechaAlta]       DATETIME       NOT NULL,
    [Iva_Id]          BIGINT         NULL,
    [Localidad_Id]    BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.Cliente] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Cliente_dbo.Iva_Iva_Id] FOREIGN KEY ([Iva_Id]) REFERENCES [dbo].[Iva] ([Id]),
    CONSTRAINT [FK_dbo.Cliente_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[Cliente]([Localidad_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Iva_Id]
    ON [dbo].[Cliente]([Iva_Id] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Cuil]
    ON [dbo].[Cliente]([Cuil] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Codigo]
    ON [dbo].[Cliente]([Codigo] ASC);

