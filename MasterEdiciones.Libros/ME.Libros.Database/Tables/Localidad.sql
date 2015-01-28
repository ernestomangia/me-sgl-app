CREATE TABLE [dbo].[Localidad] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]       NVARCHAR (100) NOT NULL,
    [Fecha_Alta]   DATETIME       NOT NULL,
    [Provincia_Id] INT            NOT NULL,
    CONSTRAINT [PK_dbo.Localidad] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Localidad_dbo.Provincia_Provincia_Id] FOREIGN KEY ([Provincia_Id]) REFERENCES [dbo].[Provincia] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Provincia_Id]
    ON [dbo].[Localidad]([Provincia_Id] ASC);

