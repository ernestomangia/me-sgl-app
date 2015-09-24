CREATE TABLE [dbo].[Localidad] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Nombre]       NVARCHAR (100) NOT NULL,
    [CodigoPostal] NVARCHAR (5)   NULL,
    [FechaAlta]    DATETIME       NOT NULL,
    [Provincia_Id] BIGINT         NOT NULL,
    [Zona_Id]      BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.Localidad] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Localidad_dbo.Provincia_Provincia_Id] FOREIGN KEY ([Provincia_Id]) REFERENCES [dbo].[Provincia] ([Id]),
    CONSTRAINT [FK_dbo.Localidad_dbo.Zona_Zona_Id] FOREIGN KEY ([Zona_Id]) REFERENCES [dbo].[Zona] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Provincia_Id]
    ON [dbo].[Localidad]([Provincia_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Zona_Id]
    ON [dbo].[Localidad]([Zona_Id] ASC);

