CREATE TABLE [dbo].[CompraItem] (
    [Id]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [Orden]                 INT             NOT NULL,
    [Cantidad]              INT             NOT NULL,
    [PrecioCosto]           DECIMAL (18, 2) NOT NULL,
    [Monto]                 DECIMAL (18, 2) NOT NULL,
    [MontoCalculado]        DECIMAL (18, 2) NOT NULL,
    [PrecioCompraCalculado] DECIMAL (18, 2) NOT NULL,
    [PrecioCompraComprado]  DECIMAL (18, 2) NOT NULL,
    [MontoComprado]         DECIMAL (18, 2) NOT NULL,
    [FechaAlta]             DATETIME        NOT NULL,
    [Compra_Id]             BIGINT          NOT NULL,
    [Producto_Id]           BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.CompraItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CompraItem_dbo.Compra_Compra_Id] FOREIGN KEY ([Compra_Id]) REFERENCES [dbo].[Compra] ([Id]),
    CONSTRAINT [FK_dbo.CompraItem_dbo.Producto_Producto_Id] FOREIGN KEY ([Producto_Id]) REFERENCES [dbo].[Producto] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Producto_Id]
    ON [dbo].[CompraItem]([Producto_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Compra_Id]
    ON [dbo].[CompraItem]([Compra_Id] ASC);

