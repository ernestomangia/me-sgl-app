CREATE TABLE [dbo].[Producto] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Nombre]       NVARCHAR (100)  NOT NULL,
    [Descripcion]  NVARCHAR (250)  NULL,
    [Stock]        BIGINT          NULL,
    [CodigoBarra]  NVARCHAR (13)   NULL,
    [PrecioCosto]  DECIMAL (18, 2) NOT NULL,
    [PrecioVenta]  DECIMAL (18, 2) NOT NULL,
    [FechaAlta]    DATETIME        NOT NULL,
    [Editorial_Id] BIGINT          NOT NULL,
    [Rubro_Id]     BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.Producto] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Producto_dbo.Editorial_Editorial_Id] FOREIGN KEY ([Editorial_Id]) REFERENCES [dbo].[Editorial] ([Id]),
    CONSTRAINT [FK_dbo.Producto_dbo.Rubro_Rubro_Id] FOREIGN KEY ([Rubro_Id]) REFERENCES [dbo].[Rubro] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Rubro_Id]
    ON [dbo].[Producto]([Rubro_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Editorial_Id]
    ON [dbo].[Producto]([Editorial_Id] ASC);

