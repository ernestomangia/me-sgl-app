CREATE TABLE [dbo].[Proveedor] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [RazonSocial]  NVARCHAR (100) NOT NULL,
    [Cuil]         NVARCHAR (13)  NOT NULL,
    [Direccion]    NVARCHAR (200) NULL,
    [TelefonoFijo] NVARCHAR (11)  NULL,
    [Celular]      NVARCHAR (11)  NULL,
    [Email]        NVARCHAR (100) NULL,
    [FechaAlta]    DATETIME       NOT NULL,
    [Localidad_Id] BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.Proveedor] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Proveedor_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[Proveedor]([Localidad_Id] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Cuil]
    ON [dbo].[Proveedor]([Cuil] ASC);

